
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class RouteDeductionVM
    {
        public int Id { get; set; }

        public long? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public long? SupplierContentPersonId { get; set; }
        public string? SupplierContentPersonIdName { get; set; }

        public long TransportationVehicleRouteId { get; set; }
        public string TransportationVehicleRouteName { get; set; }

        public string Serial { get; set; }

        public DateTime DateOfDeduction { get; set; }

        public decimal? DeductPerRound { get; set; }
        public decimal? routePrice { get; set; }

        public string Cause { get; set; }
        public string typeOfDeduction { get; set; }

        public DateTime? CreationDate { get; set; }

        public long? CreationBy { get; set; }
        public string? CreationName { get; set; }

    }
}
