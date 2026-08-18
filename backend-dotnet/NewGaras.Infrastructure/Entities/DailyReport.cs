using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyReport")]
public partial class DailyReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReprotDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Required]
    [StringLength(250)]
    public string Status { get; set; }

    public string Note { get; set; }

    public string ReviewComment { get; set; }

    public long? ReviewedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    public bool Reviewed { get; set; }

    public double? Review { get; set; }

    [InverseProperty("DailyReport")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [InverseProperty("DailyReport")]
    public virtual ICollection<DailyReportExpense> DailyReportExpenses { get; set; } = new List<DailyReportExpense>();

    [InverseProperty("DailyReport")]
    public virtual ICollection<DailyReportLine> DailyReportLines { get; set; } = new List<DailyReportLine>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyReportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ReviewedBy")]
    [InverseProperty("DailyReportReviewedByNavigations")]
    public virtual User ReviewedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("DailyReportUsers")]
    public virtual User User { get; set; }
}
