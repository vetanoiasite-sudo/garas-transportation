using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryItemPrice")]
public partial class InventoryItemPrice
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Price { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Column("SupplierID")]
    public long? SupplierId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryItemPrices")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("InventoryItemPrices")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("InventoryItemPrices")]
    public virtual Supplier Supplier { get; set; }
}
