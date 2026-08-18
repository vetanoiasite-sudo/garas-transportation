using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Building
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    public string BuildingName { get; set; }

    [InverseProperty("Building")]
    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();

    [InverseProperty("Building")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
