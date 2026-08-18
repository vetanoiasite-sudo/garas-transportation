using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectAssignUser")]
public partial class ProjectAssignUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreationBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("ProjectAssignUserCreationByNavigations")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectAssignUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectAssignUsers")]
    public virtual Project Project { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("ProjectAssignUsers")]
    public virtual Role Role { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ProjectAssignUserUsers")]
    public virtual User User { get; set; }
}
