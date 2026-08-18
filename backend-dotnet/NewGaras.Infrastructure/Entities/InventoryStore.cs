using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryStore")]
public partial class InventoryStore
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public string Name { get; set; }

    public bool Active { get; set; }

    public string Location { get; set; }

    [StringLength(50)]
    public string Tel { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryStoreCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryAddingOrder> InventoryAddingOrders { get; set; } = new List<InventoryAddingOrder>();

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryInternalBackOrder> InventoryInternalBackOrders { get; set; } = new List<InventoryInternalBackOrder>();

    [InverseProperty("FromInventoryStore")]
    public virtual ICollection<InventoryInternalTransferOrder> InventoryInternalTransferOrderFromInventoryStores { get; set; } = new List<InventoryInternalTransferOrder>();

    [InverseProperty("ToInventoryStore")]
    public virtual ICollection<InventoryInternalTransferOrder> InventoryInternalTransferOrderToInventoryStores { get; set; } = new List<InventoryInternalTransferOrder>();

    [InverseProperty("FromInventoryStore")]
    public virtual ICollection<InventoryMatrialRelease> InventoryMatrialReleases { get; set; } = new List<InventoryMatrialRelease>();

    [InverseProperty("ToInventoryStore")]
    public virtual ICollection<InventoryMatrialRequest> InventoryMatrialRequests { get; set; } = new List<InventoryMatrialRequest>();

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryReport> InventoryReports { get; set; } = new List<InventoryReport>();

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryStoreItem> InventoryStoreItems { get; set; } = new List<InventoryStoreItem>();

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryStoreKeeper> InventoryStoreKeepers { get; set; } = new List<InventoryStoreKeeper>();

    [InverseProperty("InventoryStore")]
    public virtual ICollection<InventoryStoreLocation> InventoryStoreLocations { get; set; } = new List<InventoryStoreLocation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryStoreModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Store")]
    public virtual ICollection<PosClosingDay> PosClosingDays { get; set; } = new List<PosClosingDay>();

    [InverseProperty("Store")]
    public virtual ICollection<PosNumber> PosNumbers { get; set; } = new List<PosNumber>();

    [InverseProperty("FromInventoryStore")]
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();
}
