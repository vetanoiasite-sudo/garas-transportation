using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskPrimarySubCategory")]
public partial class TaskPrimarySubCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("TaskCategoryID")]
    public long? TaskCategoryId { get; set; }

    public bool Active { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskPrimarySubCategoryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskPrimarySubCategoryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskCategoryId")]
    [InverseProperty("TaskPrimarySubCategories")]
    public virtual TaskCategory TaskCategory { get; set; }

    [InverseProperty("TaskPrimarySubCategory")]
    public virtual ICollection<TaskSecondarySubCategory> TaskSecondarySubCategories { get; set; } = new List<TaskSecondarySubCategory>();
}
