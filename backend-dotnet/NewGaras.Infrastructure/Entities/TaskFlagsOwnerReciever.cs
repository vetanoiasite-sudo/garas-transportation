using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskFlagsOwnerReciever")]
public partial class TaskFlagsOwnerReciever
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool? Read { get; set; }

    public bool? Flag { get; set; }

    public bool? Star { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskFlagsOwnerRecievers")]
    public virtual Task Task { get; set; }
}
