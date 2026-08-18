using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchasePoItemPo
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column("_ReqQuantity")]
    public double? ReqQuantity { get; set; }

    [Column("_RecivedQuantity")]
    public double? RecivedQuantity { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("FabricationOrderID")]
    public long? FabricationOrderId { get; set; }

    public string Comments { get; set; }

    [Column("PurchasePOID")]
    public long PurchasePoid { get; set; }

    [Column("PurchaseRequestItemID")]
    public long PurchaseRequestItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? EstimatedCost { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? EstimatedUnitCost { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ActualUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalUnitCost { get; set; }

    [StringLength(500)]
    public string InvoiceComments { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalActualPrice { get; set; }

    [Column("ReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity1 { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [StringLength(50)]
    public string ActualUnitPriceUnit { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    [Column("ToSupplierID")]
    public long? ToSupplierId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    [Column("POTypeID")]
    public long? PotypeId { get; set; }

    [Column("AssignedAccountantID")]
    public long? AssignedAccountantId { get; set; }

    [StringLength(50)]
    public string ApprovalStatus { get; set; }

    [StringLength(500)]
    public string AccountantReplyNotes { get; set; }

    public bool? Active { get; set; }

    [StringLength(50)]
    public string TechApprovalStatus { get; set; }

    [StringLength(500)]
    public string TechReplyNotes { get; set; }

    [StringLength(50)]
    public string FinalApprovalStatus { get; set; }

    [StringLength(500)]
    public string FinalApprovalReplyNotes { get; set; }

    [Column("UserIDForTechApprove")]
    public long? UserIdforTechApprove { get; set; }

    [Column("UserIDForFinalApprove")]
    public long? UserIdforFinalApprove { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TechApproveDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinalApproveDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? TotalEstimatedCost { get; set; }

    public bool? SentToSupplier { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SupplierDeliveryDate { get; set; }

    [Column("AssignedPurchasingPersonID")]
    public long? AssignedPurchasingPersonId { get; set; }

    [StringLength(50)]
    public string SentToSupplierMethod { get; set; }

    [Column("SentToSupplierContactPersonID")]
    public long? SentToSupplierContactPersonId { get; set; }

    public bool? IsChecked { get; set; }

    [StringLength(450)]
    public string SupplierInvoiceSerial { get; set; }
}
