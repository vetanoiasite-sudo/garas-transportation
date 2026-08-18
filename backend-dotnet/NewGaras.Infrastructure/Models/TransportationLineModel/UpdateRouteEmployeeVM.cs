

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class UpdateRouteEmployeeVM
    {
        public long Id { get; set; }

        public long TransportationVehicleRouteId { get; set; }

        public long HrUserId { get; set; }

        public bool Active { get; set; }

      
        public long? TransportationVehicleRouteDirectionId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Period { get; set; }

        public decimal? DurationLatitude { get; set; }

        public decimal? DurationLongtitud { get; set; }
    }
}
