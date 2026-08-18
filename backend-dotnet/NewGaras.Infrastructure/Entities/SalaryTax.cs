using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalaryTax")]
public partial class SalaryTax
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Percentage { get; set; }

    public long Min { get; set; }

    public long Max { get; set; }

    public bool? Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? CreationBy { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    public int? SalaryTypeId { get; set; }

    public int? TaxTypeId { get; set; }

    [StringLength(450)]
    public string TaxTypeName { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("SalaryTaxes")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("SalaryTaxes")]
    public virtual User CreationByNavigation { get; set; }

    [InverseProperty("SalaryTax")]
    public virtual ICollection<SalaryDeductionTax> SalaryDeductionTaxes { get; set; } = new List<SalaryDeductionTax>();

    [ForeignKey("SalaryTypeId")]
    [InverseProperty("SalaryTaxes")]
    public virtual SalaryType SalaryType { get; set; }

    [ForeignKey("TaxTypeId")]
    [InverseProperty("SalaryTaxes")]
    public virtual TaxType TaxType { get; set; }
}
