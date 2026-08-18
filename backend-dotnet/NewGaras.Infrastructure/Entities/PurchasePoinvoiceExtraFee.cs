using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceExtraFees")]
public partial class PurchasePoinvoiceExtraFee
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POInvoiceID")]
    public long PoinvoiceId { get; set; }

    [Column("POInvoiceExtraFeesTypeID")]
    public long PoinvoiceExtraFeesTypeId { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Percentage { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Amount { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    [Column("POItemId")]
    public long? PoitemId { get; set; }

    public string Comment { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoinvoiceExtraFeeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoinvoiceExtraFees")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PurchasePoinvoiceExtraFeeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PoinvoiceExtraFeesTypeId")]
    [InverseProperty("PurchasePoinvoiceExtraFees")]
    public virtual PurchasePoinvoiceExtraFeesType PoinvoiceExtraFeesType { get; set; }

    [ForeignKey("PoitemId")]
    [InverseProperty("PurchasePoinvoiceExtraFees")]
    public virtual PurchasePoitem Poitem { get; set; }
}
