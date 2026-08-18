using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchasePoitemInvoicePoSupplier
{
    [Column("PurchasePOItemID")]
    public long PurchasePoitemId { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ActualUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalUnitCost { get; set; }

    [Column("PurchasePOID")]
    public long PurchasePoid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InvoiceDate { get; set; }

    [Column("ToSupplierID")]
    public long? ToSupplierId { get; set; }

    [StringLength(500)]
    public string SupplierName { get; set; }

    [Column("PurchasePOInvoiceID")]
    public long? PurchasePoinvoiceId { get; set; }
}
