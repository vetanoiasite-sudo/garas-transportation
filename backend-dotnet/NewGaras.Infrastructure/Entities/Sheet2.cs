using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
[Table("Sheet2$")]
public partial class Sheet2
{
    [Column("item_name")]
    [StringLength(255)]
    public string ItemName { get; set; }

    [Column("InventoryItemCategoryID")]
    public double? InventoryItemCategoryId { get; set; }

    [Column("purchasing")]
    public double? Purchasing { get; set; }

    [Column("وحده الصرف")]
    public double? وحدهالصرف { get; set; }

    [StringLength(255)]
    public string استيراد { get; set; }

    public double? Code { get; set; }

    public double? Active { get; set; }

    public double? MinBalance { get; set; }

    public double? MaxBalance { get; set; }

    public double? ExchangeFactor { get; set; }

    public double? CalculationType { get; set; }

    public double? UnitPrice { get; set; }

    public double? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }
}
