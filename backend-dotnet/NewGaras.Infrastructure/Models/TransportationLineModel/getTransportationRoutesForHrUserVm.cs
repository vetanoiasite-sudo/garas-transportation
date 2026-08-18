

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class getTransportationRoutesForHrUserVm
    {
        public long Id { get; set; }
        public long RouteId { get; set; }
        public string Serial { get; set; }
        public string TranspotionLineName { get; set; }

        public DateTime PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }



        public DateTime? ToDate { get; set; }

        public DateTime? FromDate { get; set; }
        public string Period { get; set; }
        public decimal? DurationLatitude { get; set; }
        public decimal? DurationLongtitud { get; set; }
        public long? TransportationVehicleRouteDirectionId { get; set; }
        public string NameOfRoute { get; set; }

    }
}
