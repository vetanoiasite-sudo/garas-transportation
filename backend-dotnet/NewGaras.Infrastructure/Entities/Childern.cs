using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Childern
{
    [Key]
    public int Id { get; set; }

    public int Years { get; set; }

    public int RoomId { get; set; }

    public int ReservationId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("Childerns")]
    public virtual Reservation Reservation { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("Childerns")]
    public virtual Room Room { get; set; }
}
