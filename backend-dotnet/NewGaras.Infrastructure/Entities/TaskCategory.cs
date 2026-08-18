using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskCategory")]
public partial class TaskCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    [InverseProperty("TaskCategory")]
    public virtual ICollection<TaskInfoRevision> TaskInfoRevisions { get; set; } = new List<TaskInfoRevision>();

    [InverseProperty("TaskCategory")]
    public virtual ICollection<TaskInfo> TaskInfos { get; set; } = new List<TaskInfo>();

    [InverseProperty("TaskCategory")]
    public virtual ICollection<TaskPrimarySubCategory> TaskPrimarySubCategories { get; set; } = new List<TaskPrimarySubCategory>();
}
