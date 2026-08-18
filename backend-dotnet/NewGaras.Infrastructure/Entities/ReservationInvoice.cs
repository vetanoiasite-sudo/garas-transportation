using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ReservationInvoice")]
public partial class ReservationInvoice
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InvoiceDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    public long CreateBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Serial { get; set; }

    public bool IsClosed { get; set; }

    public long ClientId { get; set; }

    public int ReservationId { get; set; }

    public int InvoiceTypeId { get; set; }

    public int CurrencyId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ReservationInvoices")]
    public virtual Client Client { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("ReservationInvoices")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("InvoiceTypeId")]
    [InverseProperty("ReservationInvoices")]
    public virtual InvoiceType InvoiceType { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("ReservationInvoices")]
    public virtual Reservation Reservation { get; set; }
}
