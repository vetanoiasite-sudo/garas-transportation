using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class WorkingHour
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public int Day { get; set; }

    public int IntervalFromHour { get; set; }

    public int IntervalToHour { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [StringLength(250)]
    public string ModifiedBy { get; set; }

    public int IntervalFromMin { get; set; }

    public int IntervalToMin { get; set; }

    public int? IntervalMin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [ForeignKey("Day")]
    [InverseProperty("WorkingHours")]
    public virtual WeekDay DayNavigation { get; set; }
}
