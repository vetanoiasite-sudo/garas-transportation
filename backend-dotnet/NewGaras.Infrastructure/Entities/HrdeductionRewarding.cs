using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRDeductionRewarding")]
public partial class HrdeductionRewarding
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("CreatedByUserID")]
    public long CreatedByUserId { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    public int? Days { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Hours { get; set; }

    public int? Months { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    public bool IsDone { get; set; }

    public bool IsDeduction { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("HrdeductionRewardingCreatedByUsers")]
    public virtual User CreatedByUser { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("HrdeductionRewardingUsers")]
    public virtual User User { get; set; }
}
