using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectFabricationReport")]
public partial class ProjectFabricationReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectFabricationID")]
    public long ProjectFabricationId { get; set; }

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

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationReportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectFabricationReportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectFabricationId")]
    [InverseProperty("ProjectFabricationReports")]
    public virtual ProjectFabrication ProjectFabrication { get; set; }

    [InverseProperty("ProjectFabricationReport")]
    public virtual ICollection<ProjectFabricationReportAttachment> ProjectFabricationReportAttachments { get; set; } = new List<ProjectFabricationReportAttachment>();

    [InverseProperty("ProjectFabricationReport")]
    public virtual ICollection<ProjectFabricationReportClarification> ProjectFabricationReportClarifications { get; set; } = new List<ProjectFabricationReportClarification>();

    [InverseProperty("ProjectFabricationReport")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUsers { get; set; } = new List<ProjectFabricationReportUser>();
}
