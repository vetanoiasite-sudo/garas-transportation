using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Speciality")]
public partial class Speciality
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("Speciality")]
    public virtual ICollection<ClientConsultantSpecialilty> ClientConsultantSpecialilties { get; set; } = new List<ClientConsultantSpecialilty>();

    [InverseProperty("Speciality")]
    public virtual ICollection<ClientSpeciality> ClientSpecialities { get; set; } = new List<ClientSpeciality>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("SpecialityCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SpecialityModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
