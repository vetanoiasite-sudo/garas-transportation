using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VCountry
{
    [Column("CountryID")]
    public int CountryId { get; set; }

    [Required]
    [StringLength(500)]
    public string CountryName { get; set; }

    [Column("GovernorateID")]
    public int GovernorateId { get; set; }

    [Required]
    [StringLength(500)]
    public string GovernorateName { get; set; }

    [Column("AreaID")]
    public long AreaId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AreaName { get; set; }
}
