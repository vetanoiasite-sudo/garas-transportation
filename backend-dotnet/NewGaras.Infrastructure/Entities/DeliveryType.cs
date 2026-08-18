using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DeliveryType")]
public partial class DeliveryType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Type { get; set; }

    public bool Active { get; set; }

    [InverseProperty("DeliveryType")]
    public virtual ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();
}
