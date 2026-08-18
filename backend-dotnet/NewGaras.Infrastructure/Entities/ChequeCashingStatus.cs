using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ChequeCashingStatus")]
public partial class ChequeCashingStatus
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Status { get; set; }

    public bool Active { get; set; }

    [InverseProperty("ChequeChashingStatus")]
    public virtual ICollection<ProjectCheque> ProjectCheques { get; set; } = new List<ProjectCheque>();
}
