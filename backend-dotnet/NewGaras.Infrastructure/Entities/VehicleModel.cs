using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleModel")]
public partial class VehicleModel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int VehicleBrandId { get; set; }

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

    [InverseProperty("VehicleModel")]
    public virtual ICollection<VehicleBodyType> VehicleBodyTypes { get; set; } = new List<VehicleBodyType>();

    [ForeignKey("VehicleBrandId")]
    [InverseProperty("VehicleModels")]
    public virtual VehicleBrand VehicleBrand { get; set; }

    [InverseProperty("Model")]
    public virtual ICollection<VehicleMaintenanceTypeForModel> VehicleMaintenanceTypeForModels { get; set; } = new List<VehicleMaintenanceTypeForModel>();

    [InverseProperty("Model")]
    public virtual ICollection<VehiclePerClient> VehiclePerClients { get; set; } = new List<VehiclePerClient>();
}
