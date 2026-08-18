using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class RoomsReservation
{
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int ReservationId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("RoomsReservations")]
    public virtual Reservation Reservation { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomsReservations")]
    public virtual Room Room { get; set; }
}
