using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRCustodyStatus")]
public partial class HrcustodyStatus
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Status { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Hrcustody> Hrcustodies { get; set; } = new List<Hrcustody>();
}
