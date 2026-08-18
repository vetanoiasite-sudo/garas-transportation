using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportionLineExceptionPrice")]
public partial class TransportionLineExceptionPrice
{
    [Key]
    public int Id { get; set; }

    public long RouteId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FromDate { get; set; }
}
