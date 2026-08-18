using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferProduct")]
public partial class SalesOfferProduct
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Column("ProductID")]
    public long? ProductId { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    public double? Quantity { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ItemPrice { get; set; }

    public string ItemPricingComment { get; set; }

    public double? ConfirmReceivingQuantity { get; set; }

    [StringLength(250)]
    public string ConfirmReceivingComment { get; set; }

    public long? InvoicePayerClientId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DiscountPercentage { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalPrice { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TaxPercentage { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TaxValue { get; set; }

    public double? ReturnedQty { get; set; }

    public double? RemainQty { get; set; }

    /// <summary>
    /// Profit Percentage From Average Price
    /// </summary>
    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ProfitPercentage { get; set; }

    public double? ReleasedQty { get; set; }

    [Column("MaintenanceForID")]
    public long? MaintenanceForId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferProductCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("RelatedToSalesOfferProduct")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [InverseProperty("RelatedToSalesOfferProduct")]
    public virtual ICollection<DailyReportLine> DailyReportLines { get; set; } = new List<DailyReportLine>();

    [ForeignKey("InventoryItemId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryItemCategoryId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual InventoryItemCategory InventoryItemCategory { get; set; }

    [ForeignKey("InvoicePayerClientId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual Client InvoicePayerClient { get; set; }

    [ForeignKey("MaintenanceForId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual MaintenanceFor MaintenanceFor { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferProductModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual SalesOffer Offer { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<PricingProduct> PricingProducts { get; set; } = new List<PricingProduct>();

    [ForeignKey("ProductId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual Product Product { get; set; }

    [ForeignKey("ProductGroupId")]
    [InverseProperty("SalesOfferProducts")]
    public virtual ProductGroup ProductGroup { get; set; }

    [InverseProperty("SalesOfferProduct")]
    public virtual ICollection<ProjectFabricationBoq> ProjectFabricationBoqs { get; set; } = new List<ProjectFabricationBoq>();

    [InverseProperty("SalesOfferProduct")]
    public virtual ICollection<ProjectInstallationBoq> ProjectInstallationBoqs { get; set; } = new List<ProjectInstallationBoq>();

    [InverseProperty("SalesOfferProduct")]
    public virtual ICollection<SalesOfferItemAttachment> SalesOfferItemAttachments { get; set; } = new List<SalesOfferItemAttachment>();

    [InverseProperty("SalesOfferProduct")]
    public virtual ICollection<SalesOfferProductTax> SalesOfferProductTaxes { get; set; } = new List<SalesOfferProductTax>();
}
