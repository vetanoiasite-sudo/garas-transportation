using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryUOM")]
public partial class InventoryUom
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    [StringLength(50)]
    public string ShortName { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryUomCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Uom")]
    public virtual ICollection<InventoryAddingOrderItem> InventoryAddingOrderItems { get; set; } = new List<InventoryAddingOrderItem>();

    [InverseProperty("Uom")]
    public virtual ICollection<InventoryInternalBackOrderItem> InventoryInternalBackOrderItems { get; set; } = new List<InventoryInternalBackOrderItem>();

    [InverseProperty("Uom")]
    public virtual ICollection<InventoryInternalTransferOrderItem> InventoryInternalTransferOrderItems { get; set; } = new List<InventoryInternalTransferOrderItem>();

    [InverseProperty("PurchasingUom")]
    public virtual ICollection<InventoryItem> InventoryItemPurchasingUoms { get; set; } = new List<InventoryItem>();

    [InverseProperty("RequstionUom")]
    public virtual ICollection<InventoryItem> InventoryItemRequstionUoms { get; set; } = new List<InventoryItem>();

    [InverseProperty("InventoryUom")]
    public virtual ICollection<InventoryItemUom> InventoryItemUoms { get; set; } = new List<InventoryItemUom>();

    [InverseProperty("Uom")]
    public virtual ICollection<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; } = new List<InventoryMatrialRequestItem>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryUomModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Uom")]
    public virtual ICollection<ProjectInvoiceItem> ProjectInvoiceItems { get; set; } = new List<ProjectInvoiceItem>();

    [InverseProperty("Uom")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("Uom")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [InverseProperty("Uom")]
    public virtual ICollection<TaskUnitRateService> TaskUnitRateServices { get; set; } = new List<TaskUnitRateService>();
}
