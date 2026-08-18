using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Facility
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FacilityName { get; set; }

    [InverseProperty("Facility")]
    public virtual ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
}
