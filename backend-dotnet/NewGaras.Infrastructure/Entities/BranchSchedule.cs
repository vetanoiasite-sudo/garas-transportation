using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BranchSchedule")]
public partial class BranchSchedule
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public TimeOnly? From { get; set; }

    public TimeOnly? To { get; set; }

    public int ShiftNumber { get; set; }

    [Column("WeekDayID")]
    public int? WeekDayId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("BranchSchedules")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BranchScheduleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BranchScheduleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("BranchSchedule")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRoutes { get; set; } = new List<TransportationVehicleRoute>();

    [ForeignKey("WeekDayId")]
    [InverseProperty("BranchSchedules")]
    public virtual WeekDay WeekDay { get; set; }

    [InverseProperty("Shift")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();
}
