using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HrUser")]
public partial class HrUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ARFirstName")]
    [StringLength(50)]
    public string ArfirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedById { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Modified { get; set; }

    [Required]
    [Column("ARLastName")]
    [StringLength(50)]
    public string ArlastName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Column("ARMiddleName")]
    [StringLength(50)]
    public string ArmiddleName { get; set; }

    [Required]
    [StringLength(50)]
    public string MiddleName { get; set; }

    public long CreatedById { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("DepartmentID")]
    public int? DepartmentId { get; set; }

    [Column("JobTitleID")]
    public int? JobTitleId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(20)]
    public string LandLine { get; set; }

    public long? NationalityId { get; set; }

    public int? MaritalStatusId { get; set; }

    public int? MilitaryStatusId { get; set; }

    public long? TeamId { get; set; }

    public bool IsUser { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Mobile { get; set; }

    [Required]
    [StringLength(50)]
    public string Email { get; set; }

    public string ImgPath { get; set; }

    [StringLength(10)]
    public string Gender { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longtitud { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [InverseProperty("HrUser")]
    public virtual ICollection<BankDetail> BankDetails { get; set; } = new List<BankDetail>();

    [ForeignKey("BranchId")]
    [InverseProperty("HrUsers")]
    public virtual Branch Branch { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    [InverseProperty("HrUser")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployees { get; set; } = new List<ContractLeaveEmployee>();

    [ForeignKey("CreatedById")]
    [InverseProperty("HrUserCreatedBies")]
    public virtual User CreatedBy { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("HrUsers")]
    public virtual Department Department { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("HrUser")]
    public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    [ForeignKey("JobTitleId")]
    [InverseProperty("HrUsers")]
    public virtual JobTitle JobTitle { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    [ForeignKey("MaritalStatusId")]
    [InverseProperty("HrUsers")]
    public virtual MaritalStatus MaritalStatus { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<MedicalExaminationOffer> MedicalExaminationOffers { get; set; } = new List<MedicalExaminationOffer>();

    [InverseProperty("Doctor")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();

    [ForeignKey("MilitaryStatusId")]
    [InverseProperty("HrUsers")]
    public virtual MilitaryStatus MilitaryStatus { get; set; }

    [ForeignKey("ModifiedById")]
    [InverseProperty("HrUserModifiedBies")]
    public virtual User ModifiedBy { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    [InverseProperty("HrUser")]
    public virtual ICollection<ProjectProgressUser> ProjectProgressUsers { get; set; } = new List<ProjectProgressUser>();

    [InverseProperty("HrUser")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    [ForeignKey("TeamId")]
    [InverseProperty("HrUsers")]
    public virtual Team Team { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<TransportationVehicleRouteEmployeeException> TransportationVehicleRouteEmployeeExceptions { get; set; } = new List<TransportationVehicleRouteEmployeeException>();

    [InverseProperty("HrUser")]
    public virtual ICollection<TransportationVehicleRouteEmployee> TransportationVehicleRouteEmployees { get; set; } = new List<TransportationVehicleRouteEmployee>();

    [ForeignKey("UserId")]
    [InverseProperty("HrUserUsers")]
    public virtual User User { get; set; }

    [InverseProperty("HrUser")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    [InverseProperty("HrUser")]
    public virtual ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

    [InverseProperty("HrUser")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();
}
