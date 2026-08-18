using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleType")]
public partial class VehicleType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Type { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [InverseProperty("VehicleType")]
    public virtual ICollection<TransportationVehicle> TransportationVehicles { get; set; } = new List<TransportationVehicle>();
}
