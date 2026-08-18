using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryStoreItemMovement
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    public string InventoryItemName { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [StringLength(1000)]
    public string InventoryStoreName { get; set; }

    [Required]
    [StringLength(250)]
    public string OrderNumber { get; set; }

    [Column("OrderID")]
    public long OrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public bool? Active { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [StringLength(1000)]
    public string CommercialName { get; set; }

    [StringLength(1000)]
    public string MarketName { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("RequstionUOMName")]
    [StringLength(500)]
    public string RequstionUomname { get; set; }

    [Column("RequestionUOMShortName")]
    [StringLength(50)]
    public string RequestionUomshortName { get; set; }

    public bool? StoreActive { get; set; }

    [StringLength(50)]
    public string OperationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Balance { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AverageUnitPrice { get; set; }

    public long? ItemSerialCounter { get; set; }

    [Column("expDate", TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [Column("itemSerial")]
    [StringLength(100)]
    public string ItemSerial { get; set; }

    [Column("releaseParentId")]
    public long? ReleaseParentId { get; set; }

    [Column("finalBalance", TypeName = "decimal(18, 5)")]
    public decimal? FinalBalance { get; set; }

    [Column("addingOrderItemId")]
    public long? AddingOrderItemId { get; set; }

    [Column("addingFromPOId")]
    public long? AddingFromPoid { get; set; }

    [Column("POInvoiceId")]
    public long? PoinvoiceId { get; set; }

    [Column("POInvoiceTotalPrice", TypeName = "decimal(18, 5)")]
    public decimal? PoinvoiceTotalPrice { get; set; }

    [Column("POInvoiceTotalCost", TypeName = "decimal(18, 5)")]
    public decimal? PoinvoiceTotalCost { get; set; }

    [Column("currencyId")]
    public int? CurrencyId { get; set; }

    [Column("rateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    [Column("POInvoiceTotalPriceEGP", TypeName = "decimal(18, 5)")]
    public decimal? PoinvoiceTotalPriceEgp { get; set; }

    [Column("POInvoiceTotalCostEGP", TypeName = "decimal(18, 5)")]
    public decimal? PoinvoiceTotalCostEgp { get; set; }

    [Column("remainItemPrice", TypeName = "decimal(18, 5)")]
    public decimal? RemainItemPrice { get; set; }

    [Column("remainItemCosetEGP", TypeName = "decimal(18, 5)")]
    public decimal? RemainItemCosetEgp { get; set; }

    [Column("remainItemCostOtherCU", TypeName = "decimal(18, 5)")]
    public decimal? RemainItemCostOtherCu { get; set; }

    [Column("holdQty", TypeName = "decimal(18, 5)")]
    public decimal? HoldQty { get; set; }

    [Column("holdReason")]
    [StringLength(500)]
    public string HoldReason { get; set; }

    [StringLength(101)]
    public string FromUser { get; set; }

    [StringLength(500)]
    public string FromSupplier { get; set; }

    public long? SupplierId { get; set; }

    public long? ProjectId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string FromDepartment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateFilter { get; set; }

    [StringLength(21)]
    [Unicode(false)]
    public string OrderType { get; set; }

    [StringLength(250)]
    public string CurrencyName { get; set; }
}
