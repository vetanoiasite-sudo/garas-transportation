using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOAmountPaymentMethod")]
public partial class PurchasePoamountPaymentMethod
{
    [Column("POID")]
    public long Poid { get; set; }

    [Column("PurchasePaymentMethodID")]
    public long PurchasePaymentMethodId { get; set; }

    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public bool? RequiredDownPayment { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchasePoamountPaymentMethods")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("PurchasePaymentMethodId")]
    [InverseProperty("PurchasePoamountPaymentMethods")]
    public virtual PurchasePaymentMethod PurchasePaymentMethod { get; set; }
}
