using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VMaintenanceHoursReport
{
    [Column("MaintenanceReportUsersID")]
    public long MaintenanceReportUsersId { get; set; }

    [Column("MaintenanceReportID")]
    public long MaintenanceReportId { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [StringLength(500)]
    public string BranchName { get; set; }

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [StringLength(500)]
    public string DepartmentName { get; set; }

    [Column("JobTitleID")]
    public int JobTitleId { get; set; }

    [StringLength(500)]
    public string JobTitleName { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [StringLength(101)]
    public string UserName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HourNum { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Evaluation { get; set; }

    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReportDate { get; set; }

    [Column("ManagementOfMaintenanceOrderID")]
    public long? ManagementOfMaintenanceOrderId { get; set; }

    [StringLength(100)]
    public string VisitSerial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlannedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VisitDate { get; set; }

    public bool? VisitStatus { get; set; }

    public bool? VisitActive { get; set; }

    [Column("MaintenanceOfferID")]
    public long? MaintenanceOfferId { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    public int? NumberOfVisits { get; set; }

    public bool? ManagementOfMaintenanceOrderActive { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [Column("SalesPersonBranchID")]
    public int? SalesPersonBranchId { get; set; }
}
