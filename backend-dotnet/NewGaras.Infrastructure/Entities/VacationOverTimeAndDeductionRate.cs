using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class VacationOverTimeAndDeductionRate
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public TimeOnly From { get; set; }

    public TimeOnly To { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Rate { get; set; }

    [Column("VacationDayID")]
    public long VacationDayId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("VacationOverTimeAndDeductionRateCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("VacationOverTimeAndDeductionRateModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("VacationDayId")]
    [InverseProperty("VacationOverTimeAndDeductionRates")]
    public virtual VacationDay VacationDay { get; set; }
}
