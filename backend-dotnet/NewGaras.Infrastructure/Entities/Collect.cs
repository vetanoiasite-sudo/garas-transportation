using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Collect
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? Date { get; set; }

    public int ReservationId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("Collects")]
    public virtual Reservation Reservation { get; set; }
}
