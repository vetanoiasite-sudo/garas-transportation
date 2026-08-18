using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryInternalBackOrderItem
{
    [StringLength(50)]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    [Column("UOMShortName")]
    [StringLength(50)]
    public string UomshortName { get; set; }

    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryInternalBackOrderID")]
    public long InventoryInternalBackOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    public string Comments { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public bool Custody { get; set; }

    [Column("FromID")]
    public long FromId { get; set; }

    [Column("InventoryMatrialReleaseItemID")]
    public long? InventoryMatrialReleaseItemId { get; set; }

    [Column("RemainReleaseQTY", TypeName = "decimal(18, 4)")]
    public decimal? RemainReleaseQty { get; set; }
}
