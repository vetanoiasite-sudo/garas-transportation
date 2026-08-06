using System.Globalization;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Stations;

// Stations (a.k.a. Directions) — §6.5. Same base path & conventions as routes.
public class StationInput
{
    public string? RouteDirection { get; set; }
    public string? Description { get; set; }
    public object? Latitude { get; set; }
    public object? Longtitud { get; set; } // frozen typo key
    public bool? Active { get; set; }
    public int? Id { get; set; }
    public int? TransportationVehicleRouteId { get; set; }
}

public class StationsService
{
    private readonly AppDbContext _db;

    public StationsService(AppDbContext db) => _db = db;

    private static double? Num(object? v)
    {
        if (v is null) return null;
        var s = v.ToString();
        if (string.IsNullOrEmpty(s)) return null;
        return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var n) ? n : null;
    }

    /// <summary>GET GetTransportationDirection — stations for a route (TransportationDirectionsData).</summary>
    public async Task<object> List(int routeId)
    {
        if (routeId == 0) return Envelope.Fail("Err101", "routeId is required");
        var rows = await _db.TransportationVehicleRouteDirections
            .Where(d => d.RouteId == routeId)
            .OrderBy(d => d.Id)
            .ToListAsync();

        var data = rows.Select(d => new
        {
            Id = d.Id.ToString(),
            RouteDirection = d.RouteDirection,
            Description = d.Description ?? string.Empty,
            Latitude = d.Latitude != null ? d.Latitude.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
            Longtitud = d.Longitude != null ? d.Longitude.Value.ToString(CultureInfo.InvariantCulture) : string.Empty, // frozen typo key
            Active = d.Active,
        });
        return Envelope.Success(data);
    }

    /// <summary>POST AddTransportationDirection — bulk insert stations under a route.</summary>
    public async Task<object> Add(int routeId, List<StationInput> items)
    {
        if (routeId == 0) return Envelope.Fail("Err101", "TransportationVehicleRouteId is required");
        if (items.Count == 0) return Envelope.Fail("Err101", "Data is required");
        _db.TransportationVehicleRouteDirections.AddRange(items.Select(i => new TransportationVehicleRouteDirection
        {
            RouteId = routeId,
            RouteDirection = i.RouteDirection ?? string.Empty,
            Description = i.Description,
            Latitude = Num(i.Latitude),
            Longitude = Num(i.Longtitud),
            Active = i.Active ?? true,
        }));
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(routeId);
    }

    /// <summary>POST UpdateTransportationDirection — edit a station.</summary>
    public async Task<object> Update(int id, StationInput body)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var d = await _db.TransportationVehicleRouteDirections.FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return Envelope.Fail("Err142", "Station not found");
        d.RouteDirection = body.RouteDirection ?? string.Empty;
        d.Description = body.Description;
        d.Latitude = Num(body.Latitude);
        d.Longitude = Num(body.Longtitud);
        d.Active = body.Active ?? true;
        if (body.TransportationVehicleRouteId.HasValue) d.RouteId = body.TransportationVehicleRouteId.Value;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST DeleteTransportationDirection — delete a station. Blocks when a
    /// passenger is still assigned to it (FK guard).</summary>
    public async Task<object> Remove(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var assignedCount = await _db.TransportationVehicleRouteEmployees.CountAsync(e => e.DirectionId == id);
        if (assignedCount > 0)
            return Envelope.Fail("Err102", "لا يمكن حذف المحطة لارتباطها بركاب. انقل الركاب إلى محطة أخرى أولاً.");
        var d = await _db.TransportationVehicleRouteDirections.FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return Envelope.Fail("Err142", "Station not found");
        _db.TransportationVehicleRouteDirections.Remove(d);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }
}
