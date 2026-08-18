using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationVehicleRouteEmployee")]
public partial class TransportationVehicleRouteEmployee
{
    [Key]
    public long Id { get; set; }

    [Column("transportationVehicleRouteId")]
    public long TransportationVehicleRouteId { get; set; }

    [Column("hrUserId")]
    public long HrUserId { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    public long? TransportationVehicleRouteDirectionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FromDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ToDate { get; set; }

    [StringLength(100)]
    public string Period { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? DurationLatitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? DurationLongtitud { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleRouteEmployees")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("TransportationVehicleRouteEmployees")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationVehicleRouteEmployees")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }

    [ForeignKey("TransportationVehicleRouteDirectionId")]
    [InverseProperty("TransportationVehicleRouteEmployees")]
    public virtual TransportationVehicleRouteDirection TransportationVehicleRouteDirection { get; set; }
}
