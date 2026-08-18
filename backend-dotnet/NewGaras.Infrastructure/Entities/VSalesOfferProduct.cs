using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesOfferProduct
{
    [Column("ID")]
    public long? Id { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool? Active { get; set; }

    [Column("ProductID")]
    public long? ProductId { get; set; }

    [Column("OfferID")]
    public long? OfferId { get; set; }

    [StringLength(1000)]
    public string ProductName { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    [StringLength(1000)]
    public string ProductGroupName { get; set; }

    public double? Quantity { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [Required]
    public string InventoryItemName { get; set; }

    [StringLength(1000)]
    public string CategoryName { get; set; }

    public string Description { get; set; }

    [Column("PurchasingUOMID")]
    public int PurchasingUomid { get; set; }

    [Column("PurchasingUOMSHortName")]
    [StringLength(50)]
    public string PurchasingUomshortName { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ItemPrice { get; set; }

    public double? ConfirmReceivingQuantity { get; set; }

    [StringLength(250)]
    public string ConfirmReceivingComment { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal CustomeUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal LastUnitPrice { get; set; }

    public int CalculationType { get; set; }

    [StringLength(250)]
    public string ItemPricingComment { get; set; }
}
