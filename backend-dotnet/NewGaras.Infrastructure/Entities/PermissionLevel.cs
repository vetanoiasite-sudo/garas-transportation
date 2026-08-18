using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PermissionLevel")]
public partial class PermissionLevel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PermissionLevelCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PermissionLevelModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Permission")]
    public virtual ICollection<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissions { get; set; } = new List<SalesOfferAttachmentGroupPermission>();

    [InverseProperty("Permission")]
    public virtual ICollection<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissions { get; set; } = new List<SalesOfferAttachmentUserPermission>();

    [InverseProperty("Permission")]
    public virtual ICollection<SalesOfferGroupPermission> SalesOfferGroupPermissions { get; set; } = new List<SalesOfferGroupPermission>();

    [InverseProperty("Permission")]
    public virtual ICollection<SalesOfferUserPermission> SalesOfferUserPermissions { get; set; } = new List<SalesOfferUserPermission>();

    [InverseProperty("PermissionLevel")]
    public virtual ICollection<TaskPermission> TaskPermissions { get; set; } = new List<TaskPermission>();
}
