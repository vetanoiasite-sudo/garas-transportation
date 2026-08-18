using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MaintenanceReportClarification")]
public partial class MaintenanceReportClarification
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MaintenanceReportID")]
    public long MaintenanceReportId { get; set; }

    [Required]
    [StringLength(500)]
    public string ClarificationType { get; set; }

    [Required]
    public string Clarification { get; set; }

    [Column("ClarificationUserID")]
    public long ClarificationUserId { get; set; }

    public string FeedBack { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("ClarificationUserId")]
    [InverseProperty("MaintenanceReportClarificationClarificationUsers")]
    public virtual User ClarificationUser { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MaintenanceReportClarificationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("MaintenanceReportId")]
    [InverseProperty("MaintenanceReportClarifications")]
    public virtual MaintenanceReport MaintenanceReport { get; set; }

    [InverseProperty("MaintenanceReportClarification")]
    public virtual ICollection<MaintenanceReportClarificationAttachment> MaintenanceReportClarificationAttachments { get; set; } = new List<MaintenanceReportClarificationAttachment>();

    [ForeignKey("ModifedBy")]
    [InverseProperty("MaintenanceReportClarificationModifedByNavigations")]
    public virtual User ModifedByNavigation { get; set; }
}
