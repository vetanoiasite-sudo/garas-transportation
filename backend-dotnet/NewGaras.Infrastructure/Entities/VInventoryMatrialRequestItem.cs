using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialRequestItem
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialRequestID")]
    public long InventoryMatrialRequestId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [StringLength(50)]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

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

    [Column("PurchaseUOMID")]
    public int? PurchaseUomid { get; set; }

    [Column("PurchaseUOMShortName")]
    [StringLength(50)]
    public string PurchaseUomshortName { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchaseQuantity { get; set; }

    [Column("RequestedUOMID")]
    public int? RequestedUomid { get; set; }

    [Column("RequestedUOMShortName")]
    [StringLength(50)]
    public string RequestedUomshortName { get; set; }

    [Column("FromBOM")]
    public bool? FromBom { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }

    public bool? IsHold { get; set; }

    [Column("ToInventoryStoreID")]
    public int ToInventoryStoreId { get; set; }

    [Required]
    [StringLength(1000)]
    public string InventoryStoreName { get; set; }

    [Required]
    [StringLength(50)]
    public string MaterialRequestStatus { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }
}
