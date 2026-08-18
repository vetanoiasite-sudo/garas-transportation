using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ReportCCGroup")]
public partial class ReportCcgroup
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ReportID")]
    public int ReportId { get; set; }

    [Column("GroupID")]
    public long GroupId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportCcgroups")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("ReportCcgroups")]
    public virtual Group Group { get; set; }

    [ForeignKey("ReportId")]
    [InverseProperty("ReportCcgroups")]
    public virtual Report Report { get; set; }
}
