
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class DashBoardForHrUsers
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
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool ISAttendace { get; set; }

    }
}
