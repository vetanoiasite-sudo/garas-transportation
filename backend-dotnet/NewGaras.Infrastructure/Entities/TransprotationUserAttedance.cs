using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransprotationUserAttedance")]
public partial class TransprotationUserAttedance
{
    [Key]
    public long Id { get; set; }

    [Required]
    [Column("type")]
    [StringLength(450)]
    public string Type { get; set; }

    [Required]
    [Column("serial")]
    [StringLength(450)]
    public string Serial { get; set; }

    [Column("checkIn", TypeName = "datetime")]
    public DateTime? CheckIn { get; set; }

    [Column("checkOut", TypeName = "datetime")]
    public DateTime? CheckOut { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? CheckInLatitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? CheckInLongtitud { get; set; }

    public long? CheckInRouteDirectionId { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? CheckOutLatitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? CheckOutLongtitud { get; set; }

    public long? CheckOutRouteDirectionId { get; set; }

    public long? CheckInRouteId { get; set; }

    public long? CheckOutRouteId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OneWayDate { get; set; }

    [ForeignKey("CheckInRouteId")]
    [InverseProperty("TransprotationUserAttedanceCheckInRoutes")]
    public virtual TransportationVehicleRoute CheckInRoute { get; set; }

    [ForeignKey("CheckInRouteDirectionId")]
    [InverseProperty("TransprotationUserAttedanceCheckInRouteDirections")]
    public virtual TransportationVehicleRouteDirection CheckInRouteDirection { get; set; }

    [ForeignKey("CheckOutRouteId")]
    [InverseProperty("TransprotationUserAttedanceCheckOutRoutes")]
    public virtual TransportationVehicleRoute CheckOutRoute { get; set; }

    [ForeignKey("CheckOutRouteDirectionId")]
    [InverseProperty("TransprotationUserAttedanceCheckOutRouteDirections")]
    public virtual TransportationVehicleRouteDirection CheckOutRouteDirection { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransprotationUserAttedances")]
    public virtual User CreationByNavigation { get; set; }
}
