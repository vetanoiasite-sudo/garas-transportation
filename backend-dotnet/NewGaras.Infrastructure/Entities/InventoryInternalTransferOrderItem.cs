using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryInternalTransferOrderItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryInternalTransferOrderID")]
    public long InventoryInternalTransferOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [StringLength(50)]
    public string ItemSerial { get; set; }

    public string Comments { get; set; }

    [Column("TransferredQTY", TypeName = "decimal(18, 4)")]
    public decimal TransferredQty { get; set; }

    [ForeignKey("InventoryInternalTransferOrderId")]
    [InverseProperty("InventoryInternalTransferOrderItems")]
    public virtual InventoryInternalTransferOrder InventoryInternalTransferOrder { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryInternalTransferOrderItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("InventoryInternalTransferOrderItems")]
    public virtual InventoryUom Uom { get; set; }
}
