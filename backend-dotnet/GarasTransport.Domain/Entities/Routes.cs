namespace GarasTransport.Domain.Entities;

public class TransportationVehicleRoute
{
    public int Id { get; set; }
    public int TransportationLineId { get; set; }
    public int TransportationVehicleId { get; set; }
    public int? SupplierId { get; set; }
    public int? SupplierContactPersonId { get; set; }
    public int? BranchScheduleId { get; set; }
    public int? SupervisorId { get; set; } // User id (a login user with roles — NOT an HrUser/passenger)
    public string Serial { get; set; } = string.Empty;
    public DateTime? PeriodFrom { get; set; }
    public DateTime? PeriodTo { get; set; }
    public string? FromTime { get; set; } // go time of day (docs: FromoDate)
    public string? ToTime { get; set; } // return time of day
    public string NameOfRoute { get; set; } = string.Empty;
    public double LineCost { get; set; } = 0;
    public bool? OneWay { get; set; } = false;
    public bool Active { get; set; } = true;
    public bool? IsApproved { get; set; } = false;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? ModifiedBy { get; set; }

    public TransportationLine? Line { get; set; }
    public TransportationVehicle? Vehicle { get; set; }
    public Supplier? Supplier { get; set; }
    public SupplierContactPerson? ContactPerson { get; set; }
    public BranchSchedule? BranchSchedule { get; set; }
    public User? Supervisor { get; set; }
    public ICollection<TransportationVehicleRouteDirection> Directions { get; set; } = new List<TransportationVehicleRouteDirection>();
    public ICollection<TransportationVehicleRouteEmployee> Employees { get; set; } = new List<TransportationVehicleRouteEmployee>();
    public ICollection<TransportationVehicleRouteEmployeeException> Exceptions { get; set; } = new List<TransportationVehicleRouteEmployeeException>();
    public ICollection<TransportationVehicleRouteDeduction> Deductions { get; set; } = new List<TransportationVehicleRouteDeduction>();
    public ICollection<TransportationVehicleRouteAccount> Accounts { get; set; } = new List<TransportationVehicleRouteAccount>();
    public ICollection<TransportationRoundForRoute> Rounds { get; set; } = new List<TransportationRoundForRoute>();
    public ICollection<TransportionLineExceptionPrice> ExceptionPrices { get; set; } = new List<TransportionLineExceptionPrice>();
    public ICollection<TransportationLineIncreaseRequestLine> IncreaseLines { get; set; } = new List<TransportationLineIncreaseRequestLine>();
}

public class TransportationVehicleRouteDirection
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public string RouteDirection { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public TransportationVehicleRoute? Route { get; set; }
    public ICollection<TransportationVehicleRouteEmployee> Employees { get; set; } = new List<TransportationVehicleRouteEmployee>();
}

public class TransportationVehicleRouteEmployee
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int HrUserId { get; set; }
    public int? DirectionId { get; set; }
    public string Period { get; set; } = string.Empty; // Go | Return | Both
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public double? DurationLatitude { get; set; }
    public double? DurationLongitude { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public TransportationVehicleRoute? Route { get; set; }
    public HrUser? HrUser { get; set; }
    public TransportationVehicleRouteDirection? Direction { get; set; }
}

public class TransportationVehicleRouteEmployeeException
{
    public int Id { get; set; }
    public int HrUserId { get; set; }
    public int RouteId { get; set; }
    public int? DirectionId { get; set; }
    public string Period { get; set; } = string.Empty;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? DayName { get; set; }
    public DateTime? ExceptionDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ReasonException { get; set; }
    public string? ContactNumber { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }

    public HrUser? HrUser { get; set; }
    public TransportationVehicleRoute? Route { get; set; }
}
