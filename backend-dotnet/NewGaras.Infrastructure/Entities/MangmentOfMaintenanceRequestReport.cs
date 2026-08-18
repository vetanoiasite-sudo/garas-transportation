using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MangmentOfMaintenanceRequestReport")]
public partial class MangmentOfMaintenanceRequestReport
{
    [Key]
    public long Id { get; set; }

    public long MangmentOfMaintenanceRequestId { get; set; }

    public long ReportedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReportDate { get; set; }

    public long ClientId { get; set; }

    public string Description { get; set; }

    public bool? Finished { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinishedDate { get; set; }

    public long? FinishedBy { get; set; }

    public string FinisehdImgPath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    public string InstallationTeamSignature { get; set; }

    public string InstallationLeaderSignature { get; set; }

    public string ReportImage { get; set; }

    [StringLength(250)]
    public string WorkerName { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("MangmentOfMaintenanceRequestReports")]
    public virtual Client Client { get; set; }

    [ForeignKey("FinishedBy")]
    [InverseProperty("MangmentOfMaintenanceRequestReportFinishedByNavigations")]
    public virtual User FinishedByNavigation { get; set; }

    [ForeignKey("MangmentOfMaintenanceRequestId")]
    [InverseProperty("MangmentOfMaintenanceRequestReports")]
    public virtual MangmentOfMaintenanceRequest MangmentOfMaintenanceRequest { get; set; }

    [InverseProperty("MangmentOfMaintenanceRequestReport")]
    public virtual ICollection<MangmentOfMaintenanceRequestReportAttachment> MangmentOfMaintenanceRequestReportAttachments { get; set; } = new List<MangmentOfMaintenanceRequestReportAttachment>();

    [ForeignKey("ReportedBy")]
    [InverseProperty("MangmentOfMaintenanceRequestReportReportedByNavigations")]
    public virtual User ReportedByNavigation { get; set; }
}
