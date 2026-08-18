using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class MaintenanceReportUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MaintenanceReportID")]
    public long MaintenanceReportId { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [Column("JobTitleID")]
    public int JobTitleId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HourNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeTo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Evaluation { get; set; }

    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("MaintenanceReportUsers")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MaintenanceReportUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("MaintenanceReportUsers")]
    public virtual Department Department { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("MaintenanceReportUsers")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("MaintenanceReportId")]
    [InverseProperty("MaintenanceReportUsers")]
    public virtual MaintenanceReport MaintenanceReport { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MaintenanceReportUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MaintenanceReportUserUsers")]
    public virtual User User { get; set; }
}
