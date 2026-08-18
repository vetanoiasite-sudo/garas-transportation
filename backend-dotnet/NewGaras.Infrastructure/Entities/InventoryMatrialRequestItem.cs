using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InventoryMatrialRequestItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialRequestID")]
    public long InventoryMatrialRequestId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column("_ReqQuantity")]
    public double? ReqQuantity { get; set; }

    [Column("_RecivedQuantity")]
    public double? RecivedQuantity { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("FabricationOrderID")]
    public long? FabricationOrderId { get; set; }

    public string Comments { get; set; }

    [Column("_PurchaseQuantity")]
    public double? PurchaseQuantity { get; set; }

    [Column("ReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity1 { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [Column("PurchaseQuantity", TypeName = "decimal(18, 4)")]
    public decimal? PurchaseQuantity1 { get; set; }

    [Column("FromBOM")]
    public bool? FromBom { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }

    public bool? IsHold { get; set; }

    [Column("FabricationOrderItemID")]
    public long? FabricationOrderItemId { get; set; }

    [ForeignKey("FabricationOrderId")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual ProjectFabrication FabricationOrder { get; set; }

    [ForeignKey("FabricationOrderItemId")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual ProjectFabricationBoq FabricationOrderItem { get; set; }

    [InverseProperty("MaterialRequestItem")]
    public virtual ICollection<Hrcustody> Hrcustodies { get; set; } = new List<Hrcustody>();

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [InverseProperty("InventoryMatrialRequestItem")]
    public virtual ICollection<InventoryMatrialReleaseItem> InventoryMatrialReleaseItems { get; set; } = new List<InventoryMatrialReleaseItem>();

    [ForeignKey("InventoryMatrialRequestId")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual InventoryMatrialRequest InventoryMatrialRequest { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual Project Project { get; set; }

    [InverseProperty("Mritem")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("InventoryMatrialRequestItem")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [InverseProperty("InventoryMatrialRequestItem")]
    public virtual ICollection<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new List<PurchaseRequestItem>();

    [ForeignKey("Uomid")]
    [InverseProperty("InventoryMatrialRequestItems")]
    public virtual InventoryUom Uom { get; set; }
}
