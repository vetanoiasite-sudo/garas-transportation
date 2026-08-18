using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOM")]
public partial class Bom
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [StringLength(50)]
    public string Serial { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    [Column("ProductID")]
    public long? ProductId { get; set; }

    public int Revision { get; set; }

    public string Description { get; set; }

    public int NumberOfUsed { get; set; }

    [Column("BOMWorkingHours", TypeName = "decimal(18, 2)")]
    public decimal BomworkingHours { get; set; }

    [Column("BOMTotalWorkingHoursCost", TypeName = "decimal(18, 4)")]
    public decimal BomtotalWorkingHoursCost { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("OfferID")]
    public long? OfferId { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("OfferItemID")]
    public long? OfferItemId { get; set; }

    [Column("MainBOMID")]
    public long? MainBomid { get; set; }

    [InverseProperty("Bom")]
    public virtual ICollection<Bomattachment> Bomattachments { get; set; } = new List<Bomattachment>();

    [InverseProperty("Bom")]
    public virtual ICollection<Bomhistory> Bomhistories { get; set; } = new List<Bomhistory>();

    [InverseProperty("Bom")]
    public virtual ICollection<Bomimage> Bomimages { get; set; } = new List<Bomimage>();

    [InverseProperty("Bom")]
    public virtual ICollection<Bompartition> Bompartitions { get; set; } = new List<Bompartition>();

    [InverseProperty("Bom")]
    public virtual ICollection<Bomproduct> Bomproducts { get; set; } = new List<Bomproduct>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("BomCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BomModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Boms")]
    public virtual InventoryItem Product { get; set; }

    [ForeignKey("ProductGroupId")]
    [InverseProperty("Boms")]
    public virtual InventoryItemCategory ProductGroup { get; set; }

    [InverseProperty("Bom")]
    public virtual ICollection<VehicleMaintenanceType> VehicleMaintenanceTypes { get; set; } = new List<VehicleMaintenanceType>();
}
