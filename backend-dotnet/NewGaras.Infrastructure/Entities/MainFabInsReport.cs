using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class MainFabInsReport
{
    [Column("UserID")]
    public long UserId { get; set; }

    [Column("userName")]
    [StringLength(101)]
    public string UserName { get; set; } = null!;

    [StringLength(500)]
    public string DepName { get; set; } = null!;

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [StringLength(500)]
    public string? ProjectName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HourNum { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Evaluation { get; set; }

    public string? Comment { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string RequestType { get; set; } = null!;
}
