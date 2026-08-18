using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ContractDetail
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    [Column("ContactTypeID")]
    public int ContactTypeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    public int? ProbationPeriod { get; set; }

    public int? NoticedByEmployee { get; set; }

    public int? NoticedByCompany { get; set; }

    public bool IsCurrent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    public bool IsAllowOverTime { get; set; }

    [Column("ISAutomatic")]
    public bool Isautomatic { get; set; }

    public bool? FreeWorkingHours { get; set; }

    public bool? AllowBreakDeduction { get; set; }

    public bool? AllowLocationTracking { get; set; }

    public double? Diameter { get; set; }

    public bool? AllowComissions { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ComissionRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ComissionPercentage { get; set; }

    public bool? AllowOvertime { get; set; }

    public bool? AutomaticPerWorkingHours { get; set; }

    public bool? AllowedWithApproval { get; set; }

    [Column("HrUserID")]
    public long? HrUserId { get; set; }

    [Column("ReportToID")]
    public long? ReportToId { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? WorkingHours { get; set; }

    [ForeignKey("ContactTypeId")]
    [InverseProperty("ContractDetails")]
    public virtual ContractType ContactType { get; set; }

    [InverseProperty("Contract")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployees { get; set; } = new List<ContractLeaveEmployee>();

    [InverseProperty("Contract")]
    public virtual ICollection<ContractReportTo> ContractReportTos { get; set; } = new List<ContractReportTo>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("ContractDetailCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("ContractDetails")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ContractDetailModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ReportToId")]
    [InverseProperty("ContractDetailReportTos")]
    public virtual User ReportTo { get; set; }

    [InverseProperty("Contract")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    [ForeignKey("UserId")]
    [InverseProperty("ContractDetailUsers")]
    public virtual User User { get; set; }
}
