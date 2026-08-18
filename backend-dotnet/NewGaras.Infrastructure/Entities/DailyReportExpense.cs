using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyReportExpense")]
public partial class DailyReportExpense
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyReportLineID")]
    public long? DailyReportLineId { get; set; }

    [Column("DailyReportID")]
    public long? DailyReportId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public string Type { get; set; }

    public int? CurrencyId { get; set; }

    public string AttachmentPath { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public string Comment { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DailyReportExpenseCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("DailyReportExpenses")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("DailyReportId")]
    [InverseProperty("DailyReportExpenses")]
    public virtual DailyReport DailyReport { get; set; }

    [ForeignKey("DailyReportLineId")]
    [InverseProperty("DailyReportExpenses")]
    public virtual DailyReportLine DailyReportLine { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyReportExpenseModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
