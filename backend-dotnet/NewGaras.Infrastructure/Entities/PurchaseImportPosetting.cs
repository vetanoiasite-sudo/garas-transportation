using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchaseImportPOSetting")]
public partial class PurchaseImportPosetting
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("USDRate", TypeName = "decimal(18, 2)")]
    public decimal? Usdrate { get; set; }

    [Column("GBPRate", TypeName = "decimal(18, 2)")]
    public decimal? Gbprate { get; set; }

    [Column("EURORate", TypeName = "decimal(18, 2)")]
    public decimal? Eurorate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CustomsDollarRate { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchaseImportPosettings")]
    public virtual PurchasePo Po { get; set; }
}
