using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferInvoiceTax")]
public partial class SalesOfferInvoiceTax
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TaxPercentage { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TaxValue { get; set; }

    [StringLength(100)]
    public string TaxName { get; set; }

    [StringLength(100)]
    public string TaxType { get; set; }

    public long? InvoicePayerClientId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferInvoiceTaxCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InvoicePayerClientId")]
    [InverseProperty("SalesOfferInvoiceTaxes")]
    public virtual Client InvoicePayerClient { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferInvoiceTaxModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SalesOfferId")]
    [InverseProperty("SalesOfferInvoiceTaxes")]
    public virtual SalesOffer SalesOffer { get; set; }
}
