
using System.ComponentModel.DataAnnotations.Schema;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransportationRouteUpdateVM
    {
        public long Id { get; set; }

        public int TransportationLineId { get; set; }

        public long? SupplierId { get; set; }

        public long? SupplierContactPersonId { get; set; }

        public long? BranchScheduleId { get; set; }

       // public string Serial { get; set; }

        //public DateTime PeriodFrom { get; set; }


        public bool Active { get; set; }

       // public DateTime CreationDate { get; set; }

       // public long CreationBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public long ModifiedBy { get; set; }
        public DateTime? ToDate { get; set; }

        public DateTime? FromoDate { get; set; }
        public long HrUserId { get; set; }
        public string NameOfRoute { get; set; }

        public int TransportationVehicleId { get; set; }
        public DateTime PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }
        public string LineName { get; set; }

        public decimal LineCost { get; set; }

        public bool OneWay { get; set; }

        public bool IsApproved { get; set; }

        public string ApprovedBy { get; set; }


        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }

       


    }
}
