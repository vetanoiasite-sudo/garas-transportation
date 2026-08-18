using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VClientAccountsProject
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyAdjustingEntryID")]
    public long DailyAdjustingEntryId { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("SalesOfferID")]
    public long? SalesOfferId { get; set; }

    public bool? Closed { get; set; }

    public int? Revision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallEndDate { get; set; }

    [StringLength(10)]
    public string InstallDuration { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("ProjectManagerID")]
    public long? ProjectManagerId { get; set; }

    [StringLength(50)]
    public string MaintenanceType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ExtraCost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalCost { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    public bool? ProjectActive { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountType { get; set; }

    [Required]
    [StringLength(50)]
    public string AmountSign { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public bool ClientAccountActive { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }
}
