using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectFabricationTotalHour
{
    [Column("ProjectFabID")]
    public long ProjectFabId { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? TotalHours { get; set; }
}
