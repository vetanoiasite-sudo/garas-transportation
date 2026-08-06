using GarasTransport.Api.Common;
using GarasTransport.Api.Modules.Transportation.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Vehicles;

public class VehiclesService
{
    private readonly AppDbContext _db;
    private readonly NotificationsService _notifications;

    public VehiclesService(AppDbContext db, NotificationsService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    /// <summary>GET getAllVehicleType — vehicle-type lookup.</summary>
    public async Task<object> GetAllVehicleType()
    {
        var rows = await _db.VehicleTypes.OrderByDescending(t => t.Id).ToListAsync();
        var data = rows.Select(t => new
        {
            Id = t.Id,
            Type = t.Type,
            Active = t.Active,
            TransportationVehicles = Array.Empty<object>(),
        });
        return Envelope.Success(data);
    }

    /// <summary>POST AddVehicleType — create a vehicle type.</summary>
    public async Task<object> AddVehicleType(string? type, bool? active)
    {
        if (string.IsNullOrWhiteSpace(type)) return Envelope.Fail("Err101", "Type is required");
        var created = new VehicleType { Type = type, Active = active ?? true };
        _db.VehicleTypes.Add(created);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(created.Id);
    }

    /// <summary>POST UpdateVehicleType — rename / toggle a vehicle type.</summary>
    public async Task<object> UpdateVehicleType(int id, string? type, bool? active)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        if (string.IsNullOrWhiteSpace(type)) return Envelope.Fail("Err101", "Type is required");
        var vt = await _db.VehicleTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (vt == null) return Envelope.Fail("Err142", "Vehicle type not found");
        vt.Type = type;
        vt.Active = active ?? true;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>GET getAllTransportationVehicle — paginated vehicles with joined type name.</summary>
    public async Task<object> GetAllVehicles(int pageNo = 1, int noOfItems = 20)
    {
        var query = _db.TransportationVehicles.Where(v => v.Active);
        var total = await query.CountAsync();
        var rows = await query
            .Include(v => v.VehicleType)
            .OrderByDescending(v => v.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var data = rows.Select(v => new
        {
            Id = v.Id,
            VehicleTypeId = v.VehicleTypeId,
            Capacity = v.Capacity,
            IsApproved = v.IsApproved,
            ApprovedBy = v.ApprovedBy ?? string.Empty,
            Active = v.Active,
            CreationDate = v.CreationDate,
            VehicleTypeName = v.VehicleType?.Type ?? string.Empty,
            TransportationLines = Array.Empty<object>(),
        });
        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>POST AddTransportationVehicle — create (IsApproved defaults false).</summary>
    public async Task<object> AddVehicle(int vehicleTypeId, int capacity, bool? active, int userId)
    {
        if (vehicleTypeId == 0) return Envelope.Fail("Err101", "VehicleTypeId is required");
        var created = new TransportationVehicle
        {
            VehicleTypeId = vehicleTypeId,
            Capacity = capacity,
            Active = active ?? true,
            IsApproved = false,
            CreationBy = userId,
            ModifiedBy = userId,
        };
        _db.TransportationVehicles.Add(created);
        await _db.SaveChangesAsync();
        await _notifications.NotifySuperAdmins(userId, new NotifyOptions
        {
            Title = "إضافة مركبة جديدة",
            Description = "بحاجة لاعتماد",
            EntityType = "vehicle",
            EntityId = created.Id,
        });
        return Envelope.SuccessWrite(created.Id);
    }

    /// <summary>POST UpdateTransportationVehicle — update type/capacity/active.</summary>
    public async Task<object> UpdateVehicle(int id, int vehicleTypeId, int capacity, bool? active, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        if (vehicleTypeId == 0) return Envelope.Fail("Err101", "VehicleTypeId is required");
        var v = await _db.TransportationVehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return Envelope.Fail("Err142", "Vehicle not found");
        v.VehicleTypeId = vehicleTypeId;
        v.Capacity = capacity;
        v.Active = active ?? true;
        v.ModifiedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST DeleteTransportationVehicle — delete (Id header).</summary>
    public async Task<object> RemoveVehicle(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var v = await _db.TransportationVehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return Envelope.Fail("Err142", "Vehicle not found");
        _db.TransportationVehicles.Remove(v);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST ApproveTransportationVehicle — approve/unapprove (Id + Approve headers).</summary>
    public async Task<object> ApproveVehicle(int id, bool approve, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var v = await _db.TransportationVehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return Envelope.Fail("Err142", "Vehicle not found");
        v.IsApproved = approve;
        v.ApprovedBy = userId.ToString();
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }
}
