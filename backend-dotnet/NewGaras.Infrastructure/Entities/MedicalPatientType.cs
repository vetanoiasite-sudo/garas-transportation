using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MedicalPatientType")]
public partial class MedicalPatientType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Percentage { get; set; }

    public bool Active { get; set; }

    [InverseProperty("PatientType")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();
}
