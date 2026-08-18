using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VVisitsMaintenanceReportUser
{
    [Column("ID")]
    public long? Id { get; set; }

    [StringLength(100)]
    public string Serial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlannedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VisitDate { get; set; }

    public bool? Status { get; set; }

    [Column("MaintenanceForID")]
    public long? MaintenanceForId { get; set; }

    public bool? Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReportDate { get; set; }

    [Column("MaintenanceReportID")]
    public long MaintenanceReportId { get; set; }

    [Column("MainReportUsersID")]
    public long MainReportUsersId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HourNum { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Evaluation { get; set; }

    [StringLength(100)]
    public string ClientAddress { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CollectedAmount { get; set; }

    public bool? Finished { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [Column("MaintenanceOfferID")]
    public long? MaintenanceOfferId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? ClientSatisfactionRate { get; set; }
}
