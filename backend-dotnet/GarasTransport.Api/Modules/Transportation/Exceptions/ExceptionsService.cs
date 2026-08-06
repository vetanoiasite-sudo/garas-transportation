using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;
using UserEntity = GarasTransport.Domain.Entities.User;

namespace GarasTransport.Api.Modules.Transportation.Exceptions;

// Shape of the Add/Update exception payload (documented keys, §6.8).
public class ExceptionBody
{
    public int? id { get; set; }
    public int? transportationVehicleRouteId { get; set; }
    public int? hrUserId { get; set; }
    public bool? Active { get; set; }
    public string? reasonException { get; set; }
    public string? contactNumber { get; set; }
    public string? Period { get; set; } // Go | Return | Both
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? DayName { get; set; }
    public string? ExceptionDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longtitud { get; set; } // frozen typo key
    public int? TransportationVehicleRouteDirectionId { get; set; }
}

public class ExceptionsService
{
    private readonly AppDbContext _db;

    public ExceptionsService(AppDbContext db) => _db = db;

    private static string FullName(HrUser? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    private static string FullName(UserEntity? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    /// <summary>GET GetEmployeeException — paginated exceptions (TransportationExceptionData rows).</summary>
    public async Task<object> GetAll(int pageNo, int noOfItems, int? hrUserId, int? id)
    {
        var query = _db.TransportationVehicleRouteEmployeeExceptions.Where(e => e.Active);
        if (hrUserId.HasValue) query = query.Where(e => e.HrUserId == hrUserId.Value);
        if (id.HasValue) query = query.Where(e => e.Id == id.Value);

        var total = await query.CountAsync();
        var rows = await query
            .Include(e => e.HrUser)
            .Include(e => e.Route).ThenInclude(r => r!.Line)
            .OrderByDescending(e => e.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        // Resolve creator (User) names and direction names — neither has a direct
        // relation on the exception model, so batch-fetch both by id.
        var creatorIds = rows.Where(e => e.CreationBy.HasValue).Select(e => e.CreationBy!.Value).Distinct().ToList();
        var creatorMap = new Dictionary<int, string>();
        if (creatorIds.Count > 0)
        {
            var users = await _db.Users.Where(u => creatorIds.Contains(u.Id)).ToListAsync();
            foreach (var u in users)
                creatorMap[u.Id] = string.IsNullOrEmpty(FullName(u)) ? u.Email : FullName(u);
        }
        var directionIds = rows.Where(e => e.DirectionId.HasValue).Select(e => e.DirectionId!.Value).Distinct().ToList();
        var directionMap = new Dictionary<int, string>();
        if (directionIds.Count > 0)
        {
            var dirs = await _db.TransportationVehicleRouteDirections.Where(d => directionIds.Contains(d.Id)).ToListAsync();
            foreach (var d in dirs) directionMap[d.Id] = d.RouteDirection;
        }

        var data = rows.Select(e => new
        {
            Id = e.Id.ToString(),
            HrUserId = e.HrUserId.ToString(),
            TransportationVehicleRouteId = e.RouteId,
            TransportationLineName = e.Route?.Line?.LineName ?? string.Empty,
            Active = e.Active,
            CreationDate = Fmt.Iso(e.CreationDate),
            CreationBy = e.CreationBy != null ? e.CreationBy.Value.ToString() : string.Empty,
            CreationName = e.CreationBy != null ? creatorMap.GetValueOrDefault(e.CreationBy.Value, string.Empty) : string.Empty,
            TransportationVehicleRouteDirectionId = e.DirectionId != null ? e.DirectionId.Value.ToString() : string.Empty,
            TransportationVehicleRouteDirectionName = e.DirectionId != null ? directionMap.GetValueOrDefault(e.DirectionId.Value, string.Empty) : string.Empty,
            FromDate = Fmt.Iso(e.FromDate),
            ToDate = Fmt.Iso(e.ToDate),
            Period = e.Period ?? string.Empty,
            DayName = e.DayName ?? string.Empty,
            LatitudeExceptional = 0,
            LongtitudExceptional = 0, // frozen typo key
            ExceptionDate = Fmt.Iso(e.ExceptionDate),
            ExceptionDatePeriod = string.Empty,
            Latitude = e.Latitude ?? 0,
            Longtitud = e.Longitude ?? 0, // frozen typo key
            ExceptionDirectionId = string.Empty,
            ExceptionDirectionName = string.Empty,
            HrUserName = FullName(e.HrUser),
            ReasonException = e.ReasonException ?? string.Empty,
            ContactNumber = e.ContactNumber ?? string.Empty,
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>POST AddException — create an exception. creationBy = caller.</summary>
    public async Task<object> Add(ExceptionBody body, int userId)
    {
        var routeId = body.transportationVehicleRouteId ?? 0;
        var hrUserId = body.hrUserId ?? 0;
        if (routeId == 0) return Envelope.Fail("Err101", "transportationVehicleRouteId is required");
        if (hrUserId == 0) return Envelope.Fail("Err101", "hrUserId is required");

        var entity = new TransportationVehicleRouteEmployeeException();
        Apply(entity, body, hrUserId, routeId, userId);
        _db.TransportationVehicleRouteEmployeeExceptions.Add(entity);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(entity.Id);
    }

    /// <summary>POST UpdateException — update an exception.</summary>
    public async Task<object> Update(int id, ExceptionBody body, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "id is required");
        var entity = await _db.TransportationVehicleRouteEmployeeExceptions.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null) return Envelope.Fail("Err404", "Exception not found");
        var routeId = body.transportationVehicleRouteId ?? 0;
        var hrUserId = body.hrUserId ?? 0;
        Apply(entity, body, hrUserId, routeId, userId);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>GET GetCapacityNumbers — capacity breakdown for a route on a given day.</summary>
    public async Task<object> GetCapacity(int routeId, string? period, string? dateSearch)
    {
        if (routeId == 0) return Envelope.Fail("Err101", "RouteID is required");
        var route = await _db.TransportationVehicleRoutes.Include(r => r.Vehicle).FirstOrDefaultAsync(r => r.Id == routeId);
        if (route == null) return Envelope.Fail("Err404", "Route not found");

        var target = string.IsNullOrEmpty(dateSearch) ? DateTime.UtcNow
            : (DateTime.TryParse(dateSearch, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : DateTime.UtcNow);
        var dayStart = new DateTime(target.Year, target.Month, target.Day, 0, 0, 0, DateTimeKind.Utc);
        var dayEnd = new DateTime(target.Year, target.Month, target.Day, 23, 59, 59, DateTimeKind.Utc).AddMilliseconds(999);
        string[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        var dayName = dayNames[(int)target.DayOfWeek];
        var periodValues = period != null ? new[] { period, "Both" } : new[] { "Both" };

        var members = await _db.TransportationVehicleRouteEmployees
            .Where(m => m.RouteId == routeId && m.Active && periodValues.Contains(m.Period)
                        && (m.FromDate == null || m.FromDate <= dayEnd)
                        && (m.ToDate == null || m.ToDate >= dayStart))
            .Select(m => m.HrUserId)
            .ToListAsync();

        List<(int HrUserId, int RouteId)> exceptions = new();
        if (period != null)
        {
            var ex = await _db.TransportationVehicleRouteEmployeeExceptions
                .Where(e => e.Active &&
                    ((e.Period == period && e.DayName == dayName
                        && (e.FromDate == null || e.FromDate <= dayEnd)
                        && (e.ToDate == null || e.ToDate >= dayStart))
                     || (e.Period == period && e.ExceptionDate >= dayStart && e.ExceptionDate <= dayEnd)))
                .Select(e => new { e.HrUserId, e.RouteId })
                .ToListAsync();
            exceptions = ex.Select(e => (e.HrUserId, e.RouteId)).ToList();
        }

        var memberIds = members.ToHashSet();
        var exceptedUserIds = exceptions.Select(e => e.HrUserId).ToHashSet();
        var routeEmployeesToOtherLines = memberIds.Count(id => exceptedUserIds.Contains(id));
        var expectionNumFromOtherLines = exceptions.Count(e => e.RouteId == routeId);
        var capacityWithoutException = members.Count;

        var data = new
        {
            FullCapacity = route.Vehicle?.Capacity ?? 0,
            ActualCapacity = capacityWithoutException + expectionNumFromOtherLines - routeEmployeesToOtherLines,
            CapacityWithoutExpection = capacityWithoutException, // frozen typo key
            ExpectionNumFromOtherLines = expectionNumFromOtherLines, // frozen typo key
            RouteEmployeesToOtherLines = routeEmployeesToOtherLines,
        };
        return Envelope.Success(data);
    }

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : null);

    /// <summary>Maps the documented payload to the exception model columns.</summary>
    private static void Apply(TransportationVehicleRouteEmployeeException e, ExceptionBody body, int hrUserId, int routeId, int userId)
    {
        e.HrUserId = hrUserId;
        e.RouteId = routeId;
        e.Period = body.Period ?? string.Empty;
        e.FromDate = ParseDate(body.FromDate);
        e.ToDate = ParseDate(body.ToDate);
        e.DayName = body.DayName;
        e.ExceptionDate = ParseDate(body.ExceptionDate);
        e.Latitude = body.Latitude;
        e.Longitude = body.Longtitud;
        e.DirectionId = body.TransportationVehicleRouteDirectionId;
        e.ReasonException = body.reasonException;
        e.ContactNumber = body.contactNumber;
        e.Active = body.Active ?? true;
        e.CreationBy = userId;
    }
}
