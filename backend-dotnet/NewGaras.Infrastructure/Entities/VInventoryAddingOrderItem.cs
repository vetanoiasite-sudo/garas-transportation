using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryAddingOrderItem
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryAddingOrderID")]
    public long InventoryAddingOrderId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [StringLength(50)]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity { get; set; }

    [Column("UOMShortName")]
    [StringLength(50)]
    public string UomshortName { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [StringLength(50)]
    public string ItemSerial { get; set; }

    [Column("QCReportID")]
    public long? QcreportId { get; set; }

    public string Comments { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("PurchasingUOMID")]
    public int? PurchasingUomid { get; set; }

    [Column("RequstionUOMShortName")]
    [StringLength(50)]
    public string RequstionUomshortName { get; set; }

    [Column("PurchasingUOMShortName")]
    [StringLength(50)]
    public string PurchasingUomshortName { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantityAfter { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RemainQuantity { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    [Required]
    [StringLength(50)]
    public string OperationType { get; set; }
}
