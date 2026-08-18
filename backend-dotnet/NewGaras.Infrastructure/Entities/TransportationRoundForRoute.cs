using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationRoundForRoute")]
public partial class TransportationRoundForRoute
{
    [Key]
    public int Id { get; set; }

    [Column("transportationVehicleRouteId")]
    public long? TransportationVehicleRouteId { get; set; }

    [Column("serial")]
    [StringLength(100)]
    public string Serial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CheckInDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CheckOutDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OneWayDate { get; set; }

    public long SupplierId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PriceRound { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("TransportationRoundForRoutes")]
    public virtual Supplier Supplier { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationRoundForRoutes")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }
}
