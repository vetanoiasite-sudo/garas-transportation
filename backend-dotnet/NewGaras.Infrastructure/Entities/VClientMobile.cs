using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VClientMobile
{
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public bool? HasLogo { get; set; }

    [Column("SalesPersonID")]
    public long SalesPersonId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [StringLength(20)]
    public string Mobile { get; set; }

    public int? NeedApproval { get; set; }

    public long? ClientSerialCounter { get; set; }
}
