using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Languagee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("shortvalue")]
    public string Shortvalue { get; set; }

    [Required]
    [Column("value")]
    public string Value { get; set; }

    [InverseProperty("Languagee")]
    public virtual ICollection<ClientLanguagee> ClientLanguagees { get; set; } = new List<ClientLanguagee>();
}
