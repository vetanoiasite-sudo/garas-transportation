using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryStoreItemMinBalance
{
    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    public bool? Active { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? Balance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinBalance { get; set; }

    public bool? StoreActive { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    public string InventoryItemName { get; set; }

    [StringLength(50)]
    public string Code { get; set; }
}
