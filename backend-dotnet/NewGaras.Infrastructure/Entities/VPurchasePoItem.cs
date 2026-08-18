using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchasePoItem
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    public string InventoryItemName { get; set; }

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

    public int? FabNumber { get; set; }

    public string Comments { get; set; }

    [Column("PurchasePOID")]
    public long PurchasePoid { get; set; }

    [Column("PurchaseRequestItemID")]
    public long PurchaseRequestItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? EstimatedCost { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? EstimatedUnitCost { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ActualUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalUnitCost { get; set; }

    [StringLength(500)]
    public string InvoiceComments { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalActualPrice { get; set; }

    [Column("ToSupplierID")]
    public long? ToSupplierId { get; set; }

    [Column("PORequestDate", TypeName = "datetime")]
    public DateTime? PorequestDate { get; set; }

    [Column("POCreationDate", TypeName = "datetime")]
    public DateTime? PocreationDate { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }

    [Column("RequstionUOMID")]
    public int? RequstionUomid { get; set; }

    [Column("RequstionUOMShortName")]
    [StringLength(50)]
    public string RequstionUomshortName { get; set; }

    [Column("PurchasingUOMID")]
    public int? PurchasingUomid { get; set; }

    [Column("PurchasingUOMShortName")]
    [StringLength(50)]
    public string PurchasingUomshortName { get; set; }

    [StringLength(50)]
    public string ActualUnitPriceUnit { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }
}
