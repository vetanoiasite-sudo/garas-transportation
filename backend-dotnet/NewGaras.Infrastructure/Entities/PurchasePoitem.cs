using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOItem")]
public partial class PurchasePoitem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryMatrialRequestItemID")]
    public long InventoryMatrialRequestItemId { get; set; }

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

    [Column("ReqQuantity", TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity1 { get; set; }

    [Column("RecivedQuantity", TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity1 { get; set; }

    [StringLength(50)]
    public string ActualUnitPriceUnit { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    public bool? IsChecked { get; set; }

    [StringLength(450)]
    public string SupplierInvoiceSerial { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoitems")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("FabricationOrderId")]
    [InverseProperty("PurchasePoitems")]
    public virtual ProjectFabrication FabricationOrder { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("PurchasePoitems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("InventoryMatrialRequestItemId")]
    [InverseProperty("PurchasePoitems")]
    public virtual InventoryMatrialRequestItem InventoryMatrialRequestItem { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PurchasePoitems")]
    public virtual Project Project { get; set; }

    [InverseProperty("Poitem")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [ForeignKey("PurchasePoid")]
    [InverseProperty("PurchasePoitems")]
    public virtual PurchasePo PurchasePo { get; set; }

    [InverseProperty("Poitem")]
    public virtual ICollection<PurchasePoinvoiceExtraFee> PurchasePoinvoiceExtraFees { get; set; } = new List<PurchasePoinvoiceExtraFee>();

    [ForeignKey("PurchaseRequestItemId")]
    [InverseProperty("PurchasePoitems")]
    public virtual PurchaseRequestItem PurchaseRequestItem { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("PurchasePoitems")]
    public virtual InventoryUom Uom { get; set; }
}
