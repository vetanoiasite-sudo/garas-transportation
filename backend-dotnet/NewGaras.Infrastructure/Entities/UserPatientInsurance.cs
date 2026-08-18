using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserPatientInsurance")]
public partial class UserPatientInsurance
{
    [Key]
    public long Id { get; set; }

    public long UserPatientId { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [Required]
    [Column("IncuranceNO")]
    [StringLength(250)]
    public string IncuranceNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireDate { get; set; }

    public bool Active { get; set; }

    public long CreationBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("UserPatientInsuranceCreationByNavigations")]
    public virtual User CreationByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("UserPatientInsuranceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("UserPatientId")]
    [InverseProperty("UserPatientInsurances")]
    public virtual UserPatient UserPatient { get; set; }
}
