using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PRSupplierOfferItem")]
public partial class PrsupplierOfferItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PRSupplierOfferID")]
    public long PrsupplierOfferId { get; set; }

    [Column("PRID")]
    public long Prid { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column("MRItemID")]
    public long MritemId { get; set; }

    [Column("PRItemID")]
    public long PritemId { get; set; }

    [Column("POItemID")]
    public long? PoitemId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [StringLength(250)]
    public string Comment { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ReqQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? RecivedQuantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? EstimatedCost { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? TotalEstimatedCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column("RateToEGP", TypeName = "decimal(18, 5)")]
    public decimal? RateToEgp { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("MritemId")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual InventoryMatrialRequestItem Mritem { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("PoitemId")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual PurchasePoitem Poitem { get; set; }

    [ForeignKey("Prid")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual PurchaseRequestItem Pr { get; set; }

    [ForeignKey("PrsupplierOfferId")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual PrsupplierOffer PrsupplierOffer { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("PrsupplierOfferItems")]
    public virtual InventoryUom Uom { get; set; }
}
