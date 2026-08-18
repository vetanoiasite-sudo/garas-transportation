using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InvoiceType")]
public partial class InvoiceType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; }

    [InverseProperty("InvoiceType")]
    public virtual ICollection<ReservationInvoice> ReservationInvoices { get; set; } = new List<ReservationInvoice>();
}
