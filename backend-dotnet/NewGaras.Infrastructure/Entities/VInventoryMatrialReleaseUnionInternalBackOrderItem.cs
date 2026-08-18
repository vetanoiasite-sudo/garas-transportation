using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialReleaseUnionInternalBackOrderItem
{
    [Column("InventoryMatrialReleasetID")]
    public long? InventoryMatrialReleasetId { get; set; }

    [Column("InventoryMatrialRequestID")]
    public long? InventoryMatrialRequestId { get; set; }

    [StringLength(50)]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    [Column("UOMShortName")]
    [StringLength(50)]
    public string UomshortName { get; set; }

    [Column("InventoryInternalBackOrderItemsID")]
    public long? InventoryInternalBackOrderItemsId { get; set; }

    [Column("InventoryInternalBackOrderID")]
    public long? InventoryInternalBackOrderId { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("Fab num")]
    public int? FabNum { get; set; }
}
