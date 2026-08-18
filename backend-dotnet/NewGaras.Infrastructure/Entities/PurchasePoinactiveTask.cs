using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInactiveTask")]
public partial class PurchasePoinactiveTask
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InactiveTaskID")]
    public long InactiveTaskId { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchasePoinactiveTasks")]
    public virtual PurchasePo Po { get; set; }
}
