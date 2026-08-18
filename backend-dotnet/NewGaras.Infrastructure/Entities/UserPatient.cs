using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserPatient")]
public partial class UserPatient
{
    [Key]
    public long Id { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateOfBirth { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public int? CountryId { get; set; }

    public int? CityId { get; set; }

    public long? AreaId { get; set; }

    [StringLength(250)]
    public string Address { get; set; }

    [StringLength(520)]
    public string Description { get; set; }

    [StringLength(50)]
    public string Mobile { get; set; }

    [StringLength(50)]
    public string LandLine { get; set; }

    [ForeignKey("AreaId")]
    [InverseProperty("UserPatients")]
    public virtual Area Area { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("UserPatients")]
    public virtual Governorate City { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("UserPatients")]
    public virtual Country Country { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserPatientCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("UserPatientModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserPatientUsers")]
    public virtual User User { get; set; }

    [InverseProperty("UserPatient")]
    public virtual ICollection<UserPatientInsurance> UserPatientInsurances { get; set; } = new List<UserPatientInsurance>();
}
