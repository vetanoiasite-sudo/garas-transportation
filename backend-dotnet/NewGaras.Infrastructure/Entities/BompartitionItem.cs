using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMPartitionItems")]
public partial class BompartitionItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMPartitionID")]
    public long BompartitionId { get; set; }

    [Column("StoreCategoryID")]
    public int? StoreCategoryId { get; set; }

    [Column("ItemCategoryID")]
    public int? ItemCategoryId { get; set; }

    [Column("ItemID")]
    public long ItemId { get; set; }

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

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BompartitionId")]
    [InverseProperty("BompartitionItems")]
    public virtual Bompartition Bompartition { get; set; }

    [InverseProperty("BompartitionItem")]
    public virtual ICollection<BompartitionItemAttachment> BompartitionItemAttachments { get; set; } = new List<BompartitionItemAttachment>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("BompartitionItemCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("BompartitionItems")]
    public virtual InventoryItem Item { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BompartitionItemModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
