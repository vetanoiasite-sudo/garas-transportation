using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Rate
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public DateTime? StartingDate { get; set; }

    public DateTime? EndingDate { get; set; }

    [Required]
    public string Currency { get; set; }

    public int RoomRate { get; set; }

    public int? RoomTypeId { get; set; }

    public int? BuildingId { get; set; }

    public int? RoomViewId { get; set; }

    public byte SpecialOfferFlag { get; set; }

    [ForeignKey("BuildingId")]
    [InverseProperty("Rates")]
    public virtual Building Building { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("Rates")]
    public virtual Room Room { get; set; }

    [ForeignKey("RoomTypeId")]
    [InverseProperty("Rates")]
    public virtual RoomType RoomType { get; set; }

    [ForeignKey("RoomViewId")]
    [InverseProperty("Rates")]
    public virtual RoomView RoomView { get; set; }
}
