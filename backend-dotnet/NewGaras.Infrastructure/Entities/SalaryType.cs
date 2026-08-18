using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalaryType")]
public partial class SalaryType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string SalaryName { get; set; }

    public bool Active { get; set; }

    [InverseProperty("SalaryType")]
    public virtual ICollection<AllowncesType> AllowncesTypes { get; set; } = new List<AllowncesType>();

    [InverseProperty("SalaryType")]
    public virtual ICollection<SalaryTax> SalaryTaxes { get; set; } = new List<SalaryTax>();
}
