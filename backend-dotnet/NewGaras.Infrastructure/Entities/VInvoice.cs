using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInvoice
{
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(50)]
    public string Serial { get; set; }

    public int Revision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InvoiceDate { get; set; }

    [StringLength(100)]
    public string InvoiceType { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    public bool Active { get; set; }

    public bool IsClosed { get; set; }

    [StringLength(50)]
    public string CreationType { get; set; }

    [StringLength(500)]
    public string InvoiceFor { get; set; }

    [Column("eInvoiceId")]
    [StringLength(50)]
    public string EInvoiceId { get; set; }

    [Column("eInvoiceStatus")]
    [StringLength(50)]
    public string EInvoiceStatus { get; set; }

    [Column("eInvoiceAcceptDate", TypeName = "datetime")]
    public DateTime? EInvoiceAcceptDate { get; set; }

    public long? SalesOfferId { get; set; }

    [Column("eInvoiceJsonBody")]
    public string EInvoiceJsonBody { get; set; }

    [Column("eInvoiceRequestToSend")]
    public bool? EInvoiceRequestToSend { get; set; }

    [StringLength(500)]
    public string PayerClientName { get; set; }

    public long? OfferClientId { get; set; }

    [StringLength(500)]
    public string OfferClientName { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }
}
