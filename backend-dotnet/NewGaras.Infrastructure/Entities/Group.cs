using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Group")]
public partial class Group
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("GroupCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<GroupRole> GroupRoles { get; set; } = new List<GroupRole>();

    [InverseProperty("Group")]
    public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("GroupModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<ReportCcgroup> ReportCcgroups { get; set; } = new List<ReportCcgroup>();

    [InverseProperty("Group")]
    public virtual ICollection<ReportGroup> ReportGroups { get; set; } = new List<ReportGroup>();

    [InverseProperty("Group")]
    public virtual ICollection<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissions { get; set; } = new List<SalesOfferAttachmentGroupPermission>();

    [InverseProperty("Group")]
    public virtual ICollection<SalesOfferGroupPermission> SalesOfferGroupPermissions { get; set; } = new List<SalesOfferGroupPermission>();

    [InverseProperty("Group")]
    public virtual ICollection<SalesOfferInternalApproval> SalesOfferInternalApprovals { get; set; } = new List<SalesOfferInternalApproval>();
}
