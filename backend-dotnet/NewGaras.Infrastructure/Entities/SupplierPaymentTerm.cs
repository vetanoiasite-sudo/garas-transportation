using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SupplierPaymentTerm")]
public partial class SupplierPaymentTerm
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long SupplierId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Percentage { get; set; }

    public string Time { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierPaymentTerms")]
    public virtual Supplier Supplier { get; set; }
}
