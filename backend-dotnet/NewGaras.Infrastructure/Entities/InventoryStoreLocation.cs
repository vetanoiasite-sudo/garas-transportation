using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryStoreLocation")]
public partial class InventoryStoreLocation
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [Required]
    [StringLength(250)]
    public string Location { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("InventoryStoreId")]
    [InverseProperty("InventoryStoreLocations")]
    public virtual InventoryStore InventoryStore { get; set; }

    [InverseProperty("InvenoryStoreLocation")]
    public virtual ICollection<InventoryStoreItem> InventoryStoreItems { get; set; } = new List<InventoryStoreItem>();
}
