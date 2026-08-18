

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class EmployeeExceptionVM
    {
        public long Id { get; set; }

        public long TransportationVehicleRouteId { get; set; }
        public string TransportationLineName { get; set; }

         public long HrUserId { get; set; }
         public string HrUserName { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }
        public string CreationName { get; set; }

        public long? TransportationVehicleRouteDirectionId { get; set; }
        public string? TransportationVehicleRouteDirectionName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Period { get; set; }

        public string DayName { get; set; }

        public decimal? LatitudeExceptional { get; set; }

        public decimal? LongtitudExceptional { get; set; }

        public DateTime? ExceptionDate { get; set; }

        public string ExceptionDatePeriod { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitud { get; set; }

        public long? ExceptionDirectionId { get; set; }
        public string? ExceptionDirectionName { get; set; }
        public string ContactNumber { get; set; }

        public string ReasonException { get; set; }
    }
}
