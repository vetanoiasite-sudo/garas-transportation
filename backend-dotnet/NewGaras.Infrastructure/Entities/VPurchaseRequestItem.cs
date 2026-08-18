using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchaseRequestItem
{
    [Column("InventoryMatrialRequestID")]
    public long? InventoryMatrialRequestId { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [StringLength(50)]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    [Column("UOMID")]
    public int? Uomid { get; set; }

    [Column("UOMShortName")]
    [StringLength(50)]
    public string UomshortName { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("FabricationOrderID")]
    public long? FabricationOrderId { get; set; }

    public string Comments { get; set; }

    public int? FabNumber { get; set; }

    [Column("ID")]
    public long Id { get; set; }

    [Column("PurchaseRequestID")]
    public long PurchaseRequestId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? Quantity { get; set; }

    public string Expr1 { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchasedQuantity { get; set; }

    public string PurchaseRequestNotes { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CustomeUnitPrice { get; set; }

    public byte[] Image { get; set; }

    public bool? HasImage { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RemainQuantity { get; set; }

    public long? AssignedTo { get; set; }

    [Column("StatusOfPR")]
    [StringLength(50)]
    public string StatusOfPr { get; set; }

    [Column("ApprovalStatusOfPR")]
    [StringLength(50)]
    public string ApprovalStatusOfPr { get; set; }

    [Column("PurchasingUOMID")]
    public int? PurchasingUomid { get; set; }

    [Column("PurchasingUOMName")]
    [StringLength(50)]
    public string PurchasingUomname { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("RequstionUOMName")]
    [StringLength(50)]
    public string RequstionUomname { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }
}
