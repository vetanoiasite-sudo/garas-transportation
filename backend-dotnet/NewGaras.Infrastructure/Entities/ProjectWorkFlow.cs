using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectWorkFlow")]
public partial class ProjectWorkFlow
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Required]
    [StringLength(450)]
    public string WorkFlowName { get; set; }

    public int OrderNo { get; set; }

    public bool Active { get; set; }

    public long CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreateBy")]
    [InverseProperty("ProjectWorkFlowCreateByNavigations")]
    public virtual User CreateByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectWorkFlowModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectWorkFlows")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectWorkFlow")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
