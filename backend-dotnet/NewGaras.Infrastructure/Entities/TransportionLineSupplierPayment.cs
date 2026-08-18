using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportionLineSupplierPayment")]
public partial class TransportionLineSupplierPayment
{
    [Key]
    public int Id { get; set; }

    public long SupplierId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Payment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DatePayment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Required]
    [StringLength(50)]
    public string TypeOfDebt { get; set; }

    public int? NumberOfMonths { get; set; }

    [InverseProperty("SupplierPayment")]
    public virtual ICollection<DistributionSupplierPayment> DistributionSupplierPayments { get; set; } = new List<DistributionSupplierPayment>();

    [ForeignKey("SupplierId")]
    [InverseProperty("TransportionLineSupplierPayments")]
    public virtual Supplier Supplier { get; set; }
}
