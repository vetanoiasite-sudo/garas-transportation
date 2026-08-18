using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Attendance")]
public partial class Attendance
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("EmployeeID")]
    public long? EmployeeId { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [Column("TeamID")]
    public long? TeamId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public int? CheckInHour { get; set; }

    public int? CheckInMin { get; set; }

    public int? CheckOutHour { get; set; }

    public int? CheckOutMin { get; set; }

    public int? NoHours { get; set; }

    public int? NoMin { get; set; }

    public int? DelayHours { get; set; }

    public int? DelayMin { get; set; }

    public int? OverTimeHour { get; set; }

    public int? OverTimeMin { get; set; }

    [Column("AbsenceTypeID")]
    public int? AbsenceTypeId { get; set; }

    public bool? IsApprovedAbsence { get; set; }

    [Column("ApprovedByUserID")]
    public long? ApprovedByUserId { get; set; }

    public long CreatedBy { get; set; }

    public DateOnly CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    public DateOnly ModificationDate { get; set; }

    public bool Active { get; set; }

    [Column("HrUserID")]
    public long? HrUserId { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longitude { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? OvertimeCost { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? DeductionCost { get; set; }

    [Column("DayTypeID")]
    public int? DayTypeId { get; set; }

    public bool? IsApprovedOvertime { get; set; }

    [StringLength(500)]
    public string AbsenceCause { get; set; }

    [StringLength(500)]
    public string AbsenceRejectCause { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? TaskHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ShiftHours { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CheckOutDate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? HolidayHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? VacationHours { get; set; }

    [ForeignKey("AbsenceTypeId")]
    [InverseProperty("Attendances")]
    public virtual ContractLeaveSetting AbsenceType { get; set; }

    [ForeignKey("ApprovedByUserId")]
    [InverseProperty("AttendanceApprovedByUsers")]
    public virtual User ApprovedByUser { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Attendances")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AttendanceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DayTypeId")]
    [InverseProperty("Attendances")]
    public virtual DayType DayType { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Attendances")]
    public virtual Department Department { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("AttendanceEmployees")]
    public virtual User Employee { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("Attendances")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AttendanceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("Attendances")]
    public virtual Team Team { get; set; }
}
