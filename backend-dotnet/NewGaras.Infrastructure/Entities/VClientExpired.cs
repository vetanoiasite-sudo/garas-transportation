using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VClientExpired
{
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastReportDate { get; set; }

    [Column("SalesPersonID")]
    public long SalesPersonId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    public int? UnReportedDays { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ClientCreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastOfferCreationDate { get; set; }

    public int? LastOfferDiffDays { get; set; }

    public int? NeedApproval { get; set; }
}
