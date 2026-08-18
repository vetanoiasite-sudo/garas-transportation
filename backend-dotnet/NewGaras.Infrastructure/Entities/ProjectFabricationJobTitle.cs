using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectFabricationJobTitle")]
public partial class ProjectFabricationJobTitle
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("JobTitleID")]
    public int JobTitleId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public int DisplyOrder { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationJobTitleCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("ProjectFabricationJobTitles")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectFabricationJobTitleModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
