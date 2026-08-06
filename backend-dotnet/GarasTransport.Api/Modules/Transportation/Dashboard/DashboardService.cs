using System.Globalization;
using ClosedXML.Excel;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;
using UserEntity = GarasTransport.Domain.Entities.User;

namespace GarasTransport.Api.Modules.Transportation.Dashboard;

// Body of AddUsersAttedance (documented PascalCase keys, §6.9). CheckInOrCheckOut
// is a flag: true → record a check-in, false → record a check-out.
public class AddAttendanceBody
{
    public string? type { get; set; }
    public string? serial { get; set; }
    public bool? CheckInOrCheckOut { get; set; }
    public double? CheckInLatitude { get; set; }
    public double? CheckInLongtitud { get; set; } // frozen typo key
    public double? CheckOutLatitude { get; set; }
    public double? CheckOutLongtitud { get; set; } // frozen typo key
    public int? CheckInRouteDirectionId { get; set; }
    public int? CheckOutRouteDirectionId { get; set; }
}

public class DashboardFilters
{
    public int? LineId { get; set; }
    public int? SupplierId { get; set; }
    public string? SerialBus { get; set; }
    public int? RouteId { get; set; }
}

public class AttendanceFilters : DashboardFilters
{
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? AttendanceFlag { get; set; }
    public string? EmployeeId { get; set; } // fingerprint no. stored in HrUser.Email
}

public class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db) => _db = db;

    private static (DateTime Gte, DateTime Lt) DayRange(DateTime d)
    {
        var gte = d.Date;
        return (gte, gte.AddDays(1));
    }

    private static string FullName(UserEntity? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    /// <summary>GET DashBoard — single TransportationDashboardData object (every value a string).</summary>
    public async Task<object> Dashboard(string? dateSearch, DashboardFilters filters)
    {
        if (string.IsNullOrWhiteSpace(dateSearch)) return Envelope.Fail("Err101", "يرجى إدخال التاريخ");

        var routeQuery = _db.TransportationVehicleRoutes.Where(r => r.Active);
        if (filters.LineId.HasValue) routeQuery = routeQuery.Where(r => r.TransportationLineId == filters.LineId.Value);
        if (filters.SupplierId.HasValue) routeQuery = routeQuery.Where(r => r.SupplierId == filters.SupplierId.Value);
        if (!string.IsNullOrEmpty(filters.SerialBus)) routeQuery = routeQuery.Where(r => r.Serial == filters.SerialBus);
        if (filters.RouteId.HasValue) routeQuery = routeQuery.Where(r => r.Id == filters.RouteId.Value);

        var scopeRoutes = await routeQuery.Select(r => new
        {
            r.Id,
            r.Serial,
            r.TransportationLineId,
            r.TransportationVehicleId,
            r.SupplierId,
            r.OneWay,
            VehicleTypeId = (int?)(r.Vehicle != null ? r.Vehicle.VehicleTypeId : (int?)null),
        }).ToListAsync();

        var routeIds = scopeRoutes.Select(r => r.Id).ToList();
        static int DistinctCount(IEnumerable<int?> xs) => xs.Where(x => x != null).Select(x => x!.Value).Distinct().Count();

        var transportLinesNum = DistinctCount(scopeRoutes.Select(r => (int?)r.TransportationLineId));
        var vehiclesNum = DistinctCount(scopeRoutes.Select(r => (int?)r.TransportationVehicleId));
        var suppliersNum = DistinctCount(scopeRoutes.Select(r => r.SupplierId));
        var allSuppliersNum = suppliersNum;
        var vehicleTypeNum = DistinctCount(scopeRoutes.Select(r => r.VehicleTypeId));
        var twoWayVehiclesNum = scopeRoutes.Count(r => !(r.OneWay ?? false));
        var oneWayVehiclesNum = scopeRoutes.Count(r => r.OneWay ?? false);

        if (!DateTime.TryParse(dateSearch, out var parsed)) return Envelope.Fail("Err101", "يرجى إدخال تاريخ صحيح");
        var range = DayRange(parsed);

        var scopeSerials = scopeRoutes.Select(r => r.Serial).Where(s => !string.IsNullOrEmpty(s)).ToList();
        var busQuery = _db.UserAttendances.Where(a => a.Type == "Bus" && scopeSerials.Contains(a.Serial));
        var checkInBuses = await busQuery.CountAsync(a => a.CheckIn >= range.Gte && a.CheckIn < range.Lt);
        var checkOutBuses = await busQuery.CountAsync(a => a.CheckOut >= range.Gte && a.CheckOut < range.Lt);
        var oneWayBuses = await busQuery.CountAsync(a => a.OneWayDate >= range.Gte && a.OneWayDate < range.Lt);

        var activeMembers = await _db.TransportationVehicleRouteEmployees
            .Where(m => m.Active && routeIds.Contains(m.RouteId))
            .Select(m => new { m.HrUserId, Email = m.HrUser!.Email })
            .ToListAsync();
        var allMemberUserIds = await _db.TransportationVehicleRouteEmployees
            .Where(m => routeIds.Contains(m.RouteId))
            .Select(m => m.HrUserId)
            .ToListAsync();
        var hrUsersNum = activeMembers.Select(m => m.HrUserId).Distinct().Count();
        var allHrUsersNum = allMemberUserIds.Distinct().Count();

        var riderEmailByUser = new Dictionary<int, string>();
        foreach (var m in activeMembers)
            if (!string.IsNullOrEmpty(m.Email)) riderEmailByUser[m.HrUserId] = m.Email!;
        var riderEmails = riderEmailByUser.Values.Distinct().ToList();

        var attendedSerials = new HashSet<string>();
        if (riderEmails.Count > 0)
        {
            var attended = await _db.UserAttendances
                .Where(a => a.Type == "Person" && a.CheckIn >= range.Gte && a.CheckIn < range.Lt && riderEmails.Contains(a.Serial))
                .Select(a => a.Serial)
                .Distinct()
                .ToListAsync();
            attendedSerials = attended.ToHashSet();
        }
        var hrUsersAttendance = riderEmailByUser.Values.Count(email => attendedSerials.Contains(email));
        var ridersInScope = hrUsersNum;

        static string Pct(int n, int den) =>
            den > 0 ? (Math.Round(n / (double)den * 1000, MidpointRounding.AwayFromZero) / 10).ToString(CultureInfo.InvariantCulture) : "0";

        var data = new
        {
            TransportLinesNum = transportLinesNum.ToString(),
            VehiclesNum = vehiclesNum.ToString(),
            TwoWayVehiclesNum = twoWayVehiclesNum.ToString(),
            OneWayVehiclesNum = oneWayVehiclesNum.ToString(),
            HrUsersNum = hrUsersNum.ToString(),
            SuppliersNum = suppliersNum.ToString(),
            HrUsersPercent = Pct(hrUsersAttendance, ridersInScope),
            CheckInVehiclePercent = Pct(checkInBuses, twoWayVehiclesNum),
            CheckOutVehiclePercent = Pct(checkOutBuses, twoWayVehiclesNum),
            OneWayVehiclePercent = Pct(oneWayBuses, twoWayVehiclesNum),
            CheckInVehicleAttendanceNum = checkInBuses.ToString(), // frozen singular Vehicle key
            CheckOutVehicleAttendanceNum = checkOutBuses.ToString(),
            OneWayVehicleAttendanceNum = oneWayBuses.ToString(),
            HrUsersAttendanceNum = hrUsersAttendance.ToString(),
            AllHrUsersNum = allHrUsersNum.ToString(),
            AllSuppliersNum = allSuppliersNum.ToString(),
            VehiclesTypeNum = vehicleTypeNum.ToString(), // frozen key VehiclesTypeNum
        };

        return Envelope.Success(data);
    }

    private IQueryable<TransportationVehicleRouteEmployee> AttendanceQuery(AttendanceFilters filters)
    {
        var q = _db.TransportationVehicleRouteEmployees.Where(m => m.Active);
        if (filters.RouteId.HasValue) q = q.Where(m => m.RouteId == filters.RouteId.Value);
        if (!string.IsNullOrEmpty(filters.EmployeeId))
            q = q.Where(m => m.HrUser!.Email != null && m.HrUser.Email.Contains(filters.EmployeeId));
        if (filters.LineId.HasValue) q = q.Where(m => m.Route!.TransportationLineId == filters.LineId.Value);
        if (filters.SupplierId.HasValue) q = q.Where(m => m.Route!.SupplierId == filters.SupplierId.Value);
        if (!string.IsNullOrEmpty(filters.SerialBus)) q = q.Where(m => m.Route!.Serial == filters.SerialBus);
        return q;
    }

    private IQueryable<TransportationVehicleRouteEmployee> WithAttendanceIncludes(IQueryable<TransportationVehicleRouteEmployee> q) =>
        q.Include(m => m.HrUser).ThenInclude(h => h!.MaritalStatus)
         .Include(m => m.Route).ThenInclude(r => r!.Line)
         .Include(m => m.Route).ThenInclude(r => r!.Supplier)
         .Include(m => m.Route).ThenInclude(r => r!.ContactPerson)
         .Include(m => m.Route).ThenInclude(r => r!.Supervisor)
         .Include(m => m.Route).ThenInclude(r => r!.Vehicle).ThenInclude(v => v!.VehicleType);

    /// <summary>GET DashBoardForAttendenceDuration — paginated attendance rows with per-day history.</summary>
    public async Task<object> DashboardAttendance(int pageNo, int noOfItems, AttendanceFilters filters)
    {
        var query = AttendanceQuery(filters);
        var total = await query.CountAsync();
        var rows = await WithAttendanceIncludes(query)
            .OrderByDescending(m => m.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        DateTime? lower = DateTime.TryParse(filters.FromDate, out var from) ? from.Date : null;
        DateTime? upper = DateTime.TryParse(filters.ToDate, out var to) ? to.Date.AddDays(1) : null; // inclusive-of-day
        bool hasBounds = lower.HasValue || upper.HasValue;

        var serials = rows.Select(m => m.HrUser?.Email).Where(e => !string.IsNullOrEmpty(e)).Select(e => e!).Distinct().ToList();
        var attRows = serials.Count > 0
            ? await _db.UserAttendances.Where(a => a.Type == "Person" && serials.Contains(a.Serial)).OrderBy(a => a.Id).ToListAsync()
            : new List<UserAttendance>();

        bool InCond(DateTime? d) =>
            hasBounds
                ? d.HasValue && (!lower.HasValue || d.Value >= lower.Value) && (!upper.HasValue || d.Value < upper.Value)
                : d.HasValue;

        // Group attendance rows into one entry per serial+day.
        var historyBySerial = new Dictionary<string, Dictionary<string, AttendanceEntry>>();
        foreach (var a in attRows)
        {
            if (!(InCond(a.CheckIn) || InCond(a.CheckOut) || InCond(a.OneWayDate))) continue;
            var anchor = a.CheckIn ?? a.OneWayDate ?? a.CheckOut;
            if (!anchor.HasValue) continue;
            if (!historyBySerial.TryGetValue(a.Serial, out var perDay))
                historyBySerial[a.Serial] = perDay = new Dictionary<string, AttendanceEntry>();
            var key = $"{anchor.Value.Year}-{anchor.Value.Month}-{anchor.Value.Day}";
            if (!perDay.TryGetValue(key, out var entry))
            {
                entry = new AttendanceEntry { Date = Fmt.Iso(anchor.Value.Date), CheckIn = string.Empty, CheckOut = string.Empty, ISAttendace = false };
                perDay[key] = entry;
            }
            if (a.CheckIn.HasValue)
            {
                entry.CheckIn = Fmt.Iso(a.CheckIn);
                entry.ISAttendace = true; // has a check-in that day
            }
            if (a.CheckOut.HasValue) entry.CheckOut = Fmt.Iso(a.CheckOut);
        }

        List<AttendanceEntry> HistoryFor(string? email) =>
            !string.IsNullOrEmpty(email) && historyBySerial.TryGetValue(email, out var perDay)
                ? perDay.Values.OrderBy(x => x.Date, StringComparer.Ordinal).ToList()
                : new List<AttendanceEntry>();

        var data = rows.Select(m => new
        {
            Id = m.HrUserId.ToString(),
            EmployeeId = m.HrUser?.Email ?? string.Empty,
            FirstName = m.HrUser?.FirstName ?? string.Empty,
            MiddleName = m.HrUser?.MiddleName ?? string.Empty,
            LastName = m.HrUser?.LastName ?? string.Empty,
            TransportionlineName = m.Route?.Line?.LineName ?? string.Empty, // frozen typo key
            NameOfRoute = m.Route?.NameOfRoute ?? string.Empty,
            SupplierName = m.Route?.Supplier?.Name ?? string.Empty,
            supplierContactPersonName = m.Route?.ContactPerson?.Name ?? string.Empty, // frozen lowercase key
            VehicleType = m.Route?.Vehicle?.VehicleType?.Type ?? string.Empty,
            SupervisorName = FullName(m.Route?.Supervisor),
            MaritalStatus = m.HrUser?.MaritalStatus?.Name ?? string.Empty,
            AttendanceHistory = HistoryFor(m.HrUser?.Email),
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET AttendanceExcell — base64 (.xlsx) attendance roster export (RTL, Arabic headers).</summary>
    public async Task<object> AttendanceExcel(AttendanceFilters filters)
    {
        var rows = await WithAttendanceIncludes(AttendanceQuery(filters))
            .OrderByDescending(m => m.Id)
            .ToListAsync();

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("الحضور");
        sheet.RightToLeft = true;
        string[] headers = { "الاسم الأول", "الاسم الأوسط", "الاسم الأخير", "رقم هوية الراكب", "الخط", "خط السير", "المورد", "الشخص المسؤول", "نوع المركبة", "المشرف", "الحالة الاجتماعية" };
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var m in rows)
        {
            sheet.Cell(row, 1).Value = m.HrUser?.FirstName ?? string.Empty;
            sheet.Cell(row, 2).Value = m.HrUser?.MiddleName ?? string.Empty;
            sheet.Cell(row, 3).Value = m.HrUser?.LastName ?? string.Empty;
            sheet.Cell(row, 4).Value = m.HrUser?.Email ?? string.Empty;
            sheet.Cell(row, 5).Value = m.Route?.Line?.LineName ?? string.Empty;
            sheet.Cell(row, 6).Value = m.Route?.NameOfRoute ?? string.Empty;
            sheet.Cell(row, 7).Value = m.Route?.Supplier?.Name ?? string.Empty;
            sheet.Cell(row, 8).Value = m.Route?.ContactPerson?.Name ?? string.Empty;
            sheet.Cell(row, 9).Value = m.Route?.Vehicle?.VehicleType?.Type ?? string.Empty;
            sheet.Cell(row, 10).Value = FullName(m.Route?.Supervisor);
            sheet.Cell(row, 11).Value = m.HrUser?.MaritalStatus?.Name ?? string.Empty;
            row++;
        }

        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>POST AddUsersAttedance — record a check-in or check-out for a serial.</summary>
    public async Task<object> AddUsersAttendance(AddAttendanceBody body, int userId)
    {
        if (string.IsNullOrWhiteSpace(body.serial)) return Envelope.Fail("Err101", "serial is required");
        var type = body.type ?? "Person";
        var serial = body.serial!;
        var now = DateTime.UtcNow;
        var isCheckIn = body.CheckInOrCheckOut ?? false;

        if (isCheckIn)
        {
            _db.UserAttendances.Add(new UserAttendance
            {
                Type = type,
                Serial = serial,
                CheckIn = now,
                CheckInRouteId = body.CheckInRouteDirectionId,
                CheckInLatitude = body.CheckInLatitude,
                CheckInLongitude = body.CheckInLongtitud,
                CreationBy = userId,
            });
            await _db.SaveChangesAsync();
        }
        else
        {
            var open = await _db.UserAttendances.Where(a => a.Serial == serial && a.CheckOut == null)
                .OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            if (open != null)
            {
                open.CheckOut = now;
                open.CheckOutRouteId = body.CheckOutRouteDirectionId;
                open.CheckOutLatitude = body.CheckOutLatitude;
                open.CheckOutLongitude = body.CheckOutLongtitud;
            }
            else
            {
                _db.UserAttendances.Add(new UserAttendance
                {
                    Type = type,
                    Serial = serial,
                    CheckOut = now,
                    CheckOutRouteId = body.CheckOutRouteDirectionId,
                    CheckOutLatitude = body.CheckOutLatitude,
                    CheckOutLongitude = body.CheckOutLongtitud,
                    CreationBy = userId,
                });
            }
            await _db.SaveChangesAsync();
        }

        if (type == "Bus") await RecordBusRound(serial, isCheckIn, now);

        return Envelope.SuccessWrite();
    }

    /// <summary>Record one bus leg for a serial's active route (round row + monthly account upsert).</summary>
    private async Task RecordBusRound(string serial, bool isCheckIn, DateTime when)
    {
        var route = await _db.TransportationVehicleRoutes
            .Where(r => r.Serial == serial && r.Active)
            .Select(r => new { r.Id, r.SupplierId, r.LineCost, r.OneWay })
            .FirstOrDefaultAsync();
        if (route == null) return;
        var oneWay = route.OneWay ?? false;
        var price = oneWay ? route.LineCost : route.LineCost / 2;

        _db.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
        {
            RouteId = route.Id,
            SupplierId = route.SupplierId,
            Serial = serial,
            CheckInDate = !oneWay && isCheckIn ? when : null,
            CheckOutDate = !oneWay && !isCheckIn ? when : null,
            OneWayDate = oneWay ? when : null,
            PriceRound = price,
        });

        var monthStart = new DateTime(when.Year, when.Month, 1);
        var monthEnd = monthStart.AddMonths(1);
        var account = await _db.TransportationVehicleRouteAccounts
            .FirstOrDefaultAsync(a => a.RouteId == route.Id && a.DateOfMonth >= monthStart && a.DateOfMonth < monthEnd);
        if (account != null)
        {
            account.CountOfRounds += 1;
            account.PriceOfRounds = (account.PriceOfRounds ?? 0) + price;
        }
        else
        {
            _db.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
            {
                RouteId = route.Id,
                SupplierId = route.SupplierId,
                Serial = serial,
                DateOfMonth = monthStart,
                CountOfRounds = 1,
                PriceOfRounds = price,
                DeductPerRounds = 0,
            });
        }
        await _db.SaveChangesAsync();
    }

    /// <summary>POST AddUsersAttedance44 — fingerprint-device sync ack.</summary>
    public Task<object> AddUsersAttendance44() => Task.FromResult<object>(Envelope.Success("done"));

    private class AttendanceEntry
    {
        public string Date { get; set; } = string.Empty;
        public string CheckIn { get; set; } = string.Empty;
        public string CheckOut { get; set; } = string.Empty;
        public bool ISAttendace { get; set; }
    }
}
