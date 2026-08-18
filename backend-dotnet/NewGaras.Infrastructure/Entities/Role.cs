using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Role")]
public partial class Role
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
    [InverseProperty("RoleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<GroupRole> GroupRoles { get; set; } = new List<GroupRole>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("RoleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<ProjectAssignUser> ProjectAssignUsers { get; set; } = new List<ProjectAssignUser>();

    [InverseProperty("Role")]
    public virtual ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();

    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
