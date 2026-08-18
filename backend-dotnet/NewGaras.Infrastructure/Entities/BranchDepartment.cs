using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BranchDepartment")]
public partial class BranchDepartment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("BranchDepartments")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BranchDepartmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("BranchDepartments")]
    public virtual Department Department { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BranchDepartmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
