using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ExpensessStatus
{
    [Key]
    public int Id { get; set; }

    public int Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PriceOfUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalCost { get; set; }

    public DateTime? Date { get; set; }

    public long? CreatedBy { get; set; }

    public int ReservationId { get; set; }

    public int TypeServicesId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("ExpensessStatuses")]
    public virtual Reservation Reservation { get; set; }

    [ForeignKey("TypeServicesId")]
    [InverseProperty("ExpensessStatuses")]
    public virtual TypeService TypeServices { get; set; }
}
