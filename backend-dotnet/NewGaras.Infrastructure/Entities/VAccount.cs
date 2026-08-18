using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VAccount
{
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

    [StringLength(250)]
    public string AccountCategoryName { get; set; }

    [StringLength(250)]
    public string CurrencyName { get; set; }

    [Column("AdvanciedTypeID")]
    public long? AdvanciedTypeId { get; set; }

    [StringLength(250)]
    public string AdvanciedTypeName { get; set; }
}
