using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectFabricationProjectOfferEntity
{
    [Column("ID")]
    public long? Id { get; set; }

    public int? Revision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

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

    public bool? RequireCivilFeedBack { get; set; }

    [StringLength(50)]
    public string CivilFeedBackResult { get; set; }

    public string CivilFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CivilFeedBackReplyDate { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? Progress { get; set; }

    [Column("PassQC")]
    public bool? PassQc { get; set; }

    public int? FabNumber { get; set; }

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

    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    public bool? FinallyStatus { get; set; }

    public bool? RequireApprovalFeedBack { get; set; }

    [StringLength(50)]
    public string ApprovalFeedBackResult { get; set; }

    public string ApprovalFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovalFeedBackReplyDate { get; set; }

    public bool? RequireTechDrowingFeedBack { get; set; }

    [StringLength(50)]
    public string TechDrowingFeedBackResult { get; set; }

    public string TechDrowingFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TechDrowingFeedBackReplyDate { get; set; }

    public bool? CivilRequestStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CivilRequestDate { get; set; }

    public bool? RequireSalesPersonFeedBack { get; set; }

    [StringLength(50)]
    public string SalesPersonFeedBackResult { get; set; }

    public string SalesPersonFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesPersonFeedBackReplyDate { get; set; }

    public bool ProjectActivate { get; set; }
}
