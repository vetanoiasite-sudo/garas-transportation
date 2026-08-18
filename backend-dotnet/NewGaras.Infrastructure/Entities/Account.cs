using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Account
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string AccountName { get; set; }

    [StringLength(250)]
    public string AccountNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountTypeName { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    public long? ParentCategory { get; set; }

    public int DataLevel { get; set; }

    public int AccountOrder { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    public bool Haveitem { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Accumulative { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Credit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Debit { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    public bool Havetax { get; set; }

    [Column("TaxID")]
    public long? TaxId { get; set; }

    [StringLength(250)]
    public string TaxName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TaxPercentage { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("AccountCategoryID")]
    public long AccountCategoryId { get; set; }

    public bool AdvanciedSettingsStatus { get; set; }

    public bool? TranactionStatus { get; set; }

    [ForeignKey("AccountCategoryId")]
    [InverseProperty("Accounts")]
    public virtual AccountCategory AccountCategory { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<AccountOfAdjustingEntry> AccountOfAdjustingEntries { get; set; } = new List<AccountOfAdjustingEntry>();

    [InverseProperty("Account")]
    public virtual ICollection<AccountOfJournalEntry> AccountOfJournalEntries { get; set; } = new List<AccountOfJournalEntry>();

    [InverseProperty("Account")]
    public virtual ICollection<AccountOfJournalEntryOtherCurrency> AccountOfJournalEntryOtherCurrencies { get; set; } = new List<AccountOfJournalEntryOtherCurrency>();

    [InverseProperty("Account")]
    public virtual ICollection<AdvanciedSettingAccount> AdvanciedSettingAccounts { get; set; } = new List<AdvanciedSettingAccount>();

    [InverseProperty("Account")]
    public virtual ICollection<ClientAccount> ClientAccounts { get; set; } = new List<ClientAccount>();

    [InverseProperty("Account")]
    public virtual ICollection<ConfirmedRecieveAndRelease> ConfirmedRecieveAndReleases { get; set; } = new List<ConfirmedRecieveAndRelease>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AccountCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("Accounts")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AccountModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<SupplierAccount> SupplierAccounts { get; set; } = new List<SupplierAccount>();

    [ForeignKey("TaxId")]
    [InverseProperty("Accounts")]
    public virtual Tax Tax { get; set; }
}
