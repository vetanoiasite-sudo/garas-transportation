using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalaryDeductionTax")]
public partial class SalaryDeductionTax
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalaryID")]
    public long SalaryId { get; set; }

    [Required]
    [StringLength(250)]
    public string TaxName { get; set; }

    [Column("percentage", TypeName = "decimal(8, 2)")]
    public decimal Percentage { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Amount { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column("DeductionTypeID")]
    public int? DeductionTypeId { get; set; }

    [Column("SalaryTaxID")]
    public long? SalaryTaxId { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalaryDeductionTaxCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DeductionTypeId")]
    [InverseProperty("SalaryDeductionTaxes")]
    public virtual DeductionType DeductionType { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalaryDeductionTaxModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SalaryId")]
    [InverseProperty("SalaryDeductionTaxes")]
    public virtual Salary Salary { get; set; }

    [ForeignKey("SalaryTaxId")]
    [InverseProperty("SalaryDeductionTaxes")]
    public virtual SalaryTax SalaryTax { get; set; }
}
