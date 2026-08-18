using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryInternalBackOrderItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryInternalBackOrderID")]
    public long InventoryInternalBackOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column("_RecivedQuantity")]
    public double? RecivedQuantity { get; set; }

    public string Comments { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [Column("InventoryMatrialReleaseItemID")]
    public long? InventoryMatrialReleaseItemId { get; set; }

    [Column("RemainReleaseQTY", TypeName = "decimal(18, 4)")]
    public decimal? RemainReleaseQty { get; set; }

    [ForeignKey("InventoryInternalBackOrderId")]
    [InverseProperty("InventoryInternalBackOrderItems")]
    public virtual InventoryInternalBackOrder InventoryInternalBackOrder { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryInternalBackOrderItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryMatrialReleaseItemId")]
    [InverseProperty("InventoryInternalBackOrderItems")]
    public virtual InventoryMatrialReleaseItem InventoryMatrialReleaseItem { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("InventoryInternalBackOrderItems")]
    public virtual InventoryUom Uom { get; set; }
}
