using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class MealType
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [InverseProperty("MealType")]
    public virtual ICollection<RoomsReservationMeal> RoomsReservationMeals { get; set; } = new List<RoomsReservationMeal>();
}
