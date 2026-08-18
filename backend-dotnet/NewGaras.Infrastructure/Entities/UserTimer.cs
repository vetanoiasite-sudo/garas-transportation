using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserTimer")]
public partial class UserTimer
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [StringLength(250)]
    public string Time { get; set; }

    public bool AtWork { get; set; }

    [Column("UserIPAddress")]
    [StringLength(250)]
    public string UserIpaddress { get; set; }

    [Column("TaskInfoID")]
    public long TaskInfoId { get; set; }

    [Column("ISPlay")]
    public bool Isplay { get; set; }

    [ForeignKey("TaskInfoId")]
    [InverseProperty("UserTimers")]
    public virtual TaskInfo TaskInfo { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserTimers")]
    public virtual User User { get; set; }
}
