using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchaseRequestItemsPoAo
{
    [Column("PurchaseRequestID")]
    public long PurchaseRequestId { get; set; }

    [Column("PurchaseRequestItemsID")]
    public long? PurchaseRequestItemsId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchaseRequestItemQuantity { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long? InventoryMatrialRequestItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchaseRequestQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchasedRequestRemainQuantity { get; set; }

    [Column("InventoryMatrialRequestID")]
    public long? InventoryMatrialRequestId { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RequestQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RequestRecivedQuantity { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("FabricationOrderID")]
    public long? FabricationOrderId { get; set; }

    [StringLength(50)]
    public string InventoryItemCode { get; set; }

    public string InventoryItemName { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    public int? FabNumber { get; set; }

    [Column("InventoryUOMShortName")]
    [StringLength(50)]
    public string InventoryUomshortName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PurchaseRequestDate { get; set; }

    [Required]
    [StringLength(50)]
    public string PurchaseRequestStatus { get; set; }

    [Column("IsDirectPR")]
    public bool? IsDirectPr { get; set; }

    [StringLength(5)]
    public string CurrencyShortName { get; set; }

    [Column("PurchasePOID")]
    public long? PurchasePoid { get; set; }

    [Column("PurchasePOItemReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? PurchasePoitemReqQuantity { get; set; }

    [Column("PurchasePOItemRecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? PurchasePoitemRecivedQuantity { get; set; }

    [Column("PurchasePOItemEstimatedCost", TypeName = "decimal(18, 4)")]
    public decimal? PurchasePoitemEstimatedCost { get; set; }

    [Column("PurchasePOItemEstimatedUnitCost", TypeName = "decimal(18, 4)")]
    public decimal? PurchasePoitemEstimatedUnitCost { get; set; }

    [Column("PurchasePOItemActualUnitPrice", TypeName = "decimal(18, 5)")]
    public decimal? PurchasePoitemActualUnitPrice { get; set; }

    [Column("PurchasePOItemFinalUnitCost", TypeName = "decimal(18, 5)")]
    public decimal? PurchasePoitemFinalUnitCost { get; set; }

    [Column("PurchasePOItemTotalActualPrice", TypeName = "decimal(18, 5)")]
    public decimal? PurchasePoitemTotalActualPrice { get; set; }

    [Column("PurchasePORequestDate", TypeName = "datetime")]
    public DateTime? PurchasePorequestDate { get; set; }

    [Column("PurchasePOCreatedBy")]
    public long? PurchasePocreatedBy { get; set; }

    [Column("PurchasePOStatus")]
    [StringLength(50)]
    public string PurchasePostatus { get; set; }

    [Column("PurchasePOSupplierID")]
    public long? PurchasePosupplierId { get; set; }

    [Column("PurchasePOSupplierName")]
    [StringLength(500)]
    public string PurchasePosupplierName { get; set; }

    [Column("PurchasePOTypeName")]
    [StringLength(200)]
    public string PurchasePotypeName { get; set; }

    public long? InventoryAddingOrderCreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InventoryAddingOrderRecivingDate { get; set; }

    [Column("InventoryAddingOrderSupplierID")]
    public long? InventoryAddingOrderSupplierId { get; set; }

    [StringLength(500)]
    public string InventoryAddingOrderSupplierName { get; set; }

    [StringLength(1000)]
    public string InventoryAddingOrderStoreName { get; set; }

    [Column("InventoryAddingOrderID")]
    public long? InventoryAddingOrderId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? InventoryAddingOrderReqQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? InventoryAddingOrderItemsRecivedQuantity { get; set; }
}
