using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    public bool? Archived { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("BranchId")]
    [InverseProperty("Departments")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DepartmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<HrUser> HrUsers { get; set; } = new List<HrUser>();

    [InverseProperty("Department")]
    public virtual ICollection<JobInformation> JobInformations { get; set; } = new List<JobInformation>();

    [InverseProperty("Department")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUsers { get; set; } = new List<MaintenanceReportUser>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DepartmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUsers { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("Department")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUsers { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("Department")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
