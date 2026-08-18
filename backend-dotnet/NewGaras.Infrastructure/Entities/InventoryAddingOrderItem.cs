using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryAddingOrderItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryAddingOrderID")]
    public long InventoryAddingOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column("_ReqQuantity")]
    public double? ReqQuantity { get; set; }

    [Column("_RecivedQuantity")]
    public double? RecivedQuantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [StringLength(50)]
    public string ItemSerial { get; set; }

    [Column("QCReportID")]
    public long? QcreportId { get; set; }

    public string Comments { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column("ReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity1 { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantityAfter { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RemainQuantity { get; set; }

    [Column("RecivedQuantityUOP", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantityUop { get; set; }

    [ForeignKey("InventoryAddingOrderId")]
    [InverseProperty("InventoryAddingOrderItems")]
    public virtual InventoryAddingOrder InventoryAddingOrder { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryAddingOrderItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("InventoryAddingOrderItems")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("InventoryAddingOrderItems")]
    public virtual InventoryUom Uom { get; set; }
}
