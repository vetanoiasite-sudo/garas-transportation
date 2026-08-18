

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class AddRoutesForHrUserVM
    {
        public long RouteId { get; set; }

        public bool Active { get; set; }
        //   public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Period { get; set; }

        public decimal? DurationLatitude { get; set; }

        public decimal? DurationLongtitud { get; set; }
        public long? TransportationVehicleRouteDirectionId { get; set; }
    }
    public class AddRoutesForHrUserDto
    {

        public List<AddRoutesForHrUserVM> Data { get; set; }


    }
}
