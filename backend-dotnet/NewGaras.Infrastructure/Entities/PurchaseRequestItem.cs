using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class PurchaseRequestItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PurchaseRequestID")]
    public long PurchaseRequestId { get; set; }

    [Column("_Quantity")]
    public double? Quantity { get; set; }

    public string Comments { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("_PurchasedQuantity")]
    public double? PurchasedQuantity { get; set; }

    public string PurchaseRequestNotes { get; set; }

    [Column("_RemainQuantity")]
    public double? RemainQuantity { get; set; }

    public long? AssignedTo { get; set; }

    [Column("Quantity", TypeName = "decimal(18, 4)")]
    public decimal? Quantity1 { get; set; }

    [Column("PurchasedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? PurchasedQuantity1 { get; set; }

    [Column("RemainQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RemainQuantity1 { get; set; }

    [ForeignKey("InventoryMatrialRequestItemId")]
    [InverseProperty("PurchaseRequestItems")]
    public virtual InventoryMatrialRequestItem InventoryMatrialRequestItem { get; set; }

    [InverseProperty("Pr")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("PurchaseRequestItem")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [ForeignKey("PurchaseRequestId")]
    [InverseProperty("PurchaseRequestItems")]
    public virtual PurchaseRequest PurchaseRequest { get; set; }
}
