using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Payroll")]
public partial class Payroll
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("HrUserID")]
    public long HrUserId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    public DateOnly Date { get; set; }

    public int TotalWorkingDays { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TotalWorkingHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal JobTitleRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TaskRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TotalOvertimeHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal OvertimeCost { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TotalDelayHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal DeductionCost { get; set; }

    public int TotalAbsenceDays { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal AbsenceDeductionCost { get; set; }

    public int VacationDaysNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Taxs { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Insurance { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Allowances { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal OtherDeductions { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Commission { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal OtherIncome { get; set; }

    public DateOnly? From { get; set; }

    public DateOnly? To { get; set; }

    public bool? IsPaid { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? HolidayHours { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? VacationHours { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Payrolls")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PayrollCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("Payrolls")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PayrollModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
