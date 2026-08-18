using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRUserWarning")]
public partial class HruserWarning
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("CreatedByUserID")]
    public long CreatedByUserId { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime WarningDate { get; set; }

    public string SignedMemoAttachmentPath { get; set; }

    [StringLength(50)]
    public string SignedMemoFileName { get; set; }

    [StringLength(50)]
    public string SignedMemoFileExtension { get; set; }

    public string CorrectiveActionAttachment { get; set; }

    [StringLength(50)]
    public string CorrectiveActionFileName { get; set; }

    [StringLength(50)]
    public string CorrectiveActionFileExtension { get; set; }

    public string CorrectiveActionDescription { get; set; }

    public string ActionPlanAttachment { get; set; }

    [StringLength(50)]
    public string ActionPlanFileName { get; set; }

    [StringLength(50)]
    public string ActionPlanFileExtension { get; set; }

    public string ActionPlanDescription { get; set; }

    [Column("ActionPlanApprovalID")]
    public int? ActionPlanApprovalId { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ClosingEvaluation { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("ActionPlanApprovalId")]
    [InverseProperty("HruserWarnings")]
    public virtual HruserWarningActionPlanApproval ActionPlanApproval { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("HruserWarnings")]
    public virtual HruserWarningStatus Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("HruserWarnings")]
    public virtual User User { get; set; }
}
