using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationVehicle")]
public partial class TransportationVehicle
{
    [Key]
    public int Id { get; set; }

    [Column("vehicleTypeId")]
    public int VehicleTypeId { get; set; }

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("isApproved")]
    public bool IsApproved { get; set; }

    [Column("approvedBy")]
    [StringLength(450)]
    public string ApprovedBy { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    [Column("modifiedDate", TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [Column("modifiedBy")]
    public long ModifiedBy { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationVehicleCreationByNavigations")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TransportationVehicleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("TransportationVehicle")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRoutes { get; set; } = new List<TransportationVehicleRoute>();

    [ForeignKey("VehicleTypeId")]
    [InverseProperty("TransportationVehicles")]
    public virtual VehicleType VehicleType { get; set; }
}
