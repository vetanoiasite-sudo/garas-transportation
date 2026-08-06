namespace GarasTransport.Application.DTOs.Passenger;

// Response DTOs — serialized as-is (PropertyNamingPolicy = null), so every
// property name below is a FROZEN wire key the Flutter/Next clients string-match
// byte-for-byte. Some are intentional typos (FirstNane, Longtitud) — DO NOT fix.

/// <summary>Row of GetTransportationEmployeeList (was the RouteUser anonymous shape).</summary>
public class GetRouteEmployeeDto
{
    public int ID { get; set; }
    public int HrUserId { get; set; }
    public string FirstNane { get; set; } = string.Empty; // ⚠ frozen typo
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public double DirectionLatitude { get; set; }
    public double DirectionLongtitud { get; set; } // ⚠ frozen typo
    public int DirectionId { get; set; }
    public string DirectionName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
}

/// <summary>Row of GetTransportationEmployees (mobile "who rides today"; was RouteUserForMob).</summary>
public class GetRouteEmployeeForMobDto
{
    public int ID { get; set; }
    public int HrUserId { get; set; }
    public string FirstNane { get; set; } = string.Empty; // ⚠ frozen typo
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public double DirectionLatitude { get; set; }
    public double DirectionLongtitud { get; set; } // ⚠ frozen typo
    public int DirectionId { get; set; }
    public string DirectionName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longtitud { get; set; } // ⚠ frozen typo
    public bool RegisteredInLine { get; set; }
    public string CheckIn { get; set; } = string.Empty;
    public string CheckOut { get; set; } = string.Empty;
}

/// <summary>Row of getTransportationRoutesForHrUser.</summary>
public class GetHrUserRouteDto
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int TransportationVehicleRouteDirectionId { get; set; }
    public string Serial { get; set; } = string.Empty;
    public string NameOfRoute { get; set; } = string.Empty;
    public string PeriodFrom { get; set; } = string.Empty;
    public string PeriodTo { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string CreationDate { get; set; } = string.Empty;
    public string ModifiedDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public double DurationLatitude { get; set; }
    public double DurationLongtitud { get; set; } // ⚠ frozen typo
}

/// <summary>Passenger profile (getAllHrUsers / getHrUser rows).</summary>
public class GetHrUserProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public int MaritalStatusId { get; set; }
    public string MaritalStatusName { get; set; } = string.Empty;
    public string ImgPath { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool Active { get; set; }
    public int RoutesCount { get; set; }
}

/// <summary>GetMaritalStatus row.</summary>
public class MaritalStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
