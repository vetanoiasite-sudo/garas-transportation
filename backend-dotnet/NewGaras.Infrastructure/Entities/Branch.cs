using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Branch")]
public partial class Branch
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    [StringLength(1000)]
    public string Address { get; set; }

    [StringLength(50)]
    public string Telephone { get; set; }

    [StringLength(50)]
    public string Fax { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    [Column("GovernorateID")]
    public int GovernorateId { get; set; }

    public bool? IsMain { get; set; }

    public int? Building { get; set; }

    public int? Floor { get; set; }

    [StringLength(50)]
    public string Mobile { get; set; }

    public long? AreaId { get; set; }

    public bool? Archive { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Diameter { get; set; }

    [ForeignKey("AreaId")]
    [InverseProperty("Branches")]
    public virtual Area Area { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [InverseProperty("Branch")]
    public virtual ICollection<BranchProduct> BranchProducts { get; set; } = new List<BranchProduct>();

    [InverseProperty("Branch")]
    public virtual ICollection<BranchSchedule> BranchSchedules { get; set; } = new List<BranchSchedule>();

    [InverseProperty("Branch")]
    public virtual ICollection<BranchSetting> BranchSettings { get; set; } = new List<BranchSetting>();

    [InverseProperty("Branch")]
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    [ForeignKey("CountryId")]
    [InverseProperty("Branches")]
    public virtual Country Country { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BranchCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [InverseProperty("Branch")]
    public virtual ICollection<DailyJournalEntry> DailyJournalEntries { get; set; } = new List<DailyJournalEntry>();

    [InverseProperty("Branch")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [InverseProperty("Branch")]
    public virtual ICollection<DoctorRoom> DoctorRooms { get; set; } = new List<DoctorRoom>();

    [InverseProperty("Branch")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    [ForeignKey("GovernorateId")]
    [InverseProperty("Branches")]
    public virtual Governorate Governorate { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<HrUser> HrUsers { get; set; } = new List<HrUser>();

    [InverseProperty("Branch")]
    public virtual ICollection<JobInformation> JobInformations { get; set; } = new List<JobInformation>();

    [InverseProperty("Branch")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUsers { get; set; } = new List<MaintenanceReportUser>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BranchModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<OverTimeAndDeductionRate> OverTimeAndDeductionRates { get; set; } = new List<OverTimeAndDeductionRate>();

    [InverseProperty("Branch")]
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    [InverseProperty("Branch")]
    public virtual ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();

    [InverseProperty("Branch")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUsers { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("Branch")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUsers { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("Branch")]
    public virtual ICollection<ProjectTm> ProjectTms { get; set; } = new List<ProjectTm>();

    [InverseProperty("Branch")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalaryTax> SalaryTaxes { get; set; } = new List<SalaryTax>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesBranchProductTarget> SalesBranchProductTargets { get; set; } = new List<SalesBranchProductTarget>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesBranchTarget> SalesBranchTargets { get; set; } = new List<SalesBranchTarget>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargets { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesBranchUserTarget> SalesBranchUserTargets { get; set; } = new List<SalesBranchUserTarget>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesOffer> SalesOffers { get; set; } = new List<SalesOffer>();

    [InverseProperty("Branch")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [InverseProperty("Branch")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [InverseProperty("Branch")]
    public virtual ICollection<VacationDay> VacationDays { get; set; } = new List<VacationDay>();

    [InverseProperty("Branch")]
    public virtual ICollection<WeekDay> WeekDays { get; set; } = new List<WeekDay>();

    [InverseProperty("Branch")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();

    [InverseProperty("Branch")]
    public virtual ICollection<WorkshopStation> WorkshopStations { get; set; } = new List<WorkshopStation>();
}
