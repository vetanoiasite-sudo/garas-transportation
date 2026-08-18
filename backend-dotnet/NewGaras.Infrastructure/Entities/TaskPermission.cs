using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskPermission")]
public partial class TaskPermission
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Column("UserGroupID")]
    public long UserGroupId { get; set; }

    [Column("PermissionLevelID")]
    public int PermissionLevelId { get; set; }

    public bool IsGroup { get; set; }

    [ForeignKey("PermissionLevelId")]
    [InverseProperty("TaskPermissions")]
    public virtual PermissionLevel PermissionLevel { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskPermissions")]
    public virtual Task Task { get; set; }
}
