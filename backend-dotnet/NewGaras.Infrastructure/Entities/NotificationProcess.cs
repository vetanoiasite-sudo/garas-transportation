using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("NotificationProcess")]
public partial class NotificationProcess
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(250)]
    public string ProcessName { get; set; }

    [InverseProperty("NotificationProcess")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
