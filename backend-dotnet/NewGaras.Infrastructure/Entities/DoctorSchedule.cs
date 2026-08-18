using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DoctorSchedule")]
public partial class DoctorSchedule
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DoctorID")]
    public long DoctorId { get; set; }

    public int Capacity { get; set; }

    public TimeOnly IntervalFrom { get; set; }

    public TimeOnly IntervalTo { get; set; }

    [Column("consultationPrice", TypeName = "decimal(18, 2)")]
    public decimal ConsultationPrice { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    public long? RoomId { get; set; }

    [Column("percentageTypeID")]
    public int PercentageTypeId { get; set; }

    [Column("DoctorSpecialityID")]
    public long DoctorSpecialityId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ExaminationPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public long ModifiedBy { get; set; }

    public int WeekDayId { get; set; }

    public int? HoldQuantity { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("DoctorSchedules")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DoctorScheduleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("DoctorSchedules")]
    public virtual HrUser Doctor { get; set; }

    [ForeignKey("DoctorSpecialityId")]
    [InverseProperty("DoctorSchedules")]
    public virtual Team DoctorSpeciality { get; set; }

    [InverseProperty("DoctorSchedule")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DoctorScheduleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PercentageTypeId")]
    [InverseProperty("DoctorSchedules")]
    public virtual MedicalDoctorPercentageType PercentageType { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("DoctorSchedules")]
    public virtual DoctorRoom Room { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("DoctorSchedules")]
    public virtual MedicalDoctorScheduleStatus Status { get; set; }

    [ForeignKey("WeekDayId")]
    [InverseProperty("DoctorSchedules")]
    public virtual WeekDay WeekDay { get; set; }
}
