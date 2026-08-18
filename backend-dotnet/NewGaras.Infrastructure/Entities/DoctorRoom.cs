using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DoctorRoom")]
public partial class DoctorRoom
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("DoctorRooms")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DoctorRooms")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("Room")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();
}
