using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("National")]
public partial class National
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("alpha_2_code")]
    public string Alpha2Code { get; set; }

    [Required]
    [Column("alpha_3_code")]
    public string Alpha3Code { get; set; }

    [Required]
    [Column("en_short_name")]
    public string EnShortName { get; set; }

    [Required]
    [Column("nationality")]
    public string Nationality { get; set; }

    [InverseProperty("National")]
    public virtual ICollection<ClientNational> ClientNationals { get; set; } = new List<ClientNational>();
}
