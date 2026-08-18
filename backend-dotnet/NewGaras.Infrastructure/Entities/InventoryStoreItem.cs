using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryStoreItem")]
public partial class InventoryStoreItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Required]
    [StringLength(250)]
    public string OrderNumber { get; set; }

    [Column("OrderID")]
    public long OrderId { get; set; }

    [Column("_Balance")]
    public double? Balance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [StringLength(50)]
    public string OperationType { get; set; }

    [Column("Balance", TypeName = "decimal(18, 4)")]
    public decimal Balance1 { get; set; }

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

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? AverageUnitPrice { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryStoreItemCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InvenoryStoreLocationId")]
    [InverseProperty("InventoryStoreItems")]
    public virtual InventoryStoreLocation InvenoryStoreLocation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryStoreItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [InverseProperty("InvStoreItem")]
    public virtual ICollection<InventoryReportItemParent> InventoryReportItemParents { get; set; } = new List<InventoryReportItemParent>();

    [ForeignKey("InventoryStoreId")]
    [InverseProperty("InventoryStoreItems")]
    public virtual InventoryStore InventoryStore { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryStoreItemModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
