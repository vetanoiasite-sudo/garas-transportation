using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VacationPaymentStrategy")]
public partial class VacationPaymentStrategy
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Strategy { get; set; }

    public bool? Active { get; set; }

    [InverseProperty("VacationPaymentStrategy")]
    public virtual ICollection<OffDay> OffDays { get; set; } = new List<OffDay>();
}
