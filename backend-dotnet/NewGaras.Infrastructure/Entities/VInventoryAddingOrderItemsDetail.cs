using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryAddingOrderItemsDetail
{
    [Column("InventoryAddingOrderItemsID")]
    public long InventoryAddingOrderItemsId { get; set; }

    [Column("InventoryAddingOrderID")]
    public long InventoryAddingOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [Column("SupplierID")]
    public long? SupplierId { get; set; }

    [StringLength(500)]
    public string SupplierName { get; set; }

    [Column("InventoryStoreID")]
    public int? InventoryStoreId { get; set; }

    [StringLength(1000)]
    public string InventoryStoreName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RecivingDate { get; set; }

    public long? CreatedBy { get; set; }
}
