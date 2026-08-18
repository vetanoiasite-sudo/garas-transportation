using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BillingType")]
public partial class BillingType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string BillingTypeName { get; set; }

    [InverseProperty("BillingType")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
