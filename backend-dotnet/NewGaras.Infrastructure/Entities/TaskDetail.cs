using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class TaskDetail
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Required]
    [StringLength(50)]
    public string Priority { get; set; }

    public bool? NeedApproval { get; set; }

    public string CreatorAttachement { get; set; }

    public bool? ScreenMonitoring { get; set; }

    public bool? AllowTime { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ProjectBudget { get; set; }

    [StringLength(50)]
    public string Currency { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Weight { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskDetails")]
    public virtual Task Task { get; set; }
}
