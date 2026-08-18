

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class ModifyTransportationLinePriceVM
    {
        public decimal IncreaseCost { get; set; }
        public bool IsPrecent { get; set; }
        public bool ApproximateToFiveFlag { get; set; }
        public bool ForAllLines { get; set; }
        public List<long>? TransportationLineIds { get; set; }
        public DateTime CreationDate { get; set; }
        public long CreationBy { get; set; }
        public DateTime StartDate { get; set; }

    }
}
