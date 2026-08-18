using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Report")]
public partial class Report
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    public bool Active { get; set; }

    public int RepeatEveryNumDay { get; set; }

    [Required]
    public string TaskSubject { get; set; }

    [Required]
    public string TaskBody { get; set; }

    [Required]
    [StringLength(1000)]
    public string ReportCategory { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [InverseProperty("Report")]
    public virtual ICollection<ReportCcgroup> ReportCcgroups { get; set; } = new List<ReportCcgroup>();

    [InverseProperty("Report")]
    public virtual ICollection<ReportCcuser> ReportCcusers { get; set; } = new List<ReportCcuser>();

    [InverseProperty("Report")]
    public virtual ICollection<ReportGroup> ReportGroups { get; set; } = new List<ReportGroup>();

    [InverseProperty("Report")]
    public virtual ICollection<ReportUser> ReportUsers { get; set; } = new List<ReportUser>();

    [InverseProperty("Report")]
    public virtual ICollection<SubmittedReport> SubmittedReports { get; set; } = new List<SubmittedReport>();
}
