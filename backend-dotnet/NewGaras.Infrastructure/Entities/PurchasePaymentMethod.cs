using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePaymentMethod")]
public partial class PurchasePaymentMethod
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [InverseProperty("PurchasePaymentMethod")]
    public virtual ICollection<PurchasePoamountPaymentMethod> PurchasePoamountPaymentMethods { get; set; } = new List<PurchasePoamountPaymentMethod>();

    [InverseProperty("PurchasePaymentMethod")]
    public virtual ICollection<PurchasePoinvoiceClosedPayment> PurchasePoinvoiceClosedPayments { get; set; } = new List<PurchasePoinvoiceClosedPayment>();
}
