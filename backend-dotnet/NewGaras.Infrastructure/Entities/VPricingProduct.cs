using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPricingProduct
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("PricingID")]
    public long PricingId { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    [Column("ProductID")]
    public long? ProductId { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    public double Quantity { get; set; }

    [Column("PricingBOMID")]
    public long? PricingBomid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SalesManagerAddPrice { get; set; }

    public double? SalesManagerAddPricePercentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PricingManagerAddPrice { get; set; }

    public double? PricingManagerAddPricePercentage { get; set; }

    public string SalesManagerComment { get; set; }

    public string PricingManagerComment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(1000)]
    public string ProductName { get; set; }

    public string ProductDescription { get; set; }

    public bool? ProductActive { get; set; }
}
