using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectInstallationProjectOffer
{
    [Column("ID")]
    public long? Id { get; set; }

    public int? Revision { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    [StringLength(1000)]
    public string Consultant { get; set; }

    public string ClientQualityInspection { get; set; }

    public string SaftyReglation { get; set; }

    public string GeneralNote { get; set; }

    public bool? RequireFinFeedBack { get; set; }

    [StringLength(50)]
    public string FinFeedBackResult { get; set; }

    public string FinFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinFeedBackReplyDate { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? Progress { get; set; }

    [Column("PassQC")]
    public bool? PassQc { get; set; }

    public int? InsNumber { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column("ProjectManagerID")]
    public long? ProjectManagerId { get; set; }

    [StringLength(50)]
    public string ProjectManagerFirstName { get; set; }

    [StringLength(250)]
    public string ProjectManagerEmail { get; set; }

    public byte[] ProjectManagerPhoto { get; set; }

    [StringLength(50)]
    public string ProjectManagerLastName { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [StringLength(50)]
    public string SalesPersonFirstName { get; set; }

    [StringLength(250)]
    public string SalesPersonEmail { get; set; }

    [StringLength(20)]
    public string SalesPersonMobile { get; set; }

    public byte[] SalesPersonPhoto { get; set; }

    [StringLength(50)]
    public string SalesPersonLastName { get; set; }

    [Column("SalesPersonBranchID")]
    public int? SalesPersonBranchId { get; set; }

    [StringLength(500)]
    public string SalesPersonBranchName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    public bool? RequireSalesPersonFeedBack { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SalesPersonFeedBackResult { get; set; }

    [Unicode(false)]
    public string SalesPersonFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesPersonFeedBackReplyDate { get; set; }

    public bool ProjectActivate { get; set; }

    public bool? PartialDeliveryStatus { get; set; }

    public bool? PartialInstallationStatus { get; set; }

    public bool? FullDeliveryStatus { get; set; }

    public bool? FullInstallationStatus { get; set; }

    [StringLength(250)]
    public string InsOrderName { get; set; }

    [StringLength(250)]
    public string InsOrderSerial { get; set; }

    public bool? InstallationClosed { get; set; }
}
