using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("GF_Users")]
public partial class GfUser
{
    [Key]
    public long Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Mobile { get; set; }

    [Required]
    [StringLength(200)]
    public string ChurchName { get; set; }

    [Required]
    [StringLength(200)]
    public string Age { get; set; }

    [Required]
    public string Password { get; set; }
}
