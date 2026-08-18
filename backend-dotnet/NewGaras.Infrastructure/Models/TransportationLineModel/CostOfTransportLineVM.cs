

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class CostOfTransportLineVM
    {
        public string LineName { get; set; }
        public string SupplierName { get; set; }
        public decimal LineCost { get; set; }
        public bool OneWay { get; set; }
        public decimal? CountOfround { get; set; }
        public int? DeductRoundNum { get; set; }
        public decimal? DeductPerRound { get; set; }
        public decimal NetCost { get; set; }
        public string NameOfRoute { get; set; }
        public string Serial { get; set; }


    }
}
