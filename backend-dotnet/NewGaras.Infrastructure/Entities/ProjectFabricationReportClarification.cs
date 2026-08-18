using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectFabricationReportClarification")]
public partial class ProjectFabricationReportClarification
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectFabricationReportID")]
    public long ProjectFabricationReportId { get; set; }

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
    [InverseProperty("ProjectFabricationReportClarificationClarificationUsers")]
    public virtual User ClarificationUser { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationReportClarificationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifedBy")]
    [InverseProperty("ProjectFabricationReportClarificationModifedByNavigations")]
    public virtual User ModifedByNavigation { get; set; }

    [ForeignKey("ProjectFabricationReportId")]
    [InverseProperty("ProjectFabricationReportClarifications")]
    public virtual ProjectFabricationReport ProjectFabricationReport { get; set; }

    [InverseProperty("ProjectFabricationReportClarification")]
    public virtual ICollection<ProjectFabricationReportClarificationAttachment> ProjectFabricationReportClarificationAttachments { get; set; } = new List<ProjectFabricationReportClarificationAttachment>();
}
