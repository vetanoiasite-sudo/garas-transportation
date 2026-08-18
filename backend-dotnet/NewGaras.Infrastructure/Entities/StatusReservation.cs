using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class StatusReservation
{
    [Key]
    public int Id { get; set; }

    [Column("comment")]
    public string Comment { get; set; }

    [Column("status")]
    public string Status { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? Date { get; set; }

    public int ReservationId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("StatusReservations")]
    public virtual Reservation Reservation { get; set; }
}
