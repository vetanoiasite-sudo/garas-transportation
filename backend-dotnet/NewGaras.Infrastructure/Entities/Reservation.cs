using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Reservation
{
    [Key]
    public int Id { get; set; }

    [Column("reservationDate")]
    public DateTime? ReservationDate { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public long ClientId { get; set; }

    [Required]
    public string Provider { get; set; }

    public bool Confirmation { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalCost { get; set; }

    public bool IsFinished { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPaid { get; set; }

    public long OfferId { get; set; }

    [StringLength(100)]
    public string Type { get; set; }

    [Column("ParentID")]
    public int? ParentId { get; set; }

    [StringLength(500)]
    public string Notice { get; set; }

    [StringLength(255)]
    public string StatusOfReservation { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<Childern> Childerns { get; set; } = new List<Childern>();

    [ForeignKey("ClientId")]
    [InverseProperty("Reservations")]
    public virtual Client Client { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<Collect> Collects { get; set; } = new List<Collect>();

    [InverseProperty("Reservation")]
    public virtual ICollection<ExpensessStatus> ExpensessStatuses { get; set; } = new List<ExpensessStatus>();

    [InverseProperty("Parent")]
    public virtual ICollection<Reservation> InverseParent { get; set; } = new List<Reservation>();

    [ForeignKey("OfferId")]
    [InverseProperty("Reservations")]
    public virtual SalesOffer Offer { get; set; }

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Reservation Parent { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<ReservationInvoice> ReservationInvoices { get; set; } = new List<ReservationInvoice>();

    [InverseProperty("Reservation")]
    public virtual ICollection<RoomsReservationChildern> RoomsReservationChilderns { get; set; } = new List<RoomsReservationChildern>();

    [InverseProperty("Reservation")]
    public virtual ICollection<RoomsReservationMeal> RoomsReservationMeals { get; set; } = new List<RoomsReservationMeal>();

    [InverseProperty("Reservation")]
    public virtual ICollection<RoomsReservation> RoomsReservations { get; set; } = new List<RoomsReservation>();

    [InverseProperty("Reservation")]
    public virtual ICollection<StatusReservation> StatusReservations { get; set; } = new List<StatusReservation>();
}
