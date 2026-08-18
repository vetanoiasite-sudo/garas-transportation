using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MedicalReservation")]
public partial class MedicalReservation
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DoctorID")]
    public long DoctorId { get; set; }

    public int Serial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReservationDate { get; set; }

    [Column("PatientID")]
    public long PatientId { get; set; }

    [Column("PatientTypeID")]
    public int PatientTypeId { get; set; }

    [Column("DoctorScheduleID")]
    public long DoctorScheduleId { get; set; }

    [Column("TeamID")]
    public long TeamId { get; set; }

    [Column("RoomID")]
    public long RoomId { get; set; }

    public int Capacity { get; set; }

    public TimeOnly IntervalFrom { get; set; }

    public TimeOnly IntervalTo { get; set; }

    [Column("consultationPrice", TypeName = "decimal(18, 2)")]
    public decimal ConsultationPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ExaminationPrice { get; set; }

    [Required]
    [StringLength(100)]
    public string Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    [Column("ParentID")]
    public long? ParentId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public long ModifiedBy { get; set; }

    public bool Active { get; set; }

    public int? CardNumber { get; set; }

    [Column("PaymentMethodID")]
    public int? PaymentMethodId { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MedicalReservationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("MedicalReservations")]
    public virtual HrUser Doctor { get; set; }

    [ForeignKey("DoctorScheduleId")]
    [InverseProperty("MedicalReservations")]
    public virtual DoctorSchedule DoctorSchedule { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<MedicalReservation> InverseParent { get; set; } = new List<MedicalReservation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MedicalReservationModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("MedicalReservations")]
    public virtual SalesOffer Offer { get; set; }

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual MedicalReservation Parent { get; set; }

    [ForeignKey("PatientId")]
    [InverseProperty("MedicalReservations")]
    public virtual Client Patient { get; set; }

    [ForeignKey("PatientTypeId")]
    [InverseProperty("MedicalReservations")]
    public virtual MedicalPatientType PatientType { get; set; }

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("MedicalReservations")]
    public virtual PaymentMethod PaymentMethod { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("MedicalReservations")]
    public virtual DoctorRoom Room { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("MedicalReservations")]
    public virtual Team Team { get; set; }
}
