
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewGaras.Infrastructure.Models.TransportationLine
{
    public class VehicleTypeVM
    {
        public int Id { get; set; }

        [Column("vehicleType")]
        [StringLength(250)]
        public string Type { get; set; }

        [Column("active")]
        public bool Active { get; set; }

    }
}
