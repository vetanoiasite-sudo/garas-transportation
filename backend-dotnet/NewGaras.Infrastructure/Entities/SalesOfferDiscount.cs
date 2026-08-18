using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferDiscount")]
public partial class SalesOfferDiscount
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountPercentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountValue { get; set; }

    public bool? DiscountApproved { get; set; }

    public bool? ClientApproveDiscount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public long? DiscountApprovedBy { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    public long? InvoicePayerClientId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferDiscountCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DiscountApprovedBy")]
    [InverseProperty("SalesOfferDiscountDiscountApprovedByNavigations")]
    public virtual User DiscountApprovedByNavigation { get; set; }

    [ForeignKey("InvoicePayerClientId")]
    [InverseProperty("SalesOfferDiscounts")]
    public virtual Client InvoicePayerClient { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferDiscountModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SalesOfferId")]
    [InverseProperty("SalesOfferDiscounts")]
    public virtual SalesOffer SalesOffer { get; set; }
}
