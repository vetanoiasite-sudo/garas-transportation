using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskInfo")]
public partial class TaskInfo
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public string Requirement { get; set; }

    [Column("TaskCategoryID")]
    public long TaskCategoryId { get; set; }

    [Column("TaskPrimarySubCategoryID")]
    public long? TaskPrimarySubCategoryId { get; set; }

    public long? TaskSecondarySubCategory { get; set; }

    public int? Revision { get; set; }

    public bool Status { get; set; }

    [Column("PriorityID")]
    public int PriorityId { get; set; }

    [Column("ManageStageID")]
    public long? ManageStageId { get; set; }

    public bool NoPermision { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(50)]
    public string EstimateDate { get; set; }

    public bool Billable { get; set; }

    public bool TimeTracking { get; set; }

    [Column("EApprove")]
    public bool Eapprove { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PercentQuality { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PercentTime { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PercentPerformance { get; set; }

    [StringLength(1000)]
    public string SatisfactionComment { get; set; }

    public bool? SatisfactionStatus { get; set; }

    public bool TaskClouserFlag { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClosureDate { get; set; }

    [StringLength(50)]
    public string TotalTime { get; set; }

    public string ClosureDescription { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskInfoCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ManageStageId")]
    [InverseProperty("TaskInfos")]
    public virtual ManageStage ManageStage { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskInfoModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("TaskInfo")]
    public virtual ICollection<TaskAssignUser> TaskAssignUsers { get; set; } = new List<TaskAssignUser>();

    [InverseProperty("TaskInfo")]
    public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();

    [ForeignKey("TaskCategoryId")]
    [InverseProperty("TaskInfos")]
    public virtual TaskCategory TaskCategory { get; set; }

    [InverseProperty("TaskInfo")]
    public virtual ICollection<TaskClosureLog> TaskClosureLogs { get; set; } = new List<TaskClosureLog>();

    [InverseProperty("TaskInfo")]
    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    [InverseProperty("TaskInfo")]
    public virtual ICollection<TaskStageHistory> TaskStageHistories { get; set; } = new List<TaskStageHistory>();

    [InverseProperty("TaskInfo")]
    public virtual ICollection<UserTimer> UserTimers { get; set; } = new List<UserTimer>();
}
