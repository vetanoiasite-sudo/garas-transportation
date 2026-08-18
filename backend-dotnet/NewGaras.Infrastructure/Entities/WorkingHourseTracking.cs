using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("WorkingHourseTracking")]
public partial class WorkingHourseTracking
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("HrUserID")]
    public long HrUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("DayTypeID")]
    public int DayTypeId { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("ShiftID")]
    public long? ShiftId { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column("TaskID")]
    public long? TaskId { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TotalHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal WorkingHourRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal JobTitleRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TaskRate { get; set; }

    [StringLength(250)]
    public string ProgressNote { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ProgressRate { get; set; }

    public bool? WorkingHoursApproval { get; set; }

    public long? ApprovedBy { get; set; }

    public bool OverTimeAllowed { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? OvertimeRate { get; set; }

    public bool DelayingAllowed { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? DeductionRate { get; set; }

    [Column("InvoiceID")]
    public long? InvoiceId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? OverTimeHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? DelayingHours { get; set; }

    public long? ProjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TaskValidateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CheckOutDate { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("WorkingHourseTrackingApprovedByNavigations")]
    public virtual User ApprovedByNavigation { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("WorkingHourseTrackingCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DayTypeId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual DayType DayType { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual Invoice Invoice { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual Project Project { get; set; }

    [ForeignKey("ShiftId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual BranchSchedule Shift { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("WorkingHourseTrackings")]
    public virtual Task Task { get; set; }

    [InverseProperty("WorkingHourTracking")]
    public virtual ICollection<TaskRequirement> TaskRequirements { get; set; } = new List<TaskRequirement>();
}
