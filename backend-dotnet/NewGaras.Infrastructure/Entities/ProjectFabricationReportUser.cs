using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ProjectFabricationReportUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectFabricationReportID")]
    public long ProjectFabricationReportId { get; set; }

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
    [InverseProperty("ProjectFabricationReportUsers")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationReportUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("ProjectFabricationReportUsers")]
    public virtual Department Department { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("ProjectFabricationReportUsers")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectFabricationReportUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectFabricationReportId")]
    [InverseProperty("ProjectFabricationReportUsers")]
    public virtual ProjectFabricationReport ProjectFabricationReport { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ProjectFabricationReportUserUsers")]
    public virtual User User { get; set; }
}
