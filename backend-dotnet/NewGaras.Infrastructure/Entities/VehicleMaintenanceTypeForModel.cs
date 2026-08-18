using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleMaintenanceTypeForModel")]
public partial class VehicleMaintenanceTypeForModel
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long VehicleMaintenanceTypeId { get; set; }

    public bool? ForAllModles { get; set; }

    public int? BrandId { get; set; }

    public int? ModelId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("VehicleMaintenanceTypeForModels")]
    public virtual VehicleBrand Brand { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("VehicleMaintenanceTypeForModels")]
    public virtual VehicleModel Model { get; set; }

    [ForeignKey("VehicleMaintenanceTypeId")]
    [InverseProperty("VehicleMaintenanceTypeForModels")]
    public virtual VehicleMaintenanceType VehicleMaintenanceType { get; set; }
}
