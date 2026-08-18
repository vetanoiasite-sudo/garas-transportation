using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PricingExtraCost")]
public partial class PricingExtraCost
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public string Comment { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ExchangeRate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LocalCurrencyPrice { get; set; }

    [Column("LocalCurrencyID")]
    public int LocalCurrencyId { get; set; }

    [Column("PricingID")]
    public long PricingId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PricingExtraCostCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PricingExtraCostCurrencies")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("LocalCurrencyId")]
    [InverseProperty("PricingExtraCostLocalCurrencies")]
    public virtual Currency LocalCurrency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PricingExtraCostModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PricingId")]
    [InverseProperty("PricingExtraCosts")]
    public virtual Pricing Pricing { get; set; }
}
