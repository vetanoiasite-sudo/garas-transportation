using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallationReport")]
public partial class ProjectInstallationReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectInstallationID")]
    public long ProjectInstallationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReportDate { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Progress { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RemainTime { get; set; }

    [Column("AskForQC")]
    public bool AskForQc { get; set; }

    [Column("AskForQCDate", TypeName = "datetime")]
    public DateTime? AskForQcdate { get; set; }

    public string FeedBack { get; set; }

    public bool Finished { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(50)]
    public string ReportCategory { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationReportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallationReportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("ProjectInstallationReport")]
    public virtual ICollection<ProjectFinishInstallationAttachment> ProjectFinishInstallationAttachments { get; set; } = new List<ProjectFinishInstallationAttachment>();

    [ForeignKey("ProjectInstallationId")]
    [InverseProperty("ProjectInstallationReports")]
    public virtual ProjectInstallation ProjectInstallation { get; set; }

    [InverseProperty("ProjectInstallationReport")]
    public virtual ICollection<ProjectInstallationReportAttachment> ProjectInstallationReportAttachments { get; set; } = new List<ProjectInstallationReportAttachment>();

    [InverseProperty("ProjectInstallationReport")]
    public virtual ICollection<ProjectInstallationReportClarification> ProjectInstallationReportClarifications { get; set; } = new List<ProjectInstallationReportClarification>();

    [InverseProperty("ProjectInstallationReport")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUsers { get; set; } = new List<ProjectInstallationReportUser>();
}
