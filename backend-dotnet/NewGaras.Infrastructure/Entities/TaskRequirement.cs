using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskRequirement")]
public partial class TaskRequirement
{
    [Key]
    public long Id { get; set; }

    public long TaskId { get; set; }

    [Required]
    public string Name { get; set; }

    [Column("percentage", TypeName = "decimal(8, 4)")]
    public decimal Percentage { get; set; }

    public bool IsFinished { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column("WorkingHourTrackingID")]
    public long? WorkingHourTrackingId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskRequirementCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskRequirementModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskRequirements")]
    public virtual Task Task { get; set; }

    [ForeignKey("WorkingHourTrackingId")]
    [InverseProperty("TaskRequirements")]
    public virtual WorkingHourseTracking WorkingHourTracking { get; set; }
}
