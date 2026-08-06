namespace GarasTransport.Domain.Entities;

// Financials — deductions, repricing, payments, accounts, rounds, prices.

public class TransportationVehicleRouteDeduction
{
    public int Id { get; set; }
    public int? SupplierId { get; set; }
    public int RouteId { get; set; }
    public string Serial { get; set; } = string.Empty;
    public DateTime DateOfDeduction { get; set; }
    public double DeductPerRound { get; set; }
    public string? Cause { get; set; }
    public string TypeOfDedct { get; set; } = string.Empty; // Normal | Taxes
    public double? RoutePrice { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }

    public Supplier? Supplier { get; set; }
    public TransportationVehicleRoute? Route { get; set; }
}

public class TransportationLineIncreaseRequest
{
    public int Id { get; set; }
    public bool IsPercent { get; set; }
    public double IncreaseCost { get; set; }
    public bool ForAllLines { get; set; }
    public bool ApproximateToFiveFlag { get; set; } = false;
    public DateTime StartDate { get; set; }
    public bool? Approve { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int? CreationBy { get; set; }

    public ICollection<TransportationLineIncreaseRequestLine> Lines { get; set; } = new List<TransportationLineIncreaseRequestLine>();
}

public class TransportationLineIncreaseRequestLine
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int RouteId { get; set; }
    public double PriceBefore { get; set; } = 0;
    public double PriceAfter { get; set; } = 0;

    public TransportationLineIncreaseRequest? Request { get; set; }
    public TransportationVehicleRoute? Route { get; set; }
}

public class TransportionLineExceptionPrice
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public double Price { get; set; }
    public DateTime FromDate { get; set; }

    public TransportationVehicleRoute? Route { get; set; }
}

public class SupplierPayment
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public double Payment { get; set; }
    public DateTime DatePayment { get; set; }
    public DateTime? StartDate { get; set; }
    public int? NumberOfMonths { get; set; }
    public string TypeOfDebt { get; set; } = string.Empty; // Normal | Advance
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public Supplier? Supplier { get; set; }
    public ICollection<DistributionSupplierPayment> Distribution { get; set; } = new List<DistributionSupplierPayment>();
}

public class DistributionSupplierPayment
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public double Payment { get; set; }
    public int MonthNum { get; set; }
    public int YearNum { get; set; }

    public SupplierPayment? SupplierPayment { get; set; }
}

public class TransportationVehicleRouteAccount
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int? SupplierId { get; set; }
    public string Serial { get; set; } = string.Empty;
    public DateTime DateOfMonth { get; set; }
    public double CountOfRounds { get; set; } = 0;
    public double? PriceOfRounds { get; set; }
    public double DeductPerRounds { get; set; } = 0;

    public TransportationVehicleRoute? Route { get; set; }
    public Supplier? Supplier { get; set; }
}

public class TransportationRoundForRoute
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int? SupplierId { get; set; }
    public string Serial { get; set; } = string.Empty;
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public DateTime? OneWayDate { get; set; }
    public double? PriceRound { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public TransportationVehicleRoute? Route { get; set; }
    public Supplier? Supplier { get; set; }
}
