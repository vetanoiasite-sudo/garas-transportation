using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VBompartitionItemsInventoryItem
{
    [Column("BOMPartitionItemId")]
    public long BompartitionItemId { get; set; }

    [Column("BOMPartitionID")]
    public long BompartitionId { get; set; }

    [Column("BOMPartitionName")]
    [StringLength(250)]
    public string BompartitionName { get; set; }

    [Column("BOMPartitionSerial")]
    [StringLength(50)]
    public string BompartitionSerial { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PartitionWorkingHours { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PartitionTotalWorkingHoursCost { get; set; }

    [Column("BOMPartitionsActive")]
    public bool? BompartitionsActive { get; set; }

    [Column("BOMID")]
    public long? Bomid { get; set; }

    [Column("BOMName")]
    [StringLength(250)]
    public string Bomname { get; set; }

    [Column("BOMSerial")]
    [StringLength(50)]
    public string Bomserial { get; set; }

    [Column("BOMProductGroupID")]
    public int? BomproductGroupId { get; set; }

    [Column("BOMProductID")]
    public long? BomproductId { get; set; }

    [Column("BOMDescription")]
    public string Bomdescription { get; set; }

    [Column("BOMNumberOfUsed")]
    public int? BomnumberOfUsed { get; set; }

    [Column("BOMWorkingHours", TypeName = "decimal(18, 2)")]
    public decimal? BomworkingHours { get; set; }

    [Column("BOMTotalWorkingHoursCost", TypeName = "decimal(18, 4)")]
    public decimal? BomtotalWorkingHoursCost { get; set; }

    [Column("BOMActive")]
    public bool? Bomactive { get; set; }

    [Column("OfferID")]
    public long? OfferId { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }

    [Column("MainBOMID")]
    public long? MainBomid { get; set; }

    [Column("StoreCategoryID")]
    public int? StoreCategoryId { get; set; }

    [Column("ItemCategoryID")]
    public int? ItemCategoryId { get; set; }

    [Column("ItemID")]
    public long ItemId { get; set; }

    [StringLength(50)]
    public string InventoryItemCode { get; set; }

    public string InventoryItemName { get; set; }

    public bool? InventoryItemActive { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [Column("_MinBalance")]
    public double? MinBalance { get; set; }

    [Column("_MaxBalance")]
    public double? MaxBalance { get; set; }

    [Column("PurchasingUOMID")]
    public int? PurchasingUomid { get; set; }

    [StringLength(1000)]
    public string MarketName { get; set; }

    public string Details { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("_ExchangeFactor")]
    public double? ExchangeFactor { get; set; }

    [StringLength(1000)]
    public string CommercialName { get; set; }

    public int? CalculationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CustomeUnitPrice { get; set; }

    [Column("MinBalance", TypeName = "decimal(18, 2)")]
    public decimal? MinBalance1 { get; set; }

    [Column("MaxBalance", TypeName = "decimal(18, 2)")]
    public decimal? MaxBalance1 { get; set; }

    [Column("ExchangeFactor", TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? LastUnitPrice { get; set; }

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

    public int ItemOrder { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RequiredQty { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal ItemQtyPrice { get; set; }

    [StringLength(50)]
    public string ItemPriceType { get; set; }

    public bool IsAlternative { get; set; }

    public long? AlternativeItem { get; set; }

    public bool ActiveToUse { get; set; }

    [Column("BOMPartitionItemsDescription")]
    public string BompartitionItemsDescription { get; set; }

    [Column("BOMPartitionItemsActive")]
    public bool BompartitionItemsActive { get; set; }
}
