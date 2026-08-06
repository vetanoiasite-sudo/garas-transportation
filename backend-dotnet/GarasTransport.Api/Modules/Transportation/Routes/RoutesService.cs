using ClosedXML.Excel;
using GarasTransport.Api.Common;
using GarasTransport.Api.Modules.Transportation.Notifications;
using Microsoft.EntityFrameworkCore;
using UserEntity = GarasTransport.Domain.Entities.User;

namespace GarasTransport.Api.Modules.Transportation.Routes;

// Shape of the Add/Update route payload (documented PascalCase keys, §6.4).
public class RouteBody
{
    public int? Id { get; set; }
    public int? SupplierId { get; set; }
    public int? SupplierContactPersonId { get; set; }
    public int? BranchScheduleId { get; set; }
    public int? TransportationLineId { get; set; }
    public int? HrUserId { get; set; } // → supervisorId
    public string? PeriodFrom { get; set; } // ISO-UTC
    public bool? Active { get; set; }
    public string? FromDate { get; set; } // go time of day
    public string? ToDate { get; set; } // return time of day
    public string? NameOfRoute { get; set; }
    public double? LineCost { get; set; }
    public bool? OneWay { get; set; }
    public int? TransportationVehicleId { get; set; }
}

public class RouteFilters
{
    public int? LineId { get; set; }
    public int? SupplierId { get; set; }
    public string? SerialBus { get; set; }
    public string? Name { get; set; }
}

public class RoutesService
{
    private readonly AppDbContext _db;
    private readonly NotificationsService _notifications;

    public RoutesService(AppDbContext db, NotificationsService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    private static string FullName(UserEntity? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    private static string FullName(HrUser? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    private IQueryable<TransportationVehicleRoute> WithIncludes(IQueryable<TransportationVehicleRoute> q) =>
        q.Include(r => r.Line)
         .Include(r => r.Vehicle).ThenInclude(v => v!.VehicleType)
         .Include(r => r.Supplier)
         .Include(r => r.ContactPerson)
         .Include(r => r.BranchSchedule)
         .Include(r => r.Supervisor);

    /// <summary>Tally each route's active passengers by their "other identifier" code (C / M).</summary>
    private async Task<Dictionary<int, (int C, int M)>> ReligionCounts(List<int> routeIds)
    {
        var map = new Dictionary<int, (int C, int M)>();
        if (routeIds.Count == 0) return map;
        var emps = await _db.TransportationVehicleRouteEmployees
            .Where(e => routeIds.Contains(e.RouteId) && e.Active)
            .Select(e => new { e.RouteId, Name = e.HrUser!.MaritalStatus != null ? e.HrUser.MaritalStatus.Name : null })
            .ToListAsync();
        foreach (var e in emps)
        {
            map.TryGetValue(e.RouteId, out var t);
            if (e.Name == "C") t.C++;
            else if (e.Name == "M") t.M++;
            map[e.RouteId] = t;
        }
        return map;
    }

    /// <summary>Count today's attended passengers per route (distinct fingerprints with a
    /// Person check-in today whose checkInRouteId is the route).</summary>
    private async Task<Dictionary<int, int>> AttendedTodayByRoute(List<int> routeIds)
    {
        var map = new Dictionary<int, int>();
        if (routeIds.Count == 0) return map;
        var gte = DateTime.Today;
        var lt = gte.AddDays(1);
        var rows = await _db.UserAttendances
            .Where(a => a.Type == "Person" && a.CheckInRouteId != null
                        && routeIds.Contains(a.CheckInRouteId.Value)
                        && a.CheckIn >= gte && a.CheckIn < lt)
            .Select(a => new { a.Serial, a.CheckInRouteId })
            .ToListAsync();
        var perRoute = new Dictionary<int, HashSet<string>>();
        foreach (var a in rows)
        {
            if (a.CheckInRouteId == null) continue;
            if (!perRoute.TryGetValue(a.CheckInRouteId.Value, out var set))
                perRoute[a.CheckInRouteId.Value] = set = new HashSet<string>();
            set.Add(a.Serial);
        }
        foreach (var kv in perRoute) map[kv.Key] = kv.Value.Count;
        return map;
    }

    private async Task<Dictionary<int, int>> DirectionCounts(List<int> routeIds)
    {
        if (routeIds.Count == 0) return new();
        return await _db.TransportationVehicleRouteDirections
            .Where(d => routeIds.Contains(d.RouteId))
            .GroupBy(d => d.RouteId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);
    }

    private async Task<Dictionary<int, int>> ActiveEmployeeCounts(List<int> routeIds)
    {
        if (routeIds.Count == 0) return new();
        return await _db.TransportationVehicleRouteEmployees
            .Where(e => routeIds.Contains(e.RouteId) && e.Active)
            .GroupBy(e => e.RouteId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);
    }

    /// <summary>GET getAllTransportationRoute — paginated routes (TransportationRouteData rows).</summary>
    public async Task<object> GetAll(int pageNo, int noOfItems, RouteFilters filters)
    {
        var query = _db.TransportationVehicleRoutes.Where(r => r.Active);
        if (filters.LineId.HasValue) query = query.Where(r => r.TransportationLineId == filters.LineId.Value);
        if (filters.SupplierId.HasValue) query = query.Where(r => r.SupplierId == filters.SupplierId.Value);
        if (!string.IsNullOrEmpty(filters.SerialBus)) query = query.Where(r => r.Serial == filters.SerialBus);
        if (!string.IsNullOrEmpty(filters.Name)) query = query.Where(r => r.NameOfRoute.Contains(filters.Name));

        var total = await query.CountAsync();
        var rows = await WithIncludes(query)
            .OrderByDescending(r => r.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var routeIds = rows.Select(r => r.Id).ToList();
        var religion = await ReligionCounts(routeIds);
        var attended = await AttendedTodayByRoute(routeIds);
        var dirCounts = await DirectionCounts(routeIds);
        var empCounts = await ActiveEmployeeCounts(routeIds);

        var data = rows.Select(r =>
        {
            var userInRoute = empCounts.GetValueOrDefault(r.Id);
            var rc = religion.GetValueOrDefault(r.Id);
            return new
            {
                Id = r.Id,
                LineName = r.Line?.LineName ?? string.Empty,
                LineId = r.TransportationLineId,
                LineCost = r.LineCost,
                SupplierName = r.Supplier?.Name ?? string.Empty,
                SupplierId = r.SupplierId ?? 0,
                SupplierContactPersonName = r.ContactPerson?.Name ?? string.Empty,
                SupplierContactPersonId = r.SupplierContactPersonId ?? 0,
                Serial = r.Serial,
                branchScheduleId = r.BranchScheduleId ?? 0,
                PeriodFrom = Fmt.Iso(r.PeriodFrom),
                PeriodTo = Fmt.IsoOrNull(r.PeriodTo),
                BusSupervisor = FullName(r.Supervisor),
                FullCapacity = r.Vehicle?.Capacity ?? 0,
                UserInRoute = userInRoute,
                ActualCapacity = userInRoute,
                ActualUserAttedance = attended.GetValueOrDefault(r.Id), // frozen typo key
                ExpectionNumFromOtherLines = 0, // frozen typo key
                RouteEmployeesToOtherLines = 0,
                DirectionNum = dirCounts.GetValueOrDefault(r.Id),
                MuslimNum = rc.M,
                christianNum = rc.C, // lowercase key
                FromoDate = r.FromTime ?? string.Empty, // frozen typo key
                ToDate = r.ToTime ?? string.Empty,
                NameOfRoute = r.NameOfRoute,
                TransportationVehicleId = r.TransportationVehicleId,
                TransportationVehicleName = r.Vehicle?.VehicleType?.Type ?? string.Empty,
            };
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET getTransportationRoute — single route detail (all strings).</summary>
    public async Task<object> GetOne(int routeId)
    {
        if (routeId == 0) return Envelope.Fail("Err101", "RouteId is required");
        var r = await WithIncludes(_db.TransportationVehicleRoutes).FirstOrDefaultAsync(x => x.Id == routeId);
        if (r == null) return Envelope.Fail("Err404", "Route not found");

        var rc = (await ReligionCounts(new List<int> { r.Id })).GetValueOrDefault(r.Id);
        string S(object? v) => v?.ToString() ?? string.Empty;
        var data = new
        {
            Id = S(r.Id),
            LineId = S(r.TransportationLineId),
            LineName = S(r.Line?.LineName),
            SupplierId = S(r.SupplierId),
            SupplierName = S(r.Supplier?.Name),
            SupplierContactPersonId = S(r.SupplierContactPersonId),
            SupplierContactPersonName = S(r.ContactPerson?.Name),
            Serial = S(r.Serial),
            branchScheduleId = S(r.BranchScheduleId),
            branchSchedule = r.BranchSchedule != null ? $"{r.BranchSchedule.From} - {r.BranchSchedule.To}" : string.Empty,
            PeriodFrom = Fmt.Iso(r.PeriodFrom),
            PeriodTo = Fmt.Iso(r.PeriodTo),
            BuSupervisorId = S(r.SupervisorId), // frozen typo key
            BusSupervisor = FullName(r.Supervisor),
            FromoDate = S(r.FromTime), // frozen typo key
            ToDate = S(r.ToTime),
            christianNum = S(rc.C), // lowercase key
            MuslimNum = S(rc.M),
            NameOfRoute = S(r.NameOfRoute),
            LineCost = S(r.LineCost),
            TransportationVehicleId = S(r.TransportationVehicleId),
            TransportationVehicleName = S(r.Vehicle?.VehicleType?.Type),
        };
        return Envelope.Success(data);
    }

    /// <summary>GET getTransportationRouteByHrUser — routes supervised by an HR user.</summary>
    public async Task<object> GetByHrUser(int hrUserId)
    {
        if (hrUserId == 0) return Envelope.Fail("Err101", "HrUserId is required");
        var rows = await WithIncludes(_db.TransportationVehicleRoutes.Where(r => r.SupervisorId == hrUserId && r.Active))
            .OrderByDescending(r => r.Id)
            .ToListAsync();

        var routeIds = rows.Select(r => r.Id).ToList();
        var attended = await AttendedTodayByRoute(routeIds);
        var dirCounts = await DirectionCounts(routeIds);
        var empCounts = await ActiveEmployeeCounts(routeIds);

        var data = rows.Select(r =>
        {
            var userInRoute = empCounts.GetValueOrDefault(r.Id);
            return new
            {
                Id = r.Id,
                LineName = r.Line?.LineName ?? string.Empty,
                LineId = r.TransportationLineId,
                SupplierName = r.Supplier?.Name ?? string.Empty,
                SupplierId = r.SupplierId ?? 0,
                SupplierContactPersonName = r.ContactPerson?.Name ?? string.Empty,
                SupplierContactPersonId = r.SupplierContactPersonId ?? 0,
                Serial = r.Serial,
                branchScheduleId = r.BranchScheduleId ?? 0,
                PeriodFrom = Fmt.Iso(r.PeriodFrom),
                PeriodTo = Fmt.IsoOrNull(r.PeriodTo),
                BusSupervisor = FullName(r.Supervisor),
                FullCapacity = r.Vehicle?.Capacity ?? 0,
                UserInRoute = userInRoute,
                ActualCapacity = userInRoute,
                ActualUserAttedance = attended.GetValueOrDefault(r.Id), // frozen typo key
                ExpectionNumFromOtherLines = 0, // frozen typo key
                RouteEmployeesToOtherLines = 0,
                DirectionNum = dirCounts.GetValueOrDefault(r.Id),
            };
        });
        return Envelope.Success(data);
    }

    /// <summary>GET RoutesWithUsersExcel — every active route with its passengers as a base64 (.xlsx) file.</summary>
    public async Task<object> RoutesWithUsersExcel()
    {
        var routes = await _db.TransportationVehicleRoutes
            .Where(r => r.Active)
            .Include(r => r.Line)
            .Include(r => r.Supplier)
            .Include(r => r.Employees.Where(e => e.Active)).ThenInclude(e => e.HrUser)
            .Include(r => r.Employees.Where(e => e.Active)).ThenInclude(e => e.Direction)
            .OrderByDescending(r => r.Id)
            .ToListAsync();

        var periodLabel = new Dictionary<string, string> { ["Go"] = "ذهاب", ["Return"] = "عودة", ["Both"] = "ذهاب وعودة" };
        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("الخطوط مع الموظفين");
        sheet.RightToLeft = true;
        string[] headers = { "الرقم المسلسل", "خط السير", "محطة التجمع", "المورد", "الراكب", "رقم هوية الراكب", "الجوال", "المحطة", "الاتجاه" };
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var r in routes)
        {
            var serial = r.Serial;
            var routeName = r.NameOfRoute;
            var line = r.Line?.LineName ?? string.Empty;
            var supplier = r.Supplier?.Name ?? string.Empty;
            if (r.Employees.Count == 0)
            {
                sheet.Cell(row, 1).Value = serial;
                sheet.Cell(row, 2).Value = routeName;
                sheet.Cell(row, 3).Value = line;
                sheet.Cell(row, 4).Value = supplier;
                row++;
                continue;
            }
            foreach (var e in r.Employees)
            {
                sheet.Cell(row, 1).Value = serial;
                sheet.Cell(row, 2).Value = routeName;
                sheet.Cell(row, 3).Value = line;
                sheet.Cell(row, 4).Value = supplier;
                sheet.Cell(row, 5).Value = FullName(e.HrUser);
                sheet.Cell(row, 6).Value = e.HrUser?.Email ?? string.Empty;
                sheet.Cell(row, 7).Value = e.HrUser?.Mobile ?? string.Empty;
                sheet.Cell(row, 8).Value = e.Direction?.RouteDirection ?? string.Empty;
                sheet.Cell(row, 9).Value = periodLabel.GetValueOrDefault(e.Period, e.Period);
                row++;
            }
        }

        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>Resolve the route's gathering-station (line) id: use the picked one, or
    /// auto-create a new station named after the route when none was chosen.</summary>
    private async Task<int> ResolveLineId(RouteBody body, int userId)
    {
        var id = body.TransportationLineId ?? 0;
        if (id > 0) return id;
        var line = new TransportationLine
        {
            LineName = string.IsNullOrWhiteSpace(body.NameOfRoute) ? "خط" : body.NameOfRoute!.Trim(),
            IsApproved = false,
            CreationBy = userId,
            ModifiedBy = userId,
        };
        _db.TransportationLines.Add(line);
        await _db.SaveChangesAsync();
        return line.Id;
    }

    /// <summary>POST AddTransportationRoute — create a route (auto serial, seed 70000000).</summary>
    public async Task<object> Add(RouteBody body, int userId)
    {
        if (string.IsNullOrWhiteSpace(body.NameOfRoute)) return Envelope.Fail("Err101", "NameOfRoute is required");
        var serial = await NextSerial();
        var transportationLineId = await ResolveLineId(body, userId);
        var created = new TransportationVehicleRoute
        {
            TransportationLineId = transportationLineId,
            TransportationVehicleId = body.TransportationVehicleId ?? 0,
            SupplierId = body.SupplierId,
            SupplierContactPersonId = body.SupplierContactPersonId,
            BranchScheduleId = body.BranchScheduleId,
            SupervisorId = body.HrUserId,
            Serial = serial,
            PeriodFrom = ParseDate(body.PeriodFrom),
            FromTime = body.FromDate,
            ToTime = body.ToDate,
            NameOfRoute = body.NameOfRoute!,
            LineCost = body.LineCost ?? 0,
            OneWay = body.OneWay ?? false,
            Active = body.Active ?? true,
            CreationBy = userId,
            ModifiedBy = userId,
        };
        _db.TransportationVehicleRoutes.Add(created);
        await _db.SaveChangesAsync();
        await _notifications.NotifySuperAdmins(userId, new NotifyOptions
        {
            Title = "إضافة خط جديد",
            Description = "بحاجة لاعتماد",
            EntityType = "route",
            EntityId = created.Id,
        });
        return Envelope.SuccessWrite(created.Id);
    }

    /// <summary>POST UpdateTransportationRoute — update a route (serial preserved).</summary>
    public async Task<object> Update(int id, RouteBody body, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var r = await _db.TransportationVehicleRoutes.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return Envelope.Fail("Err404", "Route not found");
        var transportationLineId = await ResolveLineId(body, userId);
        r.TransportationLineId = transportationLineId;
        r.TransportationVehicleId = body.TransportationVehicleId ?? 0;
        r.SupplierId = body.SupplierId;
        r.SupplierContactPersonId = body.SupplierContactPersonId;
        r.BranchScheduleId = body.BranchScheduleId;
        r.SupervisorId = body.HrUserId;
        r.PeriodFrom = ParseDate(body.PeriodFrom);
        r.FromTime = body.FromDate;
        r.ToTime = body.ToDate;
        r.NameOfRoute = body.NameOfRoute ?? string.Empty;
        r.LineCost = body.LineCost ?? 0;
        r.OneWay = body.OneWay ?? false;
        r.Active = body.Active ?? true;
        r.ModifiedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST DeleteTransportationRoute — block if the route has employees, else delete directions + route.</summary>
    public async Task<object> Remove(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var employees = await _db.TransportationVehicleRouteEmployees.CountAsync(e => e.RouteId == id);
        if (employees > 0) return Envelope.Fail("Err102", "لا يمكن حذف المسار لوجود موظفين مرتبطين به");
        var route = await _db.TransportationVehicleRoutes.FirstOrDefaultAsync(x => x.Id == id);
        if (route == null) return Envelope.Fail("Err404", "Route not found");
        var directions = await _db.TransportationVehicleRouteDirections.Where(d => d.RouteId == id).ToListAsync();
        _db.TransportationVehicleRouteDirections.RemoveRange(directions);
        _db.TransportationVehicleRoutes.Remove(route);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST ApproveRoute — toggles `active` (per docs ApproveRoute sets Active).</summary>
    public async Task<object> Approve(int id, bool approve, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var r = await _db.TransportationVehicleRoutes.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return Envelope.Fail("Err404", "Route not found");
        r.Active = approve;
        r.ModifiedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>Next serial = (max numeric serial) + 1, seeded at 70000000 when none exist.</summary>
    private async Task<string> NextSerial()
    {
        var serials = await _db.TransportationVehicleRoutes.Select(r => r.Serial).ToListAsync();
        long max = 0;
        foreach (var s in serials)
            if (long.TryParse(s, out var n) && n > max) max = n;
        return (max > 0 ? max + 1 : 70000000).ToString();
    }

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : null);
}
