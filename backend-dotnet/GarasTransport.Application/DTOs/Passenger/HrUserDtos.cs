namespace GarasTransport.Application.DTOs.Passenger;

// Request DTOs — passenger (HrUser) profiles. Names/casing match the frozen
// wire contract (requests bind case-insensitively; kept identical to the old
// module bodies for fidelity).

/// <summary>Add/Update passenger profile payload (was HrUserBody).</summary>
public class HrUserDto
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Mobile { get; set; }
    public string? IdentityNumber { get; set; }
    public int? MaritalStatusId { get; set; }
    public string? ImgPath { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool? Active { get; set; }
    public string? Email { get; set; }
}

/// <summary>One membership row in AddTransportationEmployee's Data[] (frozen lowercase keys).</summary>
public class EmployeeInputDto
{
    public int hrUserId { get; set; }
    public int? TransportationVehicleRouteDirectionId { get; set; }
    public bool? Active { get; set; }
    public string? period { get; set; }
}

/// <summary>UpdateTransportationEmployee payload.</summary>
public class UpdateEmployeeDto
{
    public int Id { get; set; }
    public int hrUserId { get; set; }
    public int TransportationVehicleRouteId { get; set; }
    public int? TransportationVehicleRouteDirectionId { get; set; }
    public bool? Active { get; set; }
}

/// <summary>One route row in AddRoutesForEmployee's Data[].</summary>
public class RouteForEmployeeInputDto
{
    public int RouteId { get; set; }
    public bool? Active { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? Period { get; set; }
    public double? DurationLatitude { get; set; }
    public double? DurationLongtitud { get; set; } // ⚠ frozen typo key
    public int? TransportationVehicleRouteDirectionId { get; set; }
}

/// <summary>UpdateRouteEmployee payload.</summary>
public class UpdateRouteEmployeeDto
{
    public int Id { get; set; }
    public int TransportationVehicleRouteId { get; set; }
    public int HrUserId { get; set; }
    public bool? Active { get; set; }
    public int? TransportationVehicleRouteDirectionId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? Period { get; set; }
    public double? DurationLatitude { get; set; }
    public double? DurationLongtitud { get; set; } // ⚠ frozen typo key
}
