using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InvoiceItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InvoiceID")]
    public long InvoiceId { get; set; }

    public long? SalesOfferProductId { get; set; }

    public string Comments { get; set; }

    [Column("eInvoiceId")]
    [StringLength(50)]
    public string EInvoiceId { get; set; }

    [Column("eInvoiceStatus")]
    [StringLength(50)]
    public string EInvoiceStatus { get; set; }

    [Column("eInvoiceAcceptDate", TypeName = "datetime")]
    public DateTime? EInvoiceAcceptDate { get; set; }
}
