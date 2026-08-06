using ClosedXML.Excel;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Costs;

// Filters accepted by ReportCostOfLines (documented header filters, §6.11).
public class CostFilters
{
    public int? SupplierId { get; set; }
    public int? LineId { get; set; }
    public string? SerialBus { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
}

public class CostRow
{
    public string LineName { get; set; } = string.Empty;
    public string Serial { get; set; } = string.Empty;
    public string NameOfRoute { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public double LineCost { get; set; }
    public bool OneWay { get; set; }
    public int CountOfround { get; set; } // frozen odd-casing key
    public double DeductPerRound { get; set; }
    public int DeductRoundNum { get; set; }
    public double NetCost { get; set; }
}

public class CostsService
{
    private readonly AppDbContext _db;

    public CostsService(AppDbContext db) => _db = db;

    /// <summary>Build an inclusive [dateFrom, dateTo] day-range filter, or (null, null) for no bound.</summary>
    private static (DateTime? Gte, DateTime? Lt) RoundDateFilter(string? dateFrom, string? dateTo)
    {
        DateTime? gte = null, lt = null;
        if (DateTime.TryParse(dateFrom, out var f)) gte = f.Date;
        if (DateTime.TryParse(dateTo, out var t)) lt = t.Date.AddDays(1); // inclusive of the To calendar day
        return (gte, lt);
    }

    /// <summary>
    /// Compute one TransportationReportCostData row for a route (§7.5 Cost report):
    ///   two-way: rounds = checkIns + checkOuts ; NetCost = (LineCost/2)*rounds - deductions
    ///   one-way: rounds = checkIns             ; NetCost =  LineCost   *rounds - deductions
    /// </summary>
    private async Task<CostRow> CostRowForRoute(TransportationVehicleRoute r, (DateTime? Gte, DateTime? Lt) dateCond)
    {
        var oneWay = r.OneWay ?? false;

        var checkInQuery = _db.TransportationRoundForRoutes.Where(x => x.RouteId == r.Id && x.CheckInDate != null);
        if (dateCond.Gte.HasValue) checkInQuery = checkInQuery.Where(x => x.CheckInDate >= dateCond.Gte.Value);
        if (dateCond.Lt.HasValue) checkInQuery = checkInQuery.Where(x => x.CheckInDate < dateCond.Lt.Value);
        var checkIns = await checkInQuery.CountAsync();

        var checkOuts = 0;
        if (!oneWay)
        {
            var checkOutQuery = _db.TransportationRoundForRoutes.Where(x => x.RouteId == r.Id && x.CheckOutDate != null);
            if (dateCond.Gte.HasValue) checkOutQuery = checkOutQuery.Where(x => x.CheckOutDate >= dateCond.Gte.Value);
            if (dateCond.Lt.HasValue) checkOutQuery = checkOutQuery.Where(x => x.CheckOutDate < dateCond.Lt.Value);
            checkOuts = await checkOutQuery.CountAsync();
        }

        var dedQuery = _db.TransportationVehicleRouteDeductions.Where(d => d.RouteId == r.Id);
        if (dateCond.Gte.HasValue) dedQuery = dedQuery.Where(d => d.DateOfDeduction >= dateCond.Gte.Value);
        if (dateCond.Lt.HasValue) dedQuery = dedQuery.Where(d => d.DateOfDeduction < dateCond.Lt.Value);
        var deductions = await dedQuery.OrderByDescending(d => d.DateOfDeduction).Select(d => d.DeductPerRound).ToListAsync();

        var countOfRound = oneWay ? checkIns : checkIns + checkOuts;
        var deductRoundNum = deductions.Count;
        var deductPerRound = deductRoundNum > 0 ? deductions[0] : 0; // most recent row's value
        var deductionsTotal = deductions.Sum();
        var netCost = oneWay
            ? r.LineCost * countOfRound - deductionsTotal
            : (r.LineCost / 2) * countOfRound - deductionsTotal;

        return new CostRow
        {
            LineName = r.Line?.LineName ?? string.Empty,
            Serial = r.Serial ?? string.Empty,
            NameOfRoute = r.NameOfRoute ?? string.Empty,
            SupplierName = r.Supplier?.Name ?? string.Empty,
            LineCost = r.LineCost,
            OneWay = oneWay,
            CountOfround = countOfRound,
            DeductPerRound = deductPerRound,
            DeductRoundNum = deductRoundNum,
            NetCost = netCost,
        };
    }

    private IQueryable<TransportationVehicleRoute> BuildQuery(CostFilters filters)
    {
        var q = _db.TransportationVehicleRoutes.Where(r => r.Active);
        if (filters.LineId.HasValue) q = q.Where(r => r.TransportationLineId == filters.LineId.Value);
        if (filters.SupplierId.HasValue) q = q.Where(r => r.SupplierId == filters.SupplierId.Value);
        if (!string.IsNullOrEmpty(filters.SerialBus)) q = q.Where(r => r.Serial == filters.SerialBus);
        return q;
    }

    /// <summary>GET ReportCostOfLines — paginated TransportationReportCostData rows.</summary>
    public async Task<object> ReportCostOfLines(int pageNo, int noOfItems, CostFilters filters)
    {
        var query = BuildQuery(filters);
        var total = await query.CountAsync();
        var rows = await query
            .Include(r => r.Line).Include(r => r.Supplier)
            .OrderByDescending(r => r.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var dateCond = RoundDateFilter(filters.DateFrom, filters.DateTo);
        var data = new List<CostRow>();
        foreach (var r in rows) data.Add(await CostRowForRoute(r, dateCond));

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET ReportCostOfLinesExcell — base64 (.xlsx) costs export (RTL, Arabic headers).</summary>
    public async Task<object> ReportCostOfLinesExcel(CostFilters filters)
    {
        var rows = await BuildQuery(filters)
            .Include(r => r.Line).Include(r => r.Supplier)
            .OrderByDescending(r => r.Id)
            .ToListAsync();
        var dateCond = RoundDateFilter(filters.DateFrom, filters.DateTo);
        var data = new List<CostRow>();
        foreach (var r in rows) data.Add(await CostRowForRoute(r, dateCond));

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("تكلفة الخطوط");
        sheet.RightToLeft = true;
        string[] headers = { "اسم الخط", "الرقم المسلسل", "خط السير", "المورد", "تكلفة الخط", "اتجاه واحد", "عدد الجولات", "الخصم لكل جولة", "عدد جولات الخصم", "صافي التكلفة" };
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var d in data)
        {
            sheet.Cell(row, 1).Value = d.LineName;
            sheet.Cell(row, 2).Value = d.Serial;
            sheet.Cell(row, 3).Value = d.NameOfRoute;
            sheet.Cell(row, 4).Value = d.SupplierName;
            sheet.Cell(row, 5).Value = d.LineCost;
            sheet.Cell(row, 6).Value = d.OneWay ? "نعم" : "لا";
            sheet.Cell(row, 7).Value = d.CountOfround;
            sheet.Cell(row, 8).Value = d.DeductPerRound;
            sheet.Cell(row, 9).Value = d.DeductRoundNum;
            sheet.Cell(row, 10).Value = d.NetCost;
            row++;
        }

        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }
}
