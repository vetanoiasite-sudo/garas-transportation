using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class WeekDay
{
    [StringLength(50)]
    public string Day { get; set; }

    public bool? IsWeekEnd { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("WeekDays")]
    public virtual Branch Branch { get; set; }

    [InverseProperty("WeekDay")]
    public virtual ICollection<BranchSchedule> BranchSchedules { get; set; } = new List<BranchSchedule>();

    [InverseProperty("WeekDay")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("DayNavigation")]
    public virtual ICollection<WorkingHour> WorkingHours { get; set; } = new List<WorkingHour>();
}
