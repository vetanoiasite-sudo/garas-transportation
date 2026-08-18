using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Team")]
public partial class Team
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    public bool Active { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [InverseProperty("Team")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("TeamCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("DoctorSpeciality")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("Team")]
    public virtual ICollection<HrUser> HrUsers { get; set; } = new List<HrUser>();

    [InverseProperty("Team")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TeamModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Team")]
    public virtual ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

    [InverseProperty("Team")]
    public virtual ICollection<WorkshopStation> WorkshopStations { get; set; } = new List<WorkshopStation>();
}
