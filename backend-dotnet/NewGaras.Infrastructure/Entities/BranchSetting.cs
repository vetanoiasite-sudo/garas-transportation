using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class BranchSetting
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public bool AllowOverTimeInWeekends { get; set; }

    public bool AllowDelayingDeduction { get; set; }

    public bool AllowAutomaticOvertime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    public bool Active { get; set; }

    public int? PayrollFrom { get; set; }

    public int? PayrollTo { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("BranchSettings")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BranchSettingCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BranchSettingModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
