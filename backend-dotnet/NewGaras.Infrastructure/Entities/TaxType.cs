using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaxType")]
public partial class TaxType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string TaxName { get; set; }

    public bool Active { get; set; }

    [InverseProperty("TaxType")]
    public virtual ICollection<SalaryTax> SalaryTaxes { get; set; } = new List<SalaryTax>();
}
