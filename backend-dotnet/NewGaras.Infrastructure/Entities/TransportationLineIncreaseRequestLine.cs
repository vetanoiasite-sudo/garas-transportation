using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class TransportationLineIncreaseRequestLine
{
    [Key]
    public long Id { get; set; }

    public int TransportationLineIncreaseRequestId { get; set; }

    public long RouteId { get; set; }

    [ForeignKey("RouteId")]
    [InverseProperty("TransportationLineIncreaseRequestLines")]
    public virtual TransportationVehicleRoute Route { get; set; }

    [ForeignKey("TransportationLineIncreaseRequestId")]
    [InverseProperty("TransportationLineIncreaseRequestLines")]
    public virtual TransportationLineIncreaseRequest TransportationLineIncreaseRequest { get; set; }
}
