using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class SalesOfferExtraCost
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column("ExtraCostTypeID")]
    public long ExtraCostTypeId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public long? InvoicePayerClientId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferExtraCostCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ExtraCostTypeId")]
    [InverseProperty("SalesOfferExtraCosts")]
    public virtual SalesExtraCostType ExtraCostType { get; set; }

    [ForeignKey("InvoicePayerClientId")]
    [InverseProperty("SalesOfferExtraCosts")]
    public virtual Client InvoicePayerClient { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferExtraCostModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SalesOfferId")]
    [InverseProperty("SalesOfferExtraCosts")]
    public virtual SalesOffer SalesOffer { get; set; }
}
