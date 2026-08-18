using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("CompanySpecialty")]
public partial class CompanySpecialty
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SpecialityID")]
    public long SpecialityId { get; set; }

    [Required]
    [StringLength(250)]
    public string SpecialityName { get; set; }

    public string Description { get; set; }
}
