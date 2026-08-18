using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PaymentMethod")]
public partial class PaymentMethod
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<ProjectInvoiceCollected> ProjectInvoiceCollecteds { get; set; } = new List<ProjectInvoiceCollected>();

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
