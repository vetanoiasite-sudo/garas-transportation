using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("OffDay")]
public partial class OffDay
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Day { get; set; }

    [Required]
    [StringLength(250)]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [StringLength(250)]
    public string ModifiedBy { get; set; }

    public bool Active { get; set; }

    public bool IsWeekEnd { get; set; }

    [Column("HolidayID")]
    public int? HolidayId { get; set; }

    [Required]
    [StringLength(250)]
    public string WeekEndDay { get; set; }

    public bool? AllowWorking { get; set; }

    [Column("VacationPaymentStrategyID")]
    public int? VacationPaymentStrategyId { get; set; }

    [ForeignKey("HolidayId")]
    [InverseProperty("OffDays")]
    public virtual Holiday Holiday { get; set; }

    [ForeignKey("VacationPaymentStrategyId")]
    [InverseProperty("OffDays")]
    public virtual VacationPaymentStrategy VacationPaymentStrategy { get; set; }
}
