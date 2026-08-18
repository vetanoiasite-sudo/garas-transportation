using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VCustodyRequestReleaseBackOrder
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool IsAssetsType { get; set; }

    public long MaterialRequestItemId { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RequestedQuantity { get; set; }

    [Column("InventoryMatrialRequestID")]
    public long? InventoryMatrialRequestId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [Column("InventoryMatrialReleaseItemID")]
    public long? InventoryMatrialReleaseItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReleasedQuantity { get; set; }

    [Column("InventoryMatrialReleaseID")]
    public long? InventoryMatrialReleaseId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReleaseDate { get; set; }

    [Column("InventoryInternalBackOrderItemID")]
    public long? InventoryInternalBackOrderItemId { get; set; }

    [Column("InventoryInternalBackOrderID")]
    public long? InventoryInternalBackOrderId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReturnedQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RemainQuantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReturnDate { get; set; }

    public string Description { get; set; }

    public string SignedMemoAttachmentPath { get; set; }

    [StringLength(50)]
    public string SignedMemoFileName { get; set; }

    [StringLength(50)]
    public string SignedMemoFileExtension { get; set; }

    [Column("CustodyStatusID")]
    public int CustodyStatusId { get; set; }

    [StringLength(250)]
    public string CustodyStatus { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CustodyCreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }
}
