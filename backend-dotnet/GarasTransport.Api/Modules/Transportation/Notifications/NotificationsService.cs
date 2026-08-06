using System.Globalization;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Notifications;

public class NotifyOptions
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EntityType { get; set; } // route | line | vehicle | repricing
    public int? EntityId { get; set; }
}

// The super-admin role id (مسؤول عام). Only super admins receive approval
// notifications, mirroring the old backend which fanned "Need Approval" notices
// out to every "Transportation Super Admin" user.
public class NotificationsService
{
    private const int SuperAdminRoleId = 216;
    private readonly AppDbContext _db;

    public NotificationsService(AppDbContext db) => _db = db;

    private Task<List<int>> SuperAdminIds() =>
        _db.UserRoles.Where(r => r.RoleId == SuperAdminRoleId && r.Active)
            .Select(r => r.UserId).ToListAsync();

    private Task<bool> IsSuperAdmin(int userId) =>
        _db.UserRoles.AnyAsync(r => r.UserId == userId && r.RoleId == SuperAdminRoleId);

    /// <summary>
    /// Fan an approval notice out to every super admin. Skips the case where the
    /// actor is themselves a super admin (their own actions don't need to notify
    /// the approval group).
    /// </summary>
    public async Task NotifySuperAdmins(int actorUserId, NotifyOptions opts)
    {
        if (await IsSuperAdmin(actorUserId)) return;
        var supers = await SuperAdminIds();
        var recipientIds = supers.Distinct().Where(id => id != actorUserId).ToList();
        if (recipientIds.Count == 0) return;
        _db.Notifications.AddRange(recipientIds.Select(userId => new Notification
        {
            UserId = userId,
            Title = opts.Title,
            Description = opts.Description,
            EntityType = opts.EntityType,
            EntityId = opts.EntityId,
            FromUserId = actorUserId,
        }));
        await _db.SaveChangesAsync();
    }

    /// <summary>GET GetNotifications — the caller's notifications, newest first.</summary>
    public async Task<object> List(int userId, int pageNo = 1, int noOfItems = 20)
    {
        var query = _db.Notifications.Where(n => n.UserId == userId);
        var total = await query.CountAsync();
        var rows = await query
            .OrderByDescending(n => n.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var data = rows.Select(n => new
        {
            Id = n.Id,
            Title = n.Title,
            Description = n.Description ?? string.Empty,
            EntityType = n.EntityType ?? string.Empty,
            EntityId = n.EntityId ?? 0,
            FromUserId = n.FromUserId ?? 0,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
        });
        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET GetUnreadNotificationsCount — number of unread notices for the caller.</summary>
    public async Task<object> UnreadCount(int userId)
    {
        var count = await _db.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        return Envelope.Success(count);
    }

    /// <summary>POST MarkNotificationRead — mark one notice read (only the caller's own).</summary>
    public async Task<object> MarkRead(int userId, int id)
    {
        var rows = await _db.Notifications.Where(n => n.Id == id && n.UserId == userId).ToListAsync();
        foreach (var n in rows) n.IsRead = true;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST MarkAllNotificationsRead — mark all the caller's notices read.</summary>
    public async Task<object> MarkAllRead(int userId)
    {
        var rows = await _db.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        foreach (var n in rows) n.IsRead = true;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(null, $"{rows.Count}");
    }
}
