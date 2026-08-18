using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskClosureLog")]
public partial class TaskClosureLog
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskInfoID")]
    public long TaskInfoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ClosureDate { get; set; }

    public string ClosureDescreption { get; set; }

    [StringLength(50)]
    public string TaskTotalTime { get; set; }

    [StringLength(1000)]
    public string RejectComment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskClosureLogCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskClosureLogModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskInfoId")]
    [InverseProperty("TaskClosureLogs")]
    public virtual TaskInfo TaskInfo { get; set; }
}
