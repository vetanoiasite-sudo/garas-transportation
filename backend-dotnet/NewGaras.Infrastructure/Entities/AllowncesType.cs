using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AllowncesType")]
public partial class AllowncesType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? Percentage { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? Amount { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column("SalaryTypeID")]
    public int? SalaryTypeId { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("AllowncesTypes")]
    public virtual Currency Currency { get; set; }

    [InverseProperty("AllowncesType")]
    public virtual ICollection<SalaryAllownce> SalaryAllownces { get; set; } = new List<SalaryAllownce>();

    [ForeignKey("SalaryTypeId")]
    [InverseProperty("AllowncesTypes")]
    public virtual SalaryType SalaryType { get; set; }
}
