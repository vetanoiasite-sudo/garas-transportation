

using Microsoft.AspNetCore.Routing;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class BusAttendanceLineVm
    {
        public string? RouteName { get; set; }


        public List<BusAttendanceApiVm> Attendance { get; set; }
            = new List<BusAttendanceApiVm>();
    }
}
