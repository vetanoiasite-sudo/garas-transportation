using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryStoreItem
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    public string InventoryItemName { get; set; }

    public string Description { get; set; }

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

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool? Active { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [StringLength(1000)]
    public string CategoryName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinBalance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaxBalance { get; set; }

    [StringLength(1000)]
    public string CommercialName { get; set; }

    [StringLength(1000)]
    public string MarketName { get; set; }

    public string Details { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column("PurchasingUOMID")]
    public int? PurchasingUomid { get; set; }

    [Column("PurchasingUOMName")]
    [StringLength(500)]
    public string PurchasingUomname { get; set; }

    [Column("PurchasingUOMShortName")]
    [StringLength(50)]
    public string PurchasingUomshortName { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("RequstionUOMName")]
    [StringLength(500)]
    public string RequstionUomname { get; set; }

    [Column("RequestionUOMShortName")]
    [StringLength(50)]
    public string RequestionUomshortName { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }

    public int? CalculationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CustomeUnitPrice { get; set; }

    public byte[] Image { get; set; }

    public bool? HasImage { get; set; }

    public bool? StoreActive { get; set; }

    [StringLength(50)]
    public string OperationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Balance { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? LastUnitPrice { get; set; }

    [Column("PartNO")]
    [StringLength(500)]
    public string PartNo { get; set; }

    [StringLength(250)]
    public string CostName1 { get; set; }

    [StringLength(250)]
    public string CostName2 { get; set; }

    [StringLength(250)]
    public string CostName3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount3 { get; set; }

    public long? ItemSerialCounter { get; set; }

    [Column("PriorityID")]
    public int? PriorityId { get; set; }

    [StringLength(50)]
    public string PriorityName { get; set; }

    [Column("InvenoryStoreLocationID")]
    public int? InvenoryStoreLocationId { get; set; }

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
}
