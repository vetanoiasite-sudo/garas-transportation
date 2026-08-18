using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PRSupplierOffer")]
public partial class PrsupplierOffer
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PRID")]
    public long Prid { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    [Required]
    [StringLength(250)]
    public string Status { get; set; }

    [StringLength(250)]
    public string Comment { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PrsupplierOfferCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PrsupplierOfferModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PrsupplierOffers")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("Prid")]
    [InverseProperty("PrsupplierOffers")]
    public virtual PurchaseRequest Pr { get; set; }

    [InverseProperty("PrsupplierOffer")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [ForeignKey("SupplierId")]
    [InverseProperty("PrsupplierOffers")]
    public virtual Supplier Supplier { get; set; }
}
