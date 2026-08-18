using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class View1
{
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    [Column("InventoryItemCategoryID")]
    public int InventoryItemCategoryId { get; set; }

    [StringLength(1000)]
    public string CategoryName { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(1000)]
    public string CommercialName { get; set; }

    [StringLength(1000)]
    public string MarketName { get; set; }

    public string Details { get; set; }

    [Column("exported")]
    [StringLength(50)]
    public string Exported { get; set; }

    [Column("PurchasingUOMID")]
    public int PurchasingUomid { get; set; }

    [Column("PurchasingUOMSHortName")]
    [StringLength(50)]
    public string PurchasingUomshortName { get; set; }

    [Column("RequstionUOMID")]
    public int RequstionUomid { get; set; }

    [Column("RequestionUOMSHortName")]
    [StringLength(50)]
    public string RequestionUomshortName { get; set; }

    [Column(TypeName = "decimal(18, 6)")]
    public decimal? ExchangeFactor { get; set; }

    public int CalculationType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal CustomeUnitPrice { get; set; }

    public bool? HasImage { get; set; }

    public byte[] Image { get; set; }

    public bool? IsFinalProduct { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinBalance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaxBalance { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal AverageUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal MaxUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal LastUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? StockBalance { get; set; }
}
