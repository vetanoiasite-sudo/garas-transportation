using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryReportItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("InventoryReportID")]
    public long InventoryReportId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal CurrentBalance { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PhysicalBalance { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryReportItemCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryReportItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryReportId")]
    [InverseProperty("InventoryReportItems")]
    public virtual InventoryReport InventoryReport { get; set; }

    [InverseProperty("InvReportItem")]
    public virtual ICollection<InventoryReportItemParent> InventoryReportItemParents { get; set; } = new List<InventoryReportItemParent>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryReportItemModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
