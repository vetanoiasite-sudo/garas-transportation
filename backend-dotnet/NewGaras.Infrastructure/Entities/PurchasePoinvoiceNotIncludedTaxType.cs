using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceNotIncludedTaxType")]
public partial class PurchasePoinvoiceNotIncludedTaxType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("PonotIncludedTaxType")]
    public virtual ICollection<PurchasePoinvoiceNotIncludedTax> PurchasePoinvoiceNotIncludedTaxes { get; set; } = new List<PurchasePoinvoiceNotIncludedTax>();
}
