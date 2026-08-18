using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleServiceScheduleCategory")]
public partial class VehicleServiceScheduleCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string ItemName { get; set; }

    public long? ParentId { get; set; }

    public bool? HasChild { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<VehicleServiceScheduleCategory> InverseParent { get; set; } = new List<VehicleServiceScheduleCategory>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual VehicleServiceScheduleCategory Parent { get; set; }

    [InverseProperty("VehicleServiceScheduleCategory")]
    public virtual ICollection<VehicleMaintenanceTypeServiceSheduleCategory> VehicleMaintenanceTypeServiceSheduleCategories { get; set; } = new List<VehicleMaintenanceTypeServiceSheduleCategory>();
}
