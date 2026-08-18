using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DeductionType")]
public partial class DeductionType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DeductionTypes")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("DeductionType")]
    public virtual ICollection<SalaryDeductionTax> SalaryDeductionTaxes { get; set; } = new List<SalaryDeductionTax>();
}
