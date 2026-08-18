using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("CRMContactType")]
public partial class CrmcontactType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CrmcontactTypeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("CrmcontactType")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("CrmcontactTypeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
