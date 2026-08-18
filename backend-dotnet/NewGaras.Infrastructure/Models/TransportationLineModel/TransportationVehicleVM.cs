using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.TransportationLine
{
    public class TransportationVehicleVM
    {
        public int Id { get; set; }

        public int VehicleTypeId { get; set; }

        public int Capacity { get; set; }

        public bool IsApproved { get; set; }

        public string ApprovedBy { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public long ModifiedBy { get; set; }


      

    }
}
