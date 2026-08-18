using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskApplicationOpen")]
public partial class TaskApplicationOpen
{
    [Key]
    public long Id { get; set; }

    public long TaskId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(450)]
    public string AppName { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskApplicationOpens")]
    public virtual Task Task { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TaskApplicationOpens")]
    public virtual User User { get; set; }
}
