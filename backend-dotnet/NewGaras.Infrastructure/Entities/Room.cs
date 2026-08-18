using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Room
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int RoomTypeId { get; set; }

    public int BuildingId { get; set; }

    public int RoomViewId { get; set; }

    [Required]
    public string Description { get; set; }

    [Column("capacity")]
    public int? Capacity { get; set; }

    [ForeignKey("BuildingId")]
    [InverseProperty("Rooms")]
    public virtual Building Building { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<Childern> Childerns { get; set; } = new List<Childern>();

    [InverseProperty("Room")]
    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();

    [InverseProperty("Room")]
    public virtual ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();

    [ForeignKey("RoomTypeId")]
    [InverseProperty("Rooms")]
    public virtual RoomType RoomType { get; set; }

    [ForeignKey("RoomViewId")]
    [InverseProperty("Rooms")]
    public virtual RoomView RoomView { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<RoomsReservationChildern> RoomsReservationChilderns { get; set; } = new List<RoomsReservationChildern>();

    [InverseProperty("Room")]
    public virtual ICollection<RoomsReservationMeal> RoomsReservationMeals { get; set; } = new List<RoomsReservationMeal>();

    [InverseProperty("Room")]
    public virtual ICollection<RoomsReservation> RoomsReservations { get; set; } = new List<RoomsReservation>();
}
