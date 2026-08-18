using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalaryInsurance")]
public partial class SalaryInsurance
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("SalaryID")]
    public long SalaryId { get; set; }

    public bool HaveSocialInsurance { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal EmployeePercent { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal CompanyPercent { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal EmployeeAmount { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal CompanyAmount { get; set; }

    public bool HaveMedicalInsurance { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal CoveragePercent { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal MaxAmount { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalaryInsuranceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalaryInsuranceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SalaryId")]
    [InverseProperty("SalaryInsurances")]
    public virtual Salary Salary { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SalaryInsuranceUsers")]
    public virtual User User { get; set; }
}
