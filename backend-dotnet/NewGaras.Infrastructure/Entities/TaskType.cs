using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskType")]
public partial class TaskType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool? Active { get; set; }

    public bool? CanEdit { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskTypeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskTypeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("TaskType")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
