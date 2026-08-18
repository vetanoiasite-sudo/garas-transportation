using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectSalesOfferClient
{
    public long? SalesOfferId { get; set; }

    public long ProjectId { get; set; }

    public long? ClientId { get; set; }

    public DateOnly? SalesOfferStartDate { get; set; }

    public DateOnly? SalesOfferEndDate { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    public long? SalesOfferCreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferCreationDate { get; set; }

    public long? SalesOfferModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferModified { get; set; }

    public bool? SalesOfferActive { get; set; }

    [StringLength(250)]
    public string SalesOfferStatus { get; set; }

    public bool? SalesOfferCompleted { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? SalesOfferOfferAmount { get; set; }

    public bool? SendingOfferConfirmation { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SendingOfferDate { get; set; }

    [StringLength(500)]
    public string SendingOfferBy { get; set; }

    [StringLength(1000)]
    public string SendingOfferTo { get; set; }

    public bool? ClientApprove { get; set; }

    public int? VersionNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClientApprovalDate { get; set; }

    [StringLength(500)]
    public string ProductType { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [StringLength(20)]
    public string ContactPersonMobile { get; set; }

    [StringLength(50)]
    public string ContactPersonEmail { get; set; }

    [StringLength(500)]
    public string ContactPersonName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferProjectStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferProjectEndDate { get; set; }

    [Column("SalesOfferBranchID")]
    public int? SalesOfferBranchId { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(50)]
    public string ContractType { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    public bool? ClientNeedsDiscount { get; set; }

    [StringLength(250)]
    public string RejectionReason { get; set; }

    public bool? NeedsInvoice { get; set; }

    public bool? NeedsExtraCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OfferExpirationDate { get; set; }

    public int? OfferExpirationPeriod { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ExtraOrDiscountPriceBySalesPerson { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReminderDate { get; set; }

    public bool ProjectClosed { get; set; }

    public int ProjectRevision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ProjectStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ProjectEndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallEndDate { get; set; }

    [StringLength(10)]
    public string InstallDuration { get; set; }

    public long ProjectCreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ProjectCreationDate { get; set; }

    public long? ProjectModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProjectModifiedDate { get; set; }

    [Column("ProjectBranchID")]
    public int ProjectBranchId { get; set; }

    [Column("ProjectManagerID")]
    public long? ProjectManagerId { get; set; }

    [StringLength(50)]
    public string MaintenanceType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProjectExtraCost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProjectTotalCost { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    public bool ProjectActive { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(250)]
    public string ClientSupportedBy { get; set; }

    [Column("MaintenanceSalesOfferID")]
    public long? MaintenanceSalesOfferId { get; set; }

    [Column("LinkedSalesOfferID")]
    public long? LinkedSalesOfferId { get; set; }

    [StringLength(50)]
    public string SalesMaintenanceOfferType { get; set; }

    public int? SalesPersonBranchId { get; set; }
}
