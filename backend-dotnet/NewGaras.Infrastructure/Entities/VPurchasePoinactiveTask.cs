using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchasePoinactiveTask
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InactiveTaskID")]
    public long InactiveTaskId { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public bool Active { get; set; }

    public bool? TaskActive { get; set; }

    [Column("TaskTypeID")]
    public int? TaskTypeId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [StringLength(100)]
    public string RefrenceNumber { get; set; }

    [StringLength(500)]
    public string TaskTypeName { get; set; }

    [StringLength(500)]
    public string TaskUser { get; set; }
}
