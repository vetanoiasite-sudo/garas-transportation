namespace GarasTransport.Domain.Entities;

// Attendance (linked by serial strings — no FK, as in the old system).

public class UserAttendance
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // Person | Bus
    public string Serial { get; set; } = string.Empty; // Person → HrUser.email (fingerprint no.); Bus → route serial
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public DateTime? OneWayDate { get; set; }
    public int? CheckInRouteId { get; set; }
    public int? CheckOutRouteId { get; set; }
    public double? CheckInLatitude { get; set; }
    public double? CheckInLongitude { get; set; }
    public double? CheckOutLatitude { get; set; }
    public double? CheckOutLongitude { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }
}

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; } // recipient (a super admin)
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EntityType { get; set; } // route | line | vehicle | repricing
    public int? EntityId { get; set; }
    public int? FromUserId { get; set; } // the admin who triggered it
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
