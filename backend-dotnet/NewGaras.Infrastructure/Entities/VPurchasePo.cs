using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchasePo
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("ToSupplierID")]
    public long ToSupplierId { get; set; }

    [StringLength(500)]
    public string SupplierName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Required]
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

    [StringLength(200)]
    public string TypeName { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalInvoicePrice { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalInvoiceCost { get; set; }

    public bool? IsClosed { get; set; }
}
