using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("JobTitle")]
public partial class JobTitle
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

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? HourlyRate { get; set; }

    public int Currency { get; set; }

    public bool? Archive { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("JobTitleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("Currency")]
    [InverseProperty("JobTitles")]
    public virtual Currency CurrencyNavigation { get; set; }

    [InverseProperty("JobTitle")]
    public virtual ICollection<HrUser> HrUsers { get; set; } = new List<HrUser>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<JobInformation> JobInformations { get; set; } = new List<JobInformation>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUsers { get; set; } = new List<MaintenanceReportUser>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("JobTitleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("JobTitle")]
    public virtual ICollection<ProjectFabricationJobTitle> ProjectFabricationJobTitles { get; set; } = new List<ProjectFabricationJobTitle>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUsers { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<ProjectInstallationJobTitle> ProjectInstallationJobTitles { get; set; } = new List<ProjectInstallationJobTitle>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUsers { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("JobTitle")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
