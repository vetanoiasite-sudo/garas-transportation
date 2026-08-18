using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AccountOfJournalEntry")]
[Index("EntryDate", Name = "NonClusteredIndex-20220217-102246")]
[Index("AccountId", Name = "NonClusteredIndex-20220217-102415")]
[Index("Active", Name = "NonClusteredIndex-20220217-102928")]
public partial class AccountOfJournalEntry
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("EntryID")]
    public long EntryId { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Required]
    [StringLength(50)]
    public string FromOrTo { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Credit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Debit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string SignOfAccount { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    [Column("MethodID")]
    public long? MethodId { get; set; }

    [Column("DTMainType")]
    [StringLength(50)]
    public string DtmainType { get; set; }

    [Column("ExpOrIncTypeID")]
    public long? ExpOrIncTypeId { get; set; }

    [StringLength(250)]
    public string ExpOrIncTypeName { get; set; }

    [Column("ExtraIDOfType")]
    public long? ExtraIdofType { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AccBalance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AccountOfJournalEntries")]
    public virtual Account Account { get; set; }

    [InverseProperty("AccountOfJe")]
    public virtual ICollection<ClientAccount> ClientAccounts { get; set; } = new List<ClientAccount>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AccountOfJournalEntryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("EntryId")]
    [InverseProperty("AccountOfJournalEntries")]
    public virtual DailyJournalEntry Entry { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AccountOfJournalEntryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("AccountOfJe")]
    public virtual ICollection<SupplierAccount> SupplierAccounts { get; set; } = new List<SupplierAccount>();
}
