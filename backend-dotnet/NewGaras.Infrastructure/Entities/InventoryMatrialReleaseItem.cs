using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryMatrialReleaseItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialReleasetID")]
    public long InventoryMatrialReleasetId { get; set; }

    [Column("_RecivedQuantity")]
    public double? RecivedQuantity { get; set; }

    public string Comments { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [InverseProperty("InventoryMatrialReleaseItem")]
    public virtual ICollection<InventoryInternalBackOrderItem> InventoryInternalBackOrderItems { get; set; } = new List<InventoryInternalBackOrderItem>();

    [ForeignKey("InventoryMatrialReleasetId")]
    [InverseProperty("InventoryMatrialReleaseItems")]
    public virtual InventoryMatrialRelease InventoryMatrialReleaset { get; set; }

    [ForeignKey("InventoryMatrialRequestItemId")]
    [InverseProperty("InventoryMatrialReleaseItems")]
    public virtual InventoryMatrialRequestItem InventoryMatrialRequestItem { get; set; }
}
