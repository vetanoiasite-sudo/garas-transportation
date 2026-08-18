using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoiceExtraFeesType")]
public partial class PurchasePoinvoiceExtraFeesType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [InverseProperty("PoinvoiceExtraFeesType")]
    public virtual ICollection<PurchasePoinvoiceExtraFee> PurchasePoinvoiceExtraFees { get; set; } = new List<PurchasePoinvoiceExtraFee>();
}
