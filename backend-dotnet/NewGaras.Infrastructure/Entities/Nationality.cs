using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Nationality")]
public partial class Nationality
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("Nationality")]
    [StringLength(250)]
    public string Nationality1 { get; set; }

    [InverseProperty("Nationality")]
    public virtual ICollection<ClientExtraInfo> ClientExtraInfos { get; set; } = new List<ClientExtraInfo>();
}
