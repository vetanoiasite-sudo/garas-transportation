using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SupplierContactPerson")]
public partial class SupplierContactPerson
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Mobile { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SupplierContactPersonCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SupplierContactPersonModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("SupplierContactPerson")]
    public virtual ICollection<PofinalSelecteSupplier> PofinalSelecteSuppliers { get; set; } = new List<PofinalSelecteSupplier>();

    [InverseProperty("SentToSupplierContactPerson")]
    public virtual ICollection<PurchasePo> PurchasePos { get; set; } = new List<PurchasePo>();

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierContactPeople")]
    public virtual Supplier Supplier { get; set; }

    [InverseProperty("SupplierContactPerson")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRoutes { get; set; } = new List<TransportationVehicleRoute>();
}
