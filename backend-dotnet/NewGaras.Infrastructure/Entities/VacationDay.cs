using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VacationDay")]
public partial class VacationDay
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string ArName { get; set; }

    [Required]
    [StringLength(250)]
    public string EngName { get; set; }

    [StringLength(250)]
    public string OtherName { get; set; }

    [StringLength(250)]
    public string Reason { get; set; }

    public bool IsPaid { get; set; }

    public bool IsDeducted { get; set; }

    public bool AllowOverTime { get; set; }

    public bool AllowDelayingDeduction { get; set; }

    public bool AllowAutomaticCalculations { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime From { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime To { get; set; }

    public int BranchId { get; set; }

    public bool Active { get; set; }

    public bool? Archived { get; set; }

    public bool? IsWhApplied { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("VacationDays")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("VacationDayCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("VacationDayModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("VacationDay")]
    public virtual ICollection<VacationOverTimeAndDeductionRate> VacationOverTimeAndDeductionRates { get; set; } = new List<VacationOverTimeAndDeductionRate>();
}
