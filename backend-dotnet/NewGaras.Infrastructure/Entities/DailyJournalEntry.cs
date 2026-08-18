using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyJournalEntry")]
public partial class DailyJournalEntry
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool Closed { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string DocumentNumber { get; set; }

    public bool Approval { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [InverseProperty("Entry")]
    public virtual ICollection<AccountOfJournalEntry> AccountOfJournalEntries { get; set; } = new List<AccountOfJournalEntry>();

    [InverseProperty("Entry")]
    public virtual ICollection<AccountOfJournalEntryOtherCurrency> AccountOfJournalEntryOtherCurrencies { get; set; } = new List<AccountOfJournalEntryOtherCurrency>();

    [ForeignKey("BranchId")]
    [InverseProperty("DailyJournalEntries")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DailyJournalEntryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Djentry")]
    public virtual ICollection<DailyJournalEntryReverse> DailyJournalEntryReverseDjentries { get; set; } = new List<DailyJournalEntryReverse>();

    [InverseProperty("ParentDjentry")]
    public virtual ICollection<DailyJournalEntryReverse> DailyJournalEntryReverseParentDjentries { get; set; } = new List<DailyJournalEntryReverse>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyJournalEntryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("JournalEntry")]
    public virtual ICollection<PosClosingDay> PosClosingDays { get; set; } = new List<PosClosingDay>();

    [InverseProperty("DailyJournalEntry")]
    public virtual ICollection<ProjectPaymentJournalEntry> ProjectPaymentJournalEntries { get; set; } = new List<ProjectPaymentJournalEntry>();
}
