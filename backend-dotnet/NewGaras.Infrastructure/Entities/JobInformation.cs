using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("JobInformation")]
public partial class JobInformation
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("JobTitleID")]
    public int JobTitleId { get; set; }

    [StringLength(1000)]
    public string JobDescriptionFile { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [Column("ReportToID")]
    public long ReportToId { get; set; }

    [Required]
    [StringLength(50)]
    public string UserCompanyEmail { get; set; }

    [StringLength(50)]
    public string UserCompanyLandline { get; set; }

    [StringLength(50)]
    public string UserCompanyMobile { get; set; }

    [StringLength(50)]
    public string UserCompanyFax { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("JobInformations")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("JobInformationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("JobInformations")]
    public virtual Department Department { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("JobInformations")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("JobInformationModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ReportToId")]
    [InverseProperty("JobInformationReportTos")]
    public virtual User ReportTo { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("JobInformationUsers")]
    public virtual User User { get; set; }
}
