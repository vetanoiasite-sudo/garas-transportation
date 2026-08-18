using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskUserMonitor")]
public partial class TaskUserMonitor
{
    [Key]
    public long Id { get; set; }

    public long TaskId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public string ImgPath { get; set; }

    public string TabName { get; set; }

    public string AppName { get; set; }

    public string SmallImgPath { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskUserMonitors")]
    public virtual Task Task { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TaskUserMonitors")]
    public virtual User User { get; set; }
}
