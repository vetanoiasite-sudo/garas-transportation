using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceDeductionType")]
public partial class PurchasePoinvoiceDeductionType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("PodeductionType")]
    public virtual ICollection<PurchasePoinvoiceDeduction> PurchasePoinvoiceDeductions { get; set; } = new List<PurchasePoinvoiceDeduction>();
}
