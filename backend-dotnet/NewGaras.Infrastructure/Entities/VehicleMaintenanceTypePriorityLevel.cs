using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleMaintenanceTypePriorityLevel")]
public partial class VehicleMaintenanceTypePriorityLevel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string PriorityLevelName { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("VehiclePriorityLevel")]
    public virtual ICollection<VehicleMaintenanceType> VehicleMaintenanceTypes { get; set; } = new List<VehicleMaintenanceType>();
}
