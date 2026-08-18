using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferAttachmentUserPermission")]
public partial class SalesOfferAttachmentUserPermission
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("OfferAttachmentID")]
    public long OfferAttachmentId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

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
    [InverseProperty("SalesOfferAttachmentUserPermissionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferAttachmentUserPermissionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferAttachmentId")]
    [InverseProperty("SalesOfferAttachmentUserPermissions")]
    public virtual SalesOfferAttachment OfferAttachment { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("SalesOfferAttachmentUserPermissions")]
    public virtual PermissionLevel Permission { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SalesOfferAttachmentUserPermissionUsers")]
    public virtual User User { get; set; }
}
