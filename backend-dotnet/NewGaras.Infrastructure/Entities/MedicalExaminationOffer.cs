using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MedicalExaminationOffer")]
public partial class MedicalExaminationOffer
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DoctorID")]
    public long DoctorId { get; set; }

    [Required]
    public string OfferName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Percentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("MedicalExaminationOffers")]
    public virtual HrUser Doctor { get; set; }
}
