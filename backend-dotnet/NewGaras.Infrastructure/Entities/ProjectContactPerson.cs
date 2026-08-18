using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectContactPerson")]
public partial class ProjectContactPerson
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    [Column("GovernorateID")]
    public int GovernorateId { get; set; }

    [Column("AreaID")]
    public long? AreaId { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [StringLength(500)]
    public string ProjectContactPersonName { get; set; }

    [Required]
    [StringLength(100)]
    public string ProjectContactPersonMobile { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [StringLength(100)]
    public string ProjectContactPersonEmail { get; set; }

    [StringLength(50)]
    public string ProjectContactPersonHomeNum { get; set; }

    [ForeignKey("AreaId")]
    [InverseProperty("ProjectContactPeople")]
    public virtual Area Area { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("ProjectContactPeople")]
    public virtual Country Country { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectContactPersonCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("GovernorateId")]
    [InverseProperty("ProjectContactPeople")]
    public virtual Governorate Governorate { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectContactPersonModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectContactPeople")]
    public virtual Project Project { get; set; }
}
