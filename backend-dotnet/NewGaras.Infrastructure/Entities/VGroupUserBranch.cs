using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VGroupUserBranch
{
    [Column("GroupID")]
    public long GroupId { get; set; }

    [Required]
    [StringLength(500)]
    public string GroupName { get; set; }

    public string Description { get; set; }

    public bool GroupActive { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    public bool? UserGroupActive { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Column("DepartmentID")]
    public int? DepartmentId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }
}
