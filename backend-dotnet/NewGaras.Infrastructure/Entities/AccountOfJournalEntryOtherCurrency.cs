using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AccountOfJournalEntryOtherCurrency")]
public partial class AccountOfJournalEntryOtherCurrency
{
    [Key]
    public long Id { get; set; }

    public long EntryId { get; set; }

    public long AccountId { get; set; }

    public int CurrencyId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Credit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Debit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string SignOfAccount { get; set; }

    [Column("RateToLocalCU", TypeName = "decimal(8, 4)")]
    public decimal RateToLocalCu { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Accumulative { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AccountOfJournalEntryOtherCurrencies")]
    public virtual Account Account { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AccountOfJournalEntryOtherCurrencyCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("AccountOfJournalEntryOtherCurrencies")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("EntryId")]
    [InverseProperty("AccountOfJournalEntryOtherCurrencies")]
    public virtual DailyJournalEntry Entry { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AccountOfJournalEntryOtherCurrencyModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
