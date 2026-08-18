using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserViewNotification")]
public partial class UserViewNotification
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastViewDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserViewNotifications")]
    public virtual User User { get; set; }
}
