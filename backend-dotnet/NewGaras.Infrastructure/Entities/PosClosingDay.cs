using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PosClosingDay")]
public partial class PosClosingDay
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public int StoreId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal SalesCount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal SalesAmount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal SalesReturnCount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal SalesReturnAmount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal NetSalesCount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal NetSalesAmount { get; set; }

    public string Notes { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ClosingDayAmount { get; set; }

    public long? JournalEntryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PosClosingDayCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("JournalEntryId")]
    [InverseProperty("PosClosingDays")]
    public virtual DailyJournalEntry JournalEntry { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PosClosingDayModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("PosClosingDays")]
    public virtual InventoryStore Store { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PosClosingDayUsers")]
    public virtual User User { get; set; }
}
