using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProductTargetChart
{
    [Column("BranchID")]
    public int BranchId { get; set; }

    [Required]
    [StringLength(500)]
    public string BranchName { get; set; }

    [Column("ProductID")]
    public long ProductId { get; set; }

    [Required]
    [StringLength(1000)]
    public string ProductName { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Amount { get; set; }

    public int Year { get; set; }
}
