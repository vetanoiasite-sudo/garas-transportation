

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class DashBoardVM
    {
        public int TransportLinesNum { get; set; }
        public int VehiclesNum { get; set; }
        public int TwoWayVehiclesNum { get; set; }
        public int OneWayVehiclesNum { get; set; }
        public int VehiclesTypeNum { get; set; }
        public int Capacity { get; set; }
        public int CheckInVehicleAttendanceNum { get; set; }
        public int CheckOutVehicleAttendanceNum { get; set; }
        public int OneWayVehicleAttendanceNum { get; set; }
        // المفروض تكون percentages مش ارقام    
        public int OneWayVehicleAttendanceNumCheckOut { get; set; }
        public int HrUsersNum { get; set; }
        public int AllHrUsersNum { get; set; }
        public int HrUsersAttendanceNum { get; set; }
        // 
        public int AllHrUsersAttendanceNum { get; set; }
        public int HrUsersAttendanceNumCheckOut { get; set; }
        public int SuppliersNum { get; set; }
        public int AllSuppliersNum { get; set; }
        public decimal HrUsersPercent { get; set; }
        public decimal HrUsersOfCapacityPercent { get; set; }
        public decimal CheckInVehiclePercent { get; set; }
        public decimal CheckOutVehiclePercent { get; set; }
        public decimal OneWayVehiclePercent { get; set; }
        // المفروض تكون   ارقام Not percentages
        public decimal OneWayVehiclePercentCheckOut { get; set; }
    }
}
