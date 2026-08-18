using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectLetterOfCredit")]
public partial class ProjectLetterOfCredit
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("LetterOfCreditTypeID")]
    public int LetterOfCreditTypeId { get; set; }

    public int ReturnedAfter { get; set; }

    [Required]
    [StringLength(250)]
    public string BankName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amout { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(100)]
    public string Status { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("ProjectLetterOfCredits")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("LetterOfCreditTypeId")]
    [InverseProperty("ProjectLetterOfCredits")]
    public virtual LetterOfCreditType LetterOfCreditType { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectLetterOfCredits")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectLetterOfCredit")]
    public virtual ICollection<ProjectLetterOfCreditComment> ProjectLetterOfCreditComments { get; set; } = new List<ProjectLetterOfCreditComment>();
}
