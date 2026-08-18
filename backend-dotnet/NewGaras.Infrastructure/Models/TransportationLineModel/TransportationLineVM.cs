

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransportationLineVM
    {
        public long Id { get; set; }

        public string LineName { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public int RouteNum { get; set; }
        public string NameOfRoute { get; set; }


        public int TransportationVehicleId { get; set; }


        public decimal LineCost { get; set; }

        public bool OneWay { get; set; }

        public bool IsApproved { get; set; }

        public string ApprovedBy { get; set; }
        public int Capacity { get; set; }

        public string TransportationVehicleName { get; set; }


    }
}
