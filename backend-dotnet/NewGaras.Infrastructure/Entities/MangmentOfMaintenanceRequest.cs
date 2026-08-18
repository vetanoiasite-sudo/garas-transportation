using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MangmentOfMaintenanceRequest")]
public partial class MangmentOfMaintenanceRequest
{
    [Key]
    public long Id { get; set; }

    public long? ManagementOfMaintenanceOrderId { get; set; }

    public int MaintenanceRequestTypeId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public string CreatorNote { get; set; }

    public long? InstallationAssignTo { get; set; }

    public bool? InstallationDone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallationDate { get; set; }

    public string InstallationNote { get; set; }

    public bool? PreparingDone { get; set; }

    public long? PreparingUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PreparingDate { get; set; }

    public string PreparingNote { get; set; }

    public string PreparingFilePath { get; set; }

    public long? MaintenanceForId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    public bool? Accept { get; set; }

    public string RequestFilePath { get; set; }

    public string Description { get; set; }

    public string FinisehdImgPath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    public string InstallationTeamSignature { get; set; }

    public string InstallationLeaderSignature { get; set; }

    public string ReportImage { get; set; }

    [StringLength(250)]
    public string WorkerName { get; set; }

    public long? ClientId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinishedDate { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("MangmentOfMaintenanceRequests")]
    public virtual Client Client { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MangmentOfMaintenanceRequestCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InstallationAssignTo")]
    [InverseProperty("MangmentOfMaintenanceRequestInstallationAssignToNavigations")]
    public virtual User InstallationAssignToNavigation { get; set; }

    [ForeignKey("MaintenanceForId")]
    [InverseProperty("MangmentOfMaintenanceRequests")]
    public virtual MaintenanceFor MaintenanceFor { get; set; }

    [ForeignKey("MaintenanceRequestTypeId")]
    [InverseProperty("MangmentOfMaintenanceRequests")]
    public virtual MaintenanceRequestType MaintenanceRequestType { get; set; }

    [ForeignKey("ManagementOfMaintenanceOrderId")]
    [InverseProperty("MangmentOfMaintenanceRequests")]
    public virtual ManagementOfMaintenanceOrder ManagementOfMaintenanceOrder { get; set; }

    [InverseProperty("MangmentOfMaintenanceRequest")]
    public virtual ICollection<MangmentOfMaintenanceRequestAttachment> MangmentOfMaintenanceRequestAttachments { get; set; } = new List<MangmentOfMaintenanceRequestAttachment>();

    [InverseProperty("MangmentOfMaintenanceRequest")]
    public virtual ICollection<MangmentOfMaintenanceRequestReport> MangmentOfMaintenanceRequestReports { get; set; } = new List<MangmentOfMaintenanceRequestReport>();

    [ForeignKey("PreparingUserId")]
    [InverseProperty("MangmentOfMaintenanceRequestPreparingUsers")]
    public virtual User PreparingUser { get; set; }
}
