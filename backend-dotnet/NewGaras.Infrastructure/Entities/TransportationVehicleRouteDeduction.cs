using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationVehicleRouteDeduction")]
public partial class TransportationVehicleRouteDeduction
{
    [Key]
    public int Id { get; set; }

    [Column("supplierId")]
    public long? SupplierId { get; set; }

    [Column("transportationVehicleRouteId")]
    public long TransportationVehicleRouteId { get; set; }

    [Column("serial")]
    [StringLength(450)]
    public string Serial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfDeduction { get; set; }

    [Column("deductPerRound", TypeName = "decimal(10, 4)")]
    public decimal? DeductPerRound { get; set; }

    [Column("cause")]
    public string Cause { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    [Column("creationBy")]
    public long? CreationBy { get; set; }

    [Required]
    [StringLength(50)]
    public string TypeOfDedct { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleRouteDeductions")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationVehicleRouteDeductions")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }
}
