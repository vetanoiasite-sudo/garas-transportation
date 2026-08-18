using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("POApprovalSetting")]
public partial class PoapprovalSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(400)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public bool Mandatory { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(250)]
    public string CreationBy { get; set; }

    [Required]
    [StringLength(250)]
    public string ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    [InverseProperty("PoapprovalSetting")]
    public virtual ICollection<PoapprovalStatus> PoapprovalStatuses { get; set; } = new List<PoapprovalStatus>();

    [InverseProperty("PoapprovalSetting")]
    public virtual ICollection<PoapprovalUser> PoapprovalUsers { get; set; } = new List<PoapprovalUser>();
}
