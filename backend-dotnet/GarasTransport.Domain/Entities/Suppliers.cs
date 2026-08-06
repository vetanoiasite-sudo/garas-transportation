namespace GarasTransport.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Fax { get; set; }
    public string? Address { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public ICollection<SupplierContactPerson> Contacts { get; set; } = new List<SupplierContactPerson>();
    public ICollection<TransportationVehicleRoute> Routes { get; set; } = new List<TransportationVehicleRoute>();
    public ICollection<TransportationVehicleRouteDeduction> Deductions { get; set; } = new List<TransportationVehicleRouteDeduction>();
    public ICollection<SupplierPayment> Payments { get; set; } = new List<SupplierPayment>();
    public ICollection<TransportationVehicleRouteAccount> Accounts { get; set; } = new List<TransportationVehicleRouteAccount>();
    public ICollection<TransportationRoundForRoute> Rounds { get; set; } = new List<TransportationRoundForRoute>();
}

public class SupplierContactPerson
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Mobile { get; set; }

    public Supplier? Supplier { get; set; }
    public ICollection<TransportationVehicleRoute> Routes { get; set; } = new List<TransportationVehicleRoute>();
}
