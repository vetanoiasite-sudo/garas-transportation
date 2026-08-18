using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallation")]
public partial class ProjectInstallation
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    public int Revision { get; set; }

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

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int Progress { get; set; }

    [Column("PassQC")]
    public bool PassQc { get; set; }

    public int? InsNumber { get; set; }

    [StringLength(250)]
    public string InsOrderSerial { get; set; }

    public bool PartialDeliveryStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PartialDeliveryDate { get; set; }

    public bool PartialInstallationStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PartialInstallationStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PartialInstallationEndDate { get; set; }

    public bool FullDeliveryStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FullDeliveryDate { get; set; }

    public bool FullInstallationStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FullInstallationStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FullInstallationEndDate { get; set; }

    public bool RequireSalesPersonFeedBack { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SalesPersonFeedBackResult { get; set; }

    [Unicode(false)]
    public string SalesPersonFeedBackCooment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesPersonFeedBackReplyDate { get; set; }

    public bool? Closed { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallationModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectInstallations")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectInstallation")]
    public virtual ICollection<ProjectInstallationAttachment> ProjectInstallationAttachments { get; set; } = new List<ProjectInstallationAttachment>();

    [InverseProperty("ProjectInstallation")]
    public virtual ICollection<ProjectInstallationBoq> ProjectInstallationBoqs { get; set; } = new List<ProjectInstallationBoq>();

    [InverseProperty("ProjectInstallation")]
    public virtual ICollection<ProjectInstallationReport> ProjectInstallationReports { get; set; } = new List<ProjectInstallationReport>();

    [InverseProperty("ProjectInstallation")]
    public virtual ICollection<ProjectInstallationVersion> ProjectInstallationVersions { get; set; } = new List<ProjectInstallationVersion>();
}
