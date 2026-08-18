using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class RoomFacility
{
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int FacilityId { get; set; }

    [ForeignKey("FacilityId")]
    [InverseProperty("RoomFacilities")]
    public virtual Facility Facility { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomFacilities")]
    public virtual Room Room { get; set; }
}
