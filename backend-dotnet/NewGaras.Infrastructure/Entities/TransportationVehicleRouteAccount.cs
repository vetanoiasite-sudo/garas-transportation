using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class TransportationVehicleRouteAccount
{
    [Key]
    public long Id { get; set; }

    [Column("transportationVehicleRouteId")]
    public long TransportationVehicleRouteId { get; set; }

    [Required]
    [StringLength(450)]
    public string Serial { get; set; }

    public int? CountOfRounds { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? PriceOfRounds { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? DeductPerRounds { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    [Column("modifiedBy")]
    public long? ModifiedBy { get; set; }

    [Column("modifiedDate", TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long SupplierId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfMonth { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleRouteAccountCreationByNavigations")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TransportationVehicleRouteAccountModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("TransportationVehicleRouteAccounts")]
    public virtual Supplier Supplier { get; set; }

    [ForeignKey("TransportationVehicleRouteId")]
    [InverseProperty("TransportationVehicleRouteAccounts")]
    public virtual TransportationVehicleRoute TransportationVehicleRoute { get; set; }
}
