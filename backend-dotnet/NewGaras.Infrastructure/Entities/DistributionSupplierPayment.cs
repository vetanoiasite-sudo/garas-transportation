using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DistributionSupplierPayment")]
public partial class DistributionSupplierPayment
{
    [Key]
    public int Id { get; set; }

    public int SupplierPaymentId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Payment { get; set; }

    public int MonthNum { get; set; }

    public int YearNum { get; set; }

    public long SupplierId { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("DistributionSupplierPayments")]
    public virtual Supplier Supplier { get; set; }

    [ForeignKey("SupplierPaymentId")]
    [InverseProperty("DistributionSupplierPayments")]
    public virtual TransportionLineSupplierPayment SupplierPayment { get; set; }
}
