using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialRequestWithItem
{
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

    [Column("FabricationOrderID")]
    public long? FabricationOrderId { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    public string Comments { get; set; }

    [Column("ReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity1 { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchaseQuantity { get; set; }

    [Column("FromBOM")]
    public bool? FromBom { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }

    public bool? IsHold { get; set; }

    [Column("RequestTypeID")]
    public long? RequestTypeId { get; set; }

    [StringLength(50)]
    public string ApproveResult { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("FromUserID")]
    public long FromUserId { get; set; }

    [Column("ToInventoryStoreID")]
    public int ToInventoryStoreId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    [Required]
    [StringLength(200)]
    public string RequestTypeName { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    public long? ClientId { get; set; }
}
