using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserRole")]
public partial class UserRole
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("HrUserID")]
    public long? HrUserId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserRoleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("UserRoles")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserRoleUsers")]
    public virtual User User { get; set; }
}
