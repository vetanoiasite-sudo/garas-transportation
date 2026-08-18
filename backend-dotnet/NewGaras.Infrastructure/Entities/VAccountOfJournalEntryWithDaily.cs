using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VAccountOfJournalEntryWithDaily
{
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

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AccBalance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool? Closed { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? TotalAmount { get; set; }

    [StringLength(50)]
    public string DocumentNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DailyEntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DailyCreationDate { get; set; }

    public long DailyCreatedBy { get; set; }

    [StringLength(250)]
    public string AccountName { get; set; }

    [StringLength(250)]
    public string AccountNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountTypeName { get; set; }

    [Column("AccountCurrencyID")]
    public int? AccountCurrencyId { get; set; }

    [Column("AccountCategoryID")]
    public long AccountCategoryId { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    public bool AccountActive { get; set; }

    [StringLength(250)]
    public string AccountCategoryName { get; set; }

    [Required]
    [StringLength(250)]
    public string AccountCurrencyName { get; set; }

    public bool? DailyJournalEntryApproval { get; set; }

    public bool AdvanciedSettingsStatus { get; set; }

    [StringLength(50)]
    public string MethodName { get; set; }

    [StringLength(50)]
    public string DailyCreatedFirstName { get; set; }

    [StringLength(50)]
    public string DailyCreatedLastName { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("AdvanciedTypeID")]
    public long? AdvanciedTypeId { get; set; }

    [StringLength(250)]
    public string AdvanciedTypeName { get; set; }
}
