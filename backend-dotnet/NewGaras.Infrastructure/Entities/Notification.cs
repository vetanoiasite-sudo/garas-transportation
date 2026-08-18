using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Notification")]
public partial class Notification
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Required]
    [StringLength(500)]
    public string Title { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("URL")]
    [StringLength(500)]
    public string Url { get; set; }

    public bool New { get; set; }

    public long? FromUserId { get; set; }

    public int? NotificationProcessId { get; set; }

    [ForeignKey("FromUserId")]
    [InverseProperty("NotificationFromUsers")]
    public virtual User FromUser { get; set; }

    [ForeignKey("NotificationProcessId")]
    [InverseProperty("Notifications")]
    public virtual NotificationProcess NotificationProcess { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("NotificationUsers")]
    public virtual User User { get; set; }
}
