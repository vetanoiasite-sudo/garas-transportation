using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ProjectInstallationReportUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectInstallationReportID")]
    public long ProjectInstallationReportId { get; set; }

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
    [InverseProperty("ProjectInstallationReportUsers")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationReportUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("ProjectInstallationReportUsers")]
    public virtual Department Department { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("ProjectInstallationReportUsers")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallationReportUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectInstallationReportId")]
    [InverseProperty("ProjectInstallationReportUsers")]
    public virtual ProjectInstallationReport ProjectInstallationReport { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ProjectInstallationReportUserUsers")]
    public virtual User User { get; set; }
}
