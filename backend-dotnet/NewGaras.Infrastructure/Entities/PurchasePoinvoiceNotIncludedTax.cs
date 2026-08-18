using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceNotIncludedTax")]
public partial class PurchasePoinvoiceNotIncludedTax
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POInvoiceID")]
    public long PoinvoiceId { get; set; }

    [Column("PONotIncludedTaxTypeID")]
    public long PonotIncludedTaxTypeId { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Percentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoinvoiceNotIncludedTaxCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoinvoiceNotIncludedTaxes")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PurchasePoinvoiceNotIncludedTaxModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PonotIncludedTaxTypeId")]
    [InverseProperty("PurchasePoinvoiceNotIncludedTaxes")]
    public virtual PurchasePoinvoiceNotIncludedTaxType PonotIncludedTaxType { get; set; }
}
