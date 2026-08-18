using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class TypeService
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [InverseProperty("TypeServices")]
    public virtual ICollection<ExpensessStatus> ExpensessStatuses { get; set; } = new List<ExpensessStatus>();
}
