using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VUserBranchGroup
{
    [Column("UserID")]
    public long UserId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Required]
    [StringLength(500)]
    public string GroupName { get; set; }
}
