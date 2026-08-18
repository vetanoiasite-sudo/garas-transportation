using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleMaintenanceType")]
public partial class VehicleMaintenanceType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public int VehicleRateId { get; set; }

    public string Description { get; set; }

    public string Comment { get; set; }

    public int VehiclePriorityLevelId { get; set; }

    [Column("BOMID")]
    public long? Bomid { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? Milage { get; set; }

    [ForeignKey("Bomid")]
    [InverseProperty("VehicleMaintenanceTypes")]
    public virtual Bom Bom { get; set; }

    [InverseProperty("NextVisitFor")]
    public virtual ICollection<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistoryNextVisitFors { get; set; } = new List<VehicleMaintenanceJobOrderHistory>();

    [InverseProperty("VehicleMaintenanceType")]
    public virtual ICollection<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistoryVehicleMaintenanceTypes { get; set; } = new List<VehicleMaintenanceJobOrderHistory>();

    [InverseProperty("VehicleMaintenanceType")]
    public virtual ICollection<VehicleMaintenanceTypeForModel> VehicleMaintenanceTypeForModels { get; set; } = new List<VehicleMaintenanceTypeForModel>();

    [InverseProperty("VehicleMaintenanceType")]
    public virtual ICollection<VehicleMaintenanceTypeServiceSheduleCategory> VehicleMaintenanceTypeServiceSheduleCategories { get; set; } = new List<VehicleMaintenanceTypeServiceSheduleCategory>();

    [ForeignKey("VehiclePriorityLevelId")]
    [InverseProperty("VehicleMaintenanceTypes")]
    public virtual VehicleMaintenanceTypePriorityLevel VehiclePriorityLevel { get; set; }

    [ForeignKey("VehicleRateId")]
    [InverseProperty("VehicleMaintenanceTypes")]
    public virtual VehicleMaintenanceTypeRate VehicleRate { get; set; }
}
