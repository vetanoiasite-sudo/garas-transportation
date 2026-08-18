

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class UpdateTransportationEmployeeVM
    {
        public long Id { get; set; }

        public long TransportationVehicleRouteId { get; set; }
        public long TransportationVehicleRouteDirectionId { get; set; }

        public long hrUserId { get; set; }

        public bool Active { get; set; }

    }
}
