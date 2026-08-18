using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryAddingOrder")]
public partial class InventoryAddingOrder
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string OperationType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public int Revision { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    [Column("PONumber")]
    [StringLength(50)]
    public string Ponumber { get; set; }

    public int LoadBy { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecivingDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryAddingOrderCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("InventoryAddingOrder")]
    public virtual ICollection<InventoryAddingOrderItem> InventoryAddingOrderItems { get; set; } = new List<InventoryAddingOrderItem>();

    [ForeignKey("InventoryStoreId")]
    [InverseProperty("InventoryAddingOrders")]
    public virtual InventoryStore InventoryStore { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryAddingOrderModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("InventoryAddingOrders")]
    public virtual Supplier Supplier { get; set; }
}
