using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Priority")]
public partial class Priority
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("Priority")]
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

    [InverseProperty("Priority")]
    public virtual ICollection<ProjectTmrevision> ProjectTmrevisions { get; set; } = new List<ProjectTmrevision>();

    [InverseProperty("Priorty")]
    public virtual ICollection<ProjectTm> ProjectTms { get; set; } = new List<ProjectTm>();

    [InverseProperty("Priorty")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [InverseProperty("Proiority")]
    public virtual ICollection<TaskInfoRevision> TaskInfoRevisions { get; set; } = new List<TaskInfoRevision>();
}
