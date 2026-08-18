

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class CapacityTransportionNumbersVM
    {
        public int FullCapacity { get; set; }
        public int ActualCapacity { get; set; }
        public int CapacityWithoutExpection { get; set; }
        public int ExpectionNumFromOtherLines { get; set; }
        public int RouteEmployeesToOtherLines { get; set; }

    }
}
