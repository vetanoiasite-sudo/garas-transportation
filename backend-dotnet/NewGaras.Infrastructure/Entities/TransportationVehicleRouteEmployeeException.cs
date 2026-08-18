using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationVehicleRouteEmployeeException")]
public partial class TransportationVehicleRouteEmployeeException
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

    [StringLength(100)]
    public string DayName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExceptionDate { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Longtitud { get; set; }

    [StringLength(50)]
    public string ContactNumber { get; set; }

    public string ReasonException { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleRouteEmployeeExceptions")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("TransportationVehicleRouteEmployeeExceptions")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationVehicleRouteEmployeeExceptions")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }

    [ForeignKey("TransportationVehicleRouteDirectionId")]
    [InverseProperty("TransportationVehicleRouteEmployeeExceptions")]
    public virtual TransportationVehicleRouteDirection TransportationVehicleRouteDirection { get; set; }
}
