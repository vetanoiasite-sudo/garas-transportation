using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryItem")]
public partial class InventoryItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    [Column("InventoryItemCategoryID")]
    public int InventoryItemCategoryId { get; set; }

    [Column("_MinBalance")]
    public double? MinBalance { get; set; }

    [Column("_MaxBalance")]
    public double? MaxBalance { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("PurchasingUOMID")]
    public int PurchasingUomid { get; set; }

    [StringLength(1000)]
    public string MarketName { get; set; }

    public string Details { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column("RequstionUOMID")]
    public int RequstionUomid { get; set; }

    [Column("_ExchangeFactor")]
    public double? ExchangeFactor { get; set; }

    [StringLength(1000)]
    public string CommercialName { get; set; }

    public int CalculationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal CustomeUnitPrice { get; set; }

    public byte[] Image { get; set; }

    public bool? HasImage { get; set; }

    [Column("MinBalance", TypeName = "decimal(18, 2)")]
    public decimal? MinBalance1 { get; set; }

    [Column("MaxBalance", TypeName = "decimal(18, 2)")]
    public decimal? MaxBalance1 { get; set; }

    [Column("ExchangeFactor", TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal LastUnitPrice { get; set; }

    [Column("PartNO")]
    [StringLength(500)]
    public string PartNo { get; set; }

    [StringLength(250)]
    public string CostName1 { get; set; }

    [StringLength(250)]
    public string CostName2 { get; set; }

    [StringLength(250)]
    public string CostName3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CostAmount3 { get; set; }

    public long? ItemSerialCounter { get; set; }

    [Column("PriorityID")]
    public int? PriorityId { get; set; }

    [Column("prepared_search_name")]
    public string PreparedSearchName { get; set; }

    public string ImageUrl { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<BompartitionItem> BompartitionItems { get; set; } = new List<BompartitionItem>();

    [InverseProperty("Product")]
    public virtual ICollection<Bomproduct> Bomproducts { get; set; } = new List<Bomproduct>();

    [InverseProperty("Product")]
    public virtual ICollection<Bom> Boms { get; set; } = new List<Bom>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryItemCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("RelatedToInventoryItem")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [InverseProperty("RelatedToInventoryItem")]
    public virtual ICollection<DailyReportLine> DailyReportLines { get; set; } = new List<DailyReportLine>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<Hrcustody> Hrcustodies { get; set; } = new List<Hrcustody>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryAddingOrderItem> InventoryAddingOrderItems { get; set; } = new List<InventoryAddingOrderItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryInternalBackOrderItem> InventoryInternalBackOrderItems { get; set; } = new List<InventoryInternalBackOrderItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryInternalTransferOrderItem> InventoryInternalTransferOrderItems { get; set; } = new List<InventoryInternalTransferOrderItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryItemAttachment> InventoryItemAttachments { get; set; } = new List<InventoryItemAttachment>();

    [ForeignKey("InventoryItemCategoryId")]
    [InverseProperty("InventoryItems")]
    public virtual InventoryItemCategory InventoryItemCategory { get; set; }

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryItemContent> InventoryItemContents { get; set; } = new List<InventoryItemContent>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryItemPrice> InventoryItemPrices { get; set; } = new List<InventoryItemPrice>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryItemUom> InventoryItemUoms { get; set; } = new List<InventoryItemUom>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; } = new List<InventoryMatrialRequestItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryReportItem> InventoryReportItems { get; set; } = new List<InventoryReportItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<InventoryStoreItem> InventoryStoreItems { get; set; } = new List<InventoryStoreItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<MaintenanceFor> MaintenanceFors { get; set; } = new List<MaintenanceFor>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryItemModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("InventoryItem")]
    public virtual ICollection<PricingProduct> PricingProducts { get; set; } = new List<PricingProduct>();

    [ForeignKey("PriorityId")]
    [InverseProperty("InventoryItems")]
    public virtual Priority Priority { get; set; }

    [InverseProperty("InventoryItem")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [ForeignKey("PurchasingUomid")]
    [InverseProperty("InventoryItemPurchasingUoms")]
    public virtual InventoryUom PurchasingUom { get; set; }

    [ForeignKey("RequstionUomid")]
    [InverseProperty("InventoryItemRequstionUoms")]
    public virtual InventoryUom RequstionUom { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<SalesBranchProductTarget> SalesBranchProductTargets { get; set; } = new List<SalesBranchProductTarget>();

    [InverseProperty("Product")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargets { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<SalesOfferItemAttachment> SalesOfferItemAttachments { get; set; } = new List<SalesOfferItemAttachment>();

    [InverseProperty("InventoryItem")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProducts { get; set; } = new List<SalesOfferProduct>();
}
