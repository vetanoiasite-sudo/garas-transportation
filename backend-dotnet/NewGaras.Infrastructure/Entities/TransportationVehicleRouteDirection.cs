using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationVehicleRouteDirection")]
public partial class TransportationVehicleRouteDirection
{
    [Key]
    public long Id { get; set; }

    [Column("transportationVehicleRouteId")]
    public long TransportationVehicleRouteId { get; set; }

    [Required]
    [Column("routeDirection")]
    [StringLength(450)]
    public string RouteDirection { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longtitud { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleRouteDirections")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationVehicleRouteDirections")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }

    [InverseProperty("TransportationVehicleRouteDirection")]
    public virtual ICollection<TransportationVehicleRouteEmployeeException> TransportationVehicleRouteEmployeeExceptions { get; set; } = new List<TransportationVehicleRouteEmployeeException>();

    [InverseProperty("TransportationVehicleRouteDirection")]
    public virtual ICollection<TransportationVehicleRouteEmployee> TransportationVehicleRouteEmployees { get; set; } = new List<TransportationVehicleRouteEmployee>();

    [InverseProperty("CheckInRouteDirection")]
    public virtual ICollection<TransprotationUserAttedance> TransprotationUserAttedanceCheckInRouteDirections { get; set; } = new List<TransprotationUserAttedance>();

    [InverseProperty("CheckOutRouteDirection")]
    public virtual ICollection<TransprotationUserAttedance> TransprotationUserAttedanceCheckOutRouteDirections { get; set; } = new List<TransprotationUserAttedance>();
}
