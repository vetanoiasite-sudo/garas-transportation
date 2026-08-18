using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialReleaseItem
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

    [Column("InventoryMatrialReleasetID")]
    public long InventoryMatrialReleasetId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? NewRecivedQuantity { get; set; }

    public string NewComments { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column("RequestTypeID")]
    public long? RequestTypeId { get; set; }

    [Column("FromUserID")]
    public long FromUserId { get; set; }

    [StringLength(50)]
    public string ApproveResult { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    public int? CalculationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CustomeUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? LastUnitPrice { get; set; }

    [Column("FromBOM")]
    public bool? FromBom { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }
}
