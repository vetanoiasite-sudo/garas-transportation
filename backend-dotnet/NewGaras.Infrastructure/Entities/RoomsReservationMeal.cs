using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class RoomsReservationMeal
{
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int ReservationId { get; set; }

    public int MealTypeId { get; set; }

    [ForeignKey("MealTypeId")]
    [InverseProperty("RoomsReservationMeals")]
    public virtual MealType MealType { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("RoomsReservationMeals")]
    public virtual Reservation Reservation { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomsReservationMeals")]
    public virtual Room Room { get; set; }
}
