


using System.ComponentModel.DataAnnotations.Schema;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransportationRouteVM
    {
        public long Id { get; set; }

        public string LineName { get; set; }
        public int LineId { get; set; }
        public decimal LineCost { get; set; }
        public string SupplierName { get; set; }
        public long SupplierId { get; set; }
        public string SupplierContactPersonName { get; set; }
        public long SupplierContactPersonId { get; set; }
        public string Serial { get; set; }
        public long branchScheduleId { get; set; }
        public int ShiftNumber { get; set; }
        public DateTime? PeriodTo { get; set; }
        // comment
        public string? BusSupervisor { get; set; }
        public long BuSupervisorId { get; set; }
        public string BusType { get; set; }
        public int FullCapacity { get; set; }
        public int UserInRoute { get; set; }
        public int ActualCapacity { get; set; }
        public int ActualUserAttedance { get; set; }
        public int ExpectionNumFromOtherLines { get; set; }
        public int RouteEmployeesToOtherLines { get; set; }
        public int DirectionNum { get; set; }
        public int MuslimNum { get; set; }
        public int christianNum { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime PeriodFrom { get; set; }
        public bool? OneWay { get; set; }

        public DateTime? FromoDate { get; set; }
        public string NameOfRoute { get; set; }

        public int TransportationVehicleId { get; set; }
        public string TransportationVehicleName { get; set; }


    }
}
