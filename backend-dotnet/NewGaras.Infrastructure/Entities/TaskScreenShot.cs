using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskScreenShot")]
public partial class TaskScreenShot
{
    [Key]
    public long Id { get; set; }

    public long TaskId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    public string ImgPath { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskScreenShots")]
    public virtual Task Task { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TaskScreenShots")]
    public virtual User User { get; set; }
}
