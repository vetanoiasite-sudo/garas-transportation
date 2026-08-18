

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class DashBoardForHrUsersFromDuration
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TransportionlineName { get; set; }
        public string SupplierName { get; set; }
        public string supplierContactPersonName { get; set; }
        public string VehicleType { get; set; }
        public string SupervisorName { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }
        public string NameOfRoute { get; set; }

        // القائمة الجديدة التي تحتوي على تفاصيل الأيام
        public List<AttendanceDayDetail> AttendanceHistory { get; set; } = new List<AttendanceDayDetail>();
    }
    public class AttendanceDayDetail
    {
        public DateTime Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool ISAttendace { get; set; }
    }
}

