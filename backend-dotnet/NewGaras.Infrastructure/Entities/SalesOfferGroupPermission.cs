using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferGroupPermission")]
public partial class SalesOfferGroupPermission
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

    [Column("GroupID")]
    public long GroupId { get; set; }

    [Column("PermissionID")]
    public int PermissionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferGroupPermissionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("SalesOfferGroupPermissions")]
    public virtual Group Group { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferGroupPermissionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("SalesOfferGroupPermissions")]
    public virtual SalesOffer Offer { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("SalesOfferGroupPermissions")]
    public virtual PermissionLevel Permission { get; set; }
}
