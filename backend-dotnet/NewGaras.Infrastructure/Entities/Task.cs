using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Task")]
public partial class Task
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(500)]
    public string TaskUser { get; set; }

    [Column("TaskTypeID")]
    public int TaskTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public string RefrenceNumber { get; set; }

    [StringLength(500)]
    public string TaskCategory { get; set; }

    public string TaskSubject { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [StringLength(250)]
    public string TaskPage { get; set; }

    public bool GroupReply { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    public long? ProjectSprintId { get; set; }

    public long? ProjectWorkFlowId { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Tasks")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("Tasks")]
    public virtual Project Project { get; set; }

    [ForeignKey("ProjectSprintId")]
    [InverseProperty("Tasks")]
    public virtual ProjectSprint ProjectSprint { get; set; }

    [ForeignKey("ProjectWorkFlowId")]
    [InverseProperty("Tasks")]
    public virtual ProjectWorkFlow ProjectWorkFlow { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<TaskApplicationOpen> TaskApplicationOpens { get; set; } = new List<TaskApplicationOpen>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskBrowserTab> TaskBrowserTabs { get; set; } = new List<TaskBrowserTab>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskExpensi> TaskExpensis { get; set; } = new List<TaskExpensi>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskFlagsOwnerReciever> TaskFlagsOwnerRecievers { get; set; } = new List<TaskFlagsOwnerReciever>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskPermission> TaskPermissions { get; set; } = new List<TaskPermission>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskRequirement> TaskRequirements { get; set; } = new List<TaskRequirement>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskScreenShot> TaskScreenShots { get; set; } = new List<TaskScreenShot>();

    [ForeignKey("TaskTypeId")]
    [InverseProperty("Tasks")]
    public virtual TaskType TaskType { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<TaskUnitRateService> TaskUnitRateServices { get; set; } = new List<TaskUnitRateService>();

    [InverseProperty("Task")]
    public virtual ICollection<TaskUserMonitor> TaskUserMonitors { get; set; } = new List<TaskUserMonitor>();

    [InverseProperty("Task")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();
}
