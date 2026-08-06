using ClosedXML.Excel;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Deductions;

// Shape of the Add/Update deduction payload (documented PascalCase keys, §6.10).
public class DeductionBody
{
    public int? Id { get; set; }
    public int? SupplierId { get; set; }
    public string? Serial { get; set; }
    public int? TransportationVehicleRouteId { get; set; }
    public double? DeductPerRound { get; set; }
    public string? Cause { get; set; }
    public string? DateOfDeduction { get; set; }
    public string? TypeOfDedct { get; set; } // Taxes | Normal
}

public class DeductionFilters
{
    public int? SupplierId { get; set; }
    public int? RouteId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? Name { get; set; } // route name search
}

public class DeductionsService
{
    private readonly AppDbContext _db;

    public DeductionsService(AppDbContext db) => _db = db;

    /// <summary>Resolve a set of creator (User) ids → their full names, in one query.</summary>
    private async Task<Dictionary<int, string>> CreatorNames(IEnumerable<int?> ids)
    {
        var unique = ids.Where(i => i.HasValue).Select(i => i!.Value).Distinct().ToList();
        var map = new Dictionary<int, string>();
        if (unique.Count == 0) return map;
        var users = await _db.Users.Where(u => unique.Contains(u.Id)).ToListAsync();
        foreach (var u in users)
        {
            var name = string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));
            map[u.Id] = string.IsNullOrEmpty(name) ? u.Email : name;
        }
        return map;
    }

    private IQueryable<TransportationVehicleRouteDeduction> BuildQuery(DeductionFilters filters)
    {
        var q = _db.TransportationVehicleRouteDeductions.AsQueryable();
        if (filters.SupplierId.HasValue) q = q.Where(d => d.SupplierId == filters.SupplierId.Value);
        if (filters.RouteId.HasValue) q = q.Where(d => d.RouteId == filters.RouteId.Value);
        if (!string.IsNullOrEmpty(filters.Name)) q = q.Where(d => d.Route!.NameOfRoute.Contains(filters.Name));
        var from = ParseDate(filters.FromDate);
        var to = ParseDate(filters.ToDate);
        if (from.HasValue) q = q.Where(d => d.DateOfDeduction >= from.Value);
        if (to.HasValue) q = q.Where(d => d.DateOfDeduction <= to.Value);
        return q;
    }

    /// <summary>GET getAllDeduction — paginated deductions (TransportationDeductionData rows).</summary>
    public async Task<object> GetAll(int pageNo, int noOfItems, DeductionFilters filters)
    {
        var query = BuildQuery(filters);
        var total = await query.CountAsync();
        var rows = await query
            .Include(d => d.Supplier)
            .Include(d => d.Route).ThenInclude(r => r!.ContactPerson)
            .OrderByDescending(d => d.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var creators = await CreatorNames(rows.Select(d => d.CreationBy));

        var data = rows.Select(d => new
        {
            Id = d.Id,
            SupplierName = d.Supplier?.Name ?? string.Empty,
            SupplierId = d.SupplierId ?? 0,
            TransportationVehicleRouteName = d.Route?.NameOfRoute ?? string.Empty,
            TransportationVehicleRouteId = d.RouteId,
            Serial = string.IsNullOrEmpty(d.Serial) ? "0" : d.Serial,
            Date = Fmt.Iso(d.DateOfDeduction),
            CreationDate = Fmt.Iso(d.CreationDate),
            DeductPerRound = d.DeductPerRound,
            Cause = d.Cause ?? string.Empty,
            CreationName = d.CreationBy != null ? creators.GetValueOrDefault(d.CreationBy.Value, string.Empty) : string.Empty,
            CreationBy = d.CreationBy ?? 0,
            SupplierContentPersonIdName = d.Route?.ContactPerson?.Name ?? string.Empty, // frozen typo key (Content vs Contact)
            FromDate = Fmt.Iso(d.FromDate),
            ToDate = Fmt.Iso(d.ToDate),
            typeOfDeduction = d.TypeOfDedct ?? string.Empty, // lowercase frozen key
            // routePrice is the route's CURRENT LineCost (dynamic), not a stored column.
            routePrice = d.Route?.LineCost ?? d.RoutePrice ?? 0, // lowercase frozen key
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET getAllDeductionExcel — the filtered deductions as a base64 (.xlsx) file.</summary>
    public async Task<object> Excel(DeductionFilters filters)
    {
        var rows = await BuildQuery(filters)
            .Include(d => d.Supplier)
            .Include(d => d.Route).ThenInclude(r => r!.ContactPerson)
            .OrderByDescending(d => d.Id)
            .ToListAsync();

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("الخصومات");
        sheet.RightToLeft = true;
        string[] headers = { "الخط", "المورد", "السيريال", "التاريخ", "قيمة الخصم", "النوع", "السبب" };
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var d in rows)
        {
            sheet.Cell(row, 1).Value = d.Route?.NameOfRoute ?? string.Empty;
            sheet.Cell(row, 2).Value = d.Supplier?.Name ?? string.Empty;
            sheet.Cell(row, 3).Value = d.Serial ?? string.Empty;
            sheet.Cell(row, 4).Value = Fmt.IsoDate(d.DateOfDeduction);
            sheet.Cell(row, 5).Value = d.DeductPerRound;
            sheet.Cell(row, 6).Value = (d.TypeOfDedct ?? string.Empty).ToLowerInvariant() == "taxes" ? "ضرائب" : "عادي";
            sheet.Cell(row, 7).Value = d.Cause ?? string.Empty;
            row++;
        }

        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>POST AddDeduction — create a deduction. creationBy = caller.</summary>
    public async Task<object> Add(DeductionBody body, int userId)
    {
        var routeId = body.TransportationVehicleRouteId ?? 0;
        if (routeId == 0) return Envelope.Fail("Err101", "TransportationVehicleRouteId is required");
        var entity = new TransportationVehicleRouteDeduction();
        Apply(entity, body, routeId);
        entity.CreationBy = userId;
        _db.TransportationVehicleRouteDeductions.Add(entity);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(entity.Id);
    }

    /// <summary>POST UpdateDeduction — update a deduction. Preserves the original creator.</summary>
    public async Task<object> Update(int id, DeductionBody body)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var entity = await _db.TransportationVehicleRouteDeductions.FirstOrDefaultAsync(d => d.Id == id);
        if (entity == null) return Envelope.Fail("Err404", "Deduction not found");
        var routeId = body.TransportationVehicleRouteId ?? 0;
        Apply(entity, body, routeId);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : null);

    /// <summary>Maps the documented payload to the deduction model columns (no creator).</summary>
    private static void Apply(TransportationVehicleRouteDeduction e, DeductionBody body, int routeId)
    {
        e.SupplierId = body.SupplierId;
        e.RouteId = routeId;
        e.Serial = body.Serial ?? string.Empty;
        e.DateOfDeduction = ParseDate(body.DateOfDeduction) ?? DateTime.UtcNow;
        e.DeductPerRound = body.DeductPerRound ?? 0;
        e.Cause = body.Cause;
        e.TypeOfDedct = body.TypeOfDedct ?? "Normal";
    }
}
