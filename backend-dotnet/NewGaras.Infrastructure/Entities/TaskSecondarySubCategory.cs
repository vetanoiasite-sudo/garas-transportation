using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskSecondarySubCategory")]
public partial class TaskSecondarySubCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("TaskPrimarySubCategoryID")]
    public long TaskPrimarySubCategoryId { get; set; }

    public bool Active { get; set; }

    public string Description { get; set; }

    [ForeignKey("TaskPrimarySubCategoryId")]
    [InverseProperty("TaskSecondarySubCategories")]
    public virtual TaskPrimarySubCategory TaskPrimarySubCategory { get; set; }
}
