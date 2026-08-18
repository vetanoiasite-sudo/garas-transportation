using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EmailCategoryType")]
public partial class EmailCategoryType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(150)]
    public string CategoryName { get; set; }

    [InverseProperty("CategoryType")]
    public virtual ICollection<EmailCategory> EmailCategories { get; set; } = new List<EmailCategory>();
}
