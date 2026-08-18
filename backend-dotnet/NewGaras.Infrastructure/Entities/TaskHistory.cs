using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskHistory")]
public partial class TaskHistory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    public string Action { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskHistories")]
    public virtual Task Task { get; set; }
}
