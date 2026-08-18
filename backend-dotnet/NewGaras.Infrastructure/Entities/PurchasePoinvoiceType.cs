using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceType")]
public partial class PurchasePoinvoiceType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool Active { get; set; }

    public bool IsDraft { get; set; }

    [InverseProperty("PurchasePoinvoiceType")]
    public virtual ICollection<PurchasePoinvoice> PurchasePoinvoices { get; set; } = new List<PurchasePoinvoice>();
}
