using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryStoreItemPriceReport
{
    [StringLength(50)]
    public string Code { get; set; }

    public string InventoryItemName { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    public string Description { get; set; }

    [Column("RequestionUOMShortName")]
    [StringLength(50)]
    public string RequestionUomshortName { get; set; }

    [StringLength(1000)]
    public string InventoryStoreName { get; set; }

    [StringLength(1000)]
    public string CategoryName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinBalance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaxBalance { get; set; }

    public bool? Active { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    public bool? StoreActive { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? Balance { get; set; }

    [Column("SUMCustomeUnitPrice", TypeName = "decimal(38, 6)")]
    public decimal? SumcustomeUnitPrice { get; set; }

    [Column("SUMAverageUnitPrice", TypeName = "decimal(38, 6)")]
    public decimal? SumaverageUnitPrice { get; set; }

    [Column("SUMMaxUnitPrice", TypeName = "decimal(38, 6)")]
    public decimal? SummaxUnitPrice { get; set; }

    [Column("SUMLastUnitPrice", TypeName = "decimal(38, 6)")]
    public decimal? SumlastUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CustomeUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? LastUnitPrice { get; set; }

    public int? CalculationType { get; set; }

    [Column("finalBalance", TypeName = "decimal(18, 5)")]
    public decimal? FinalBalance { get; set; }

    [Column("expDate", TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [Column("itemSerial")]
    [StringLength(100)]
    public string ItemSerial { get; set; }

    [Column("releaseParentId")]
    public long? ReleaseParentId { get; set; }

    [Column("addingOrderItemId")]
    public long? AddingOrderItemId { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? StockBalance { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? ReqQuantity { get; set; }

    [Column(TypeName = "decimal(38, 5)")]
    public decimal? StockFinalBalance { get; set; }
}
