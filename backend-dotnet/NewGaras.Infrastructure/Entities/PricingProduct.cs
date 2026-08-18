using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PricingProduct")]
public partial class PricingProduct
{
    [Key]
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

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PricingProductCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("PricingProducts")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryItemCategoryId")]
    [InverseProperty("PricingProducts")]
    public virtual InventoryItemCategory InventoryItemCategory { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PricingProductModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PricingId")]
    [InverseProperty("PricingProducts")]
    public virtual Pricing Pricing { get; set; }

    [InverseProperty("PricingProduct")]
    public virtual ICollection<PricingBom> PricingBoms { get; set; } = new List<PricingBom>();

    [InverseProperty("PricingProduct")]
    public virtual ICollection<PricingProductAttachment> PricingProductAttachments { get; set; } = new List<PricingProductAttachment>();

    [ForeignKey("ProductId")]
    [InverseProperty("PricingProducts")]
    public virtual SalesOfferProduct Product { get; set; }

    [ForeignKey("ProductGroupId")]
    [InverseProperty("PricingProducts")]
    public virtual ProductGroup ProductGroup { get; set; }
}
