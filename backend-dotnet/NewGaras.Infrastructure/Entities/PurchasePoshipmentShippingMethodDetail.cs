using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOShipmentShippingMethodDetails")]
public partial class PurchasePoshipmentShippingMethodDetail
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ShippingMethodID")]
    public long ShippingMethodId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Fees { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoshipmentShippingMethodDetails")]
    public virtual Currency Currency { get; set; }

    [InverseProperty("ShipmentShippingMethodDetails")]
    public virtual ICollection<PuchasePoshipment> PuchasePoshipments { get; set; } = new List<PuchasePoshipment>();

    [InverseProperty("PurchasePoshipmentShippingMethodDetails")]
    public virtual ICollection<ShippingCompany> ShippingCompanies { get; set; } = new List<ShippingCompany>();

    [ForeignKey("ShippingMethodId")]
    [InverseProperty("PurchasePoshipmentShippingMethodDetails")]
    public virtual ShippingMethod ShippingMethod { get; set; }
}
