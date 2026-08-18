using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("LetterOfCreditType")]
public partial class LetterOfCreditType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("LOCTypeName")]
    [StringLength(50)]
    public string LoctypeName { get; set; }

    [InverseProperty("LetterOfCreditType")]
    public virtual ICollection<ProjectLetterOfCredit> ProjectLetterOfCredits { get; set; } = new List<ProjectLetterOfCredit>();
}
