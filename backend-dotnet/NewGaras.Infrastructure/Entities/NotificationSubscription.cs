using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("NotificationSubscription")]
public partial class NotificationSubscription
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(150)]
    public string SubscriptionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpirationDateTime { get; set; }
}
