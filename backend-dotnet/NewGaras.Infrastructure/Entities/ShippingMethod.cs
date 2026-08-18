using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ShippingMethod")]
public partial class ShippingMethod
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("ShippingMethod")]
    public virtual ICollection<PurchasePoshipmentShippingMethodDetail> PurchasePoshipmentShippingMethodDetails { get; set; } = new List<PurchasePoshipmentShippingMethodDetail>();
}
