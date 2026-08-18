
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransportationDirectionByRouteIdVm
    {
        public long Id { get; set; }
        public string RouteDirection { get; set; }
        public string Description { get; set; }
        public decimal? Latitude { get; set; }

        public decimal? Longtitud { get; set; }

    }
}
