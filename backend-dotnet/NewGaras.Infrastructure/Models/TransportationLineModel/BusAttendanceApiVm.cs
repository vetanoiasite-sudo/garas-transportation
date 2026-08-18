

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class BusAttendanceApiVm
    {
        public string? LineName { get; set; }
        public string? RouteName { get; set; }
        public string? Serial { get; set; }

        public DateTime Date { get; set; }

        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }
        public bool OneWay { get; set; }

        public int? CheckInUsersCount { get; set; }
        public int? CheckOutUsersCount { get; set; }
        public int? OneWayUsersCount { get; set; }
    }
}
