

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class getAllModifyPriceVM
    {
        public int Id { get; set; }

        public bool IsPercent { get; set; }

        public bool ApproximateToFiveFlag { get; set; }

        public decimal IncreaseCost { get; set; }

        public bool? Approve { get; set; }

        public long? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool ForAllLines { get; set; }

        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }
        public string CreationName { get; set; }
       



        public List<TransportationLineDetailsVM>? transportationLineDetails { get; set; }

    }

    public class TransportationLineDetailsVM
    {
        public long RouteId { get; set; }
        public string RouteName  { get; set; }
        public decimal? PriceBefore { get; set; }
        public decimal? PriceAfter { get; set; }
    }
}
