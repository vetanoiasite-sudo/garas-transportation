using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceTaxIncludedType")]
public partial class PurchasePoinvoiceTaxIncludedType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("PotaxIncludedType")]
    public virtual ICollection<PurchasePoinvoiceTaxIncluded> PurchasePoinvoiceTaxIncludeds { get; set; } = new List<PurchasePoinvoiceTaxIncluded>();
}
