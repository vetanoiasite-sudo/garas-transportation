using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ShippingCompany")]
public partial class ShippingCompany
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PurchasePOShipmentShippingMethodDetailsID")]
    public long PurchasePoshipmentShippingMethodDetailsId { get; set; }

    [StringLength(50)]
    public string CompanyName { get; set; }

    [StringLength(50)]
    public string ContactPerson { get; set; }

    [StringLength(50)]
    public string PaymentMethod { get; set; }

    [StringLength(50)]
    public string Contacts { get; set; }

    [Column("AttachementID")]
    public long? AttachementId { get; set; }

    public bool Active { get; set; }

    [ForeignKey("AttachementId")]
    [InverseProperty("ShippingCompanies")]
    public virtual ShippingCompanyAttachment Attachement { get; set; }

    [ForeignKey("PurchasePoshipmentShippingMethodDetailsId")]
    [InverseProperty("ShippingCompanies")]
    public virtual PurchasePoshipmentShippingMethodDetail PurchasePoshipmentShippingMethodDetails { get; set; }
}
