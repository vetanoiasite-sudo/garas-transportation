using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceCalculatedShipmentValue")]
public partial class PurchasePoinvoiceCalculatedShipmentValue
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PurchasePOInvoiceID")]
    public long PurchasePoinvoiceId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoinvoiceCalculatedShipmentValues")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoinvoiceCalculatedShipmentValues")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("PurchasePoinvoiceId")]
    [InverseProperty("PurchasePoinvoiceCalculatedShipmentValues")]
    public virtual PurchasePoinvoice PurchasePoinvoice { get; set; }
}
