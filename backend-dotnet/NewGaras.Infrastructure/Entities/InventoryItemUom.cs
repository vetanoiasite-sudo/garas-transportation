using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryItemUOM")]
public partial class InventoryItemUom
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("InventoryUOMID")]
    public int InventoryUomid { get; set; }

    public bool IsPrimary { get; set; }

    public bool CanEdit { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryItemUomCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryItemUoms")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryUomid")]
    [InverseProperty("InventoryItemUoms")]
    public virtual InventoryUom InventoryUom { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryItemUomModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
