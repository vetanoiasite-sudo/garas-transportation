using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class RoomsReservationChildern
{
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int ReservationId { get; set; }

    public int NumbersofAdulte { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("RoomsReservationChilderns")]
    public virtual Reservation Reservation { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomsReservationChilderns")]
    public virtual Room Room { get; set; }
}
