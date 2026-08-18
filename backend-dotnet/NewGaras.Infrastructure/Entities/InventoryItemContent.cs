using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryItemContent")]
public partial class InventoryItemContent
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Required]
    [StringLength(250)]
    public string ChapterName { get; set; }

    [StringLength(250)]
    public string ChapterNumber { get; set; }

    [Column("ParentContentID")]
    public long? ParentContentId { get; set; }

    public int? DataLevel { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    public bool? Haveitem { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("prepared_search_name")]
    [StringLength(250)]
    public string PreparedSearchName { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryItemContentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryItemContents")]
    public virtual InventoryItem InventoryItem { get; set; }

    [InverseProperty("ParentContent")]
    public virtual ICollection<InventoryItemContent> InverseParentContent { get; set; } = new List<InventoryItemContent>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryItemContentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ParentContentId")]
    [InverseProperty("InverseParentContent")]
    public virtual InventoryItemContent ParentContent { get; set; }
}
