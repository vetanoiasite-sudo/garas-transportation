using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientContactPerson")]
public partial class ClientContactPerson
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Mobile { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientContactPeople")]
    public virtual Client Client { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ClientContactPersonCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("ClientContactPerson")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ClientContactPersonModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
