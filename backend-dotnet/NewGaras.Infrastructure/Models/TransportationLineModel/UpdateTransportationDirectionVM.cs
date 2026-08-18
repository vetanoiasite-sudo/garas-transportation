

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class UpdateTransportationDirectionVM
    {
         public long Id { get; set; }

          public long TransportationVehicleRouteId { get; set; }


        public string RouteDirection { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }
        public decimal? Latitude { get; set; }

        public decimal? Longtitud { get; set; }


    }
}
