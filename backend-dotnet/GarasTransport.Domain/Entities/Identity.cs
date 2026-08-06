namespace GarasTransport.Domain.Entities;

// Identity, sessions & roles (§5.5). Field names mirror the Prisma schema; the
// frozen PascalCase JSON keys live at the DTO layer, so entities stay clean.

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Mobile { get; set; }
    public bool Active { get; set; } = true;
    public int? BranchId { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public HrUser? HrUser { get; set; }
    public ICollection<TransportationVehicleRoute> SupervisedRoutes { get; set; } = new List<TransportationVehicleRoute>();
}

public class UserSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Active { get; set; } = true;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }

    public User? User { get; set; }
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class UserRole
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public bool Active { get; set; } = true;

    public User? User { get; set; }
    public Role? Role { get; set; }
}

public class MaritalStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<HrUser> HrUsers { get; set; } = new List<HrUser>();
}

public class HrUser
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Mobile { get; set; }
    public string? IdentityNumber { get; set; }
    public int? MaritalStatusId { get; set; }
    public string? ImgPath { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public ICollection<TransportationVehicleRouteEmployee> Memberships { get; set; } = new List<TransportationVehicleRouteEmployee>();
    public ICollection<TransportationVehicleRouteEmployeeException> Exceptions { get; set; } = new List<TransportationVehicleRouteEmployeeException>();
}
