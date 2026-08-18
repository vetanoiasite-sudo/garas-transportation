using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryStoreKeeper
{
    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool Active { get; set; }

    [StringLength(1000)]
    public string Name { get; set; }

    public bool? StoreActive { get; set; }
}
