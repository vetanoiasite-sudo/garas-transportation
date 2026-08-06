using GarasTransport.Api.Common;
using GarasTransport.Api.Modules.Transportation.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Lines;

public class LinesService
{
    private readonly AppDbContext _db;
    private readonly NotificationsService _notifications;

    public LinesService(AppDbContext db, NotificationsService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    /// <summary>GET getAllTransportationLine — paginated lines ({ Id, Name, RouteNum }); optional name search.</summary>
    public async Task<object> GetAll(int pageNo = 1, int noOfItems = 20, string? name = null)
    {
        var query = _db.TransportationLines.Where(l => l.Active);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(l => l.LineName.Contains(name));

        var total = await query.CountAsync();
        var rows = await query
            .OrderByDescending(l => l.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .Select(l => new
            {
                Id = l.Id,
                Name = l.LineName,
                RouteNum = l.Routes.Count(r => r.Active), // count of active child routes
                IsApproved = l.IsApproved ?? false,
            })
            .ToListAsync();

        return Envelope.SuccessList(rows, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>POST AddTransportationLine — create a line.</summary>
    public async Task<object> Add(string? lineName, int userId)
    {
        if (string.IsNullOrWhiteSpace(lineName)) return Envelope.Fail("Err101", "LineName is required");
        var created = new TransportationLine { LineName = lineName, CreationBy = userId, ModifiedBy = userId };
        _db.TransportationLines.Add(created);
        await _db.SaveChangesAsync();
        await _notifications.NotifySuperAdmins(userId, new NotifyOptions
        {
            Title = "إضافة محطة تجمع جديدة",
            Description = "بحاجة لاعتماد",
            EntityType = "line",
            EntityId = created.Id,
        });
        return Envelope.SuccessWrite(created.Id);
    }

    /// <summary>POST UpdateTransportationLine — rename a line.</summary>
    public async Task<object> Update(int id, string? lineName, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        if (string.IsNullOrWhiteSpace(lineName)) return Envelope.Fail("Err101", "LineName is required");
        var line = await _db.TransportationLines.FirstOrDefaultAsync(l => l.Id == id);
        if (line == null) return Envelope.Fail("Err142", "Line not found");
        line.LineName = lineName;
        line.ModifiedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST DeleteTransportationLine — delete (Id header). Blocks when the
    /// gathering station still has routes (FK guard).</summary>
    public async Task<object> Remove(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var routeCount = await _db.TransportationVehicleRoutes.CountAsync(r => r.TransportationLineId == id);
        if (routeCount > 0)
            return Envelope.Fail("Err102", "لا يمكن حذف محطة التجمع لارتباطها بخطوط. احذف الخطوط المرتبطة بها أولاً.");
        var line = await _db.TransportationLines.FirstOrDefaultAsync(l => l.Id == id);
        if (line == null) return Envelope.Fail("Err142", "Line not found");
        _db.TransportationLines.Remove(line);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST ApproveTransportationLine — approve/unapprove (Id + Approve headers).</summary>
    public async Task<object> Approve(int id, bool approve, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var line = await _db.TransportationLines.FirstOrDefaultAsync(l => l.Id == id);
        if (line == null) return Envelope.Fail("Err142", "Line not found");
        line.IsApproved = approve;
        line.ApprovedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }
}
