using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectCostingType")]
public partial class ProjectCostingType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string CostingTypeName { get; set; }

    [InverseProperty("CostType")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
