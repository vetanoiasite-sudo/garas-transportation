using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryItemInventoryItemCategory
{
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    public string ItemName { get; set; }

    public bool ItemActive { get; set; }

    [Column("InventoryItemCategoryID")]
    public int InventoryItemCategoryId { get; set; }

    [StringLength(1000)]
    public string CategoryName { get; set; }

    public bool? IsFinalProduct { get; set; }

    public bool? IsRentItem { get; set; }

    public bool? IsAsset { get; set; }

    public bool? IsNonStock { get; set; }

    public bool? CategoryActive { get; set; }
}
