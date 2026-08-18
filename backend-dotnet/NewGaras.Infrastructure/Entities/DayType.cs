using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DayType")]
public partial class DayType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("DayType")]
    [StringLength(50)]
    public string DayType1 { get; set; }

    [InverseProperty("DayType")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [InverseProperty("DayType")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();
}
