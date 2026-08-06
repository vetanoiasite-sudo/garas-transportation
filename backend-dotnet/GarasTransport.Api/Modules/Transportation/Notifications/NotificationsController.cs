using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Notifications;

// Base path /api/Transportation; every action requires CompanyName + UserToken
// via the filter. Notifications belong to the calling user (populated only for
// super admins, who are the approval group).
[Route("api/Transportation")]
[HeaderAuth]
public class NotificationsController : ApiControllerBase
{
    private readonly NotificationsService _service;

    public NotificationsController(NotificationsService service) => _service = service;

    [HttpGet("GetNotifications")]
    public Task<object> List() =>
        _service.List(CurrentUser.UserId, HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20));

    [HttpGet("GetUnreadNotificationsCount")]
    public Task<object> Unread() => _service.UnreadCount(CurrentUser.UserId);

    [HttpPost("MarkNotificationRead")]
    public Task<object> MarkRead() => _service.MarkRead(CurrentUser.UserId, HeaderInt("Id", 0));

    [HttpPost("MarkAllNotificationsRead")]
    public Task<object> MarkAllRead() => _service.MarkAllRead(CurrentUser.UserId);
}
