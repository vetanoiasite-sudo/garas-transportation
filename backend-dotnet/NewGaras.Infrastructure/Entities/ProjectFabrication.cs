using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectFabrication")]
public partial class ProjectFabrication
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    public int Revision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    [StringLength(1000)]
    public string Consultant { get; set; }

    public string ClientQualityInspection { get; set; }

    public string SaftyReglation { get; set; }

    public string GeneralNote { get; set; }

    public bool RequireFinFeedBack { get; set; }

    [StringLength(50)]
    public string FinFeedBackResult { get; set; }

    public string FinFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinFeedBackReplyDate { get; set; }

    public bool RequireCivilFeedBack { get; set; }

    [StringLength(50)]
    public string CivilFeedBackResult { get; set; }

    public string CivilFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CivilFeedBackReplyDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int Progress { get; set; }

    [Column("PassQC")]
    public bool PassQc { get; set; }

    public int? FabNumber { get; set; }

    [StringLength(250)]
    public string FabOrderSerial { get; set; }

    public bool IntiallyStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? IntiallyDate { get; set; }

    [StringLength(250)]
    public string IntiallyType { get; set; }

    public bool PartialStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PartialDate { get; set; }

    public bool FinallyStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinallyDate { get; set; }

    public bool ModificationStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public bool ExtraModificationStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExtraModificationDate { get; set; }

    public bool RequireApprovalFeedBack { get; set; }

    [StringLength(50)]
    public string ApprovalFeedBackResult { get; set; }

    public string ApprovalFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovalFeedBackReplyDate { get; set; }

    public bool RequireSalesPersonFeedBack { get; set; }

    [StringLength(50)]
    public string SalesPersonFeedBackResult { get; set; }

    public string SalesPersonFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesPersonFeedBackReplyDate { get; set; }

    public bool RequireTechDrowingFeedBack { get; set; }

    [StringLength(50)]
    public string TechDrowingFeedBackResult { get; set; }

    public string TechDrowingFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TechDrowingFeedBackReplyDate { get; set; }

    public bool CivilRequestStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CivilRequestDate { get; set; }

    [Column("TechnicalID")]
    public long? TechnicalId { get; set; }

    public bool FabUserStatus { get; set; }

    public bool RequirePricingFeedBack { get; set; }

    [StringLength(50)]
    public string PricingFeedBackResult { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? PricingFeedBackCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PricingFeedBackReplyDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("FabricationOrder")]
    public virtual ICollection<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; } = new List<InventoryMatrialRequestItem>();

    [InverseProperty("FabricationOrder")]
    public virtual ICollection<InvoiceExtraModification> InvoiceExtraModifications { get; set; } = new List<InvoiceExtraModification>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectFabricationModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectFabrications")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectFabrication")]
    public virtual ICollection<ProjectFabricationAttachment> ProjectFabricationAttachments { get; set; } = new List<ProjectFabricationAttachment>();

    [InverseProperty("ProjectFabrication")]
    public virtual ICollection<ProjectFabricationBoq> ProjectFabricationBoqs { get; set; } = new List<ProjectFabricationBoq>();

    [InverseProperty("ProjectFabrication")]
    public virtual ICollection<ProjectFabricationOrderUser> ProjectFabricationOrderUsers { get; set; } = new List<ProjectFabricationOrderUser>();

    [InverseProperty("ProjectFabrication")]
    public virtual ICollection<ProjectFabricationReport> ProjectFabricationReports { get; set; } = new List<ProjectFabricationReport>();

    [InverseProperty("ProjectFabrication")]
    public virtual ICollection<ProjectFabricationVersion> ProjectFabricationVersions { get; set; } = new List<ProjectFabricationVersion>();

    [InverseProperty("FabricationOrder")]
    public virtual ICollection<ProjectFabricationWorkshopStationHistory> ProjectFabricationWorkshopStationHistories { get; set; } = new List<ProjectFabricationWorkshopStationHistory>();

    [InverseProperty("FabricationOrder")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();
}
