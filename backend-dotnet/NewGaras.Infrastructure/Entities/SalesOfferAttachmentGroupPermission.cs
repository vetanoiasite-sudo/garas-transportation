using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferAttachmentGroupPermission")]
public partial class SalesOfferAttachmentGroupPermission
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("OfferAttachmentID")]
    public long OfferAttachmentId { get; set; }

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
    [InverseProperty("SalesOfferAttachmentGroupPermissionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("SalesOfferAttachmentGroupPermissions")]
    public virtual Group Group { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferAttachmentGroupPermissionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferAttachmentId")]
    [InverseProperty("SalesOfferAttachmentGroupPermissions")]
    public virtual SalesOfferAttachment OfferAttachment { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("SalesOfferAttachmentGroupPermissions")]
    public virtual PermissionLevel Permission { get; set; }
}
