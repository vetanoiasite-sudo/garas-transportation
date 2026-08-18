using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MaintenanceReport")]
public partial class MaintenanceReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MaintVisitID")]
    public long MaintVisitId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReportDate { get; set; }

    [StringLength(100)]
    public string ClientAddress { get; set; }

    public string DefectDescription { get; set; }

    public string WorkDescription { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CollectedAmount { get; set; }

    [StringLength(50)]
    public string ByUser { get; set; }

    [Column("ByUserID")]
    public long? ByUserId { get; set; }

    [Column("ClientPRStatus")]
    public bool ClientPrstatus { get; set; }

    [Column("InstallationPRStatus")]
    public bool InstallationPrstatus { get; set; }

    [Column("FabricationPRStatus")]
    public bool FabricationPrstatus { get; set; }

    [Column("DesignPRStatus")]
    public bool DesignPrstatus { get; set; }

    [Column("ProductLifePRStatus")]
    public bool ProductLifePrstatus { get; set; }

    [Column("MaintenanceTeamPRStatus")]
    public bool MaintenanceTeamPrstatus { get; set; }

    public string InternalPartComments { get; set; }

    public bool Finished { get; set; }

    [Column("CRMFeedbackStatus")]
    public bool CrmfeedbackStatus { get; set; }

    [Column("CRMFeedback")]
    [StringLength(50)]
    public string Crmfeedback { get; set; }

    [Column("CRMCommitment")]
    [StringLength(50)]
    public string Crmcommitment { get; set; }

    [Column("CRMFeedbackComments")]
    public string CrmfeedbackComments { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public string ClientCommentsAndFeedback { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? ClientSatisfactionRate { get; set; }

    [StringLength(100)]
    public string ClientSignature { get; set; }

    public string ProblemReportImage { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MaintenanceReportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("MaintVisitId")]
    [InverseProperty("MaintenanceReports")]
    public virtual VisitsScheduleOfMaintenance MaintVisit { get; set; }

    [InverseProperty("MaintenanceReport")]
    public virtual ICollection<MaintenanceReportAttachment> MaintenanceReportAttachments { get; set; } = new List<MaintenanceReportAttachment>();

    [InverseProperty("MaintenanceReport")]
    public virtual ICollection<MaintenanceReportClarification> MaintenanceReportClarifications { get; set; } = new List<MaintenanceReportClarification>();

    [InverseProperty("MaintenanceReport")]
    public virtual ICollection<MaintenanceReportExpense> MaintenanceReportExpenses { get; set; } = new List<MaintenanceReportExpense>();

    [InverseProperty("MaintenanceReport")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUsers { get; set; } = new List<MaintenanceReportUser>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MaintenanceReportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
