namespace GarasTransport.Domain.Entities;

public class BranchSchedule
{
    public int Id { get; set; }
    public int ShiftNumber { get; set; }
    public int WeekDayId { get; set; } // 1=Sunday … 7=Saturday
    public string From { get; set; } = string.Empty; // HH:mm
    public string To { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public int? BranchId { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? ModifiedBy { get; set; }

    public ICollection<TransportationVehicleRoute> Routes { get; set; } = new List<TransportationVehicleRoute>();
}

public class TransportationLine
{
    public int Id { get; set; }
    public string LineName { get; set; } = string.Empty;
    public bool? IsApproved { get; set; } = false;
    public int? ApprovedBy { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? ModifiedBy { get; set; }

    public ICollection<TransportationVehicleRoute> Routes { get; set; } = new List<TransportationVehicleRoute>();
}

public class VehicleType
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool Active { get; set; } = true;

    public ICollection<TransportationVehicle> Vehicles { get; set; } = new List<TransportationVehicle>();
}

public class TransportationVehicle
{
    public int Id { get; set; }
    public int VehicleTypeId { get; set; }
    public int Capacity { get; set; }
    public bool IsApproved { get; set; } = false;
    public string? ApprovedBy { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? ModifiedBy { get; set; }

    public VehicleType? VehicleType { get; set; }
    public ICollection<TransportationVehicleRoute> Routes { get; set; } = new List<TransportationVehicleRoute>();
}
