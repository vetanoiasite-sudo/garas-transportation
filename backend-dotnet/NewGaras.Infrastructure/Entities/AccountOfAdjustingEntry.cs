using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AccountOfAdjustingEntry")]
public partial class AccountOfAdjustingEntry
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Credit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Debit { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

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
    [InverseProperty("AccountOfAdjustingEntries")]
    public virtual Account Account { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AccountOfAdjustingEntryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AccountOfAdjustingEntryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
