using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesBranchUserTargetTargetYear
{
    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Amount { get; set; }

    public int Year { get; set; }
}
