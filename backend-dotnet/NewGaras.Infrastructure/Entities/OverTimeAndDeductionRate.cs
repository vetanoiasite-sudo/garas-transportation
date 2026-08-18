using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("OverTimeAndDeductionRate")]
public partial class OverTimeAndDeductionRate
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public TimeOnly From { get; set; }

    public TimeOnly To { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Rate { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("OverTimeAndDeductionRates")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("OverTimeAndDeductionRateCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("OverTimeAndDeductionRateModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
