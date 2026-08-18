using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleBrand")]
public partial class VehicleBrand
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [InverseProperty("Brand")]
    public virtual ICollection<VehicleMaintenanceTypeForModel> VehicleMaintenanceTypeForModels { get; set; } = new List<VehicleMaintenanceTypeForModel>();

    [InverseProperty("VehicleBrand")]
    public virtual ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();

    [InverseProperty("Brand")]
    public virtual ICollection<VehiclePerClient> VehiclePerClients { get; set; } = new List<VehiclePerClient>();
}
