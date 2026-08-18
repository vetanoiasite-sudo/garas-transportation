using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskBrowserTab")]
public partial class TaskBrowserTab
{
    [Key]
    public long Id { get; set; }

    public long TaskId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(450)]
    public string TabName { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskBrowserTabs")]
    public virtual Task Task { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TaskBrowserTabs")]
    public virtual User User { get; set; }
}
