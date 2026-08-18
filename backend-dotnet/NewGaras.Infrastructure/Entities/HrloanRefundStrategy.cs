using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRLoanRefundStrategy")]
public partial class HrloanRefundStrategy
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Strategy { get; set; }

    public bool Active { get; set; }

    [InverseProperty("RefundStrategy")]
    public virtual ICollection<Hrloan> Hrloans { get; set; } = new List<Hrloan>();
}
