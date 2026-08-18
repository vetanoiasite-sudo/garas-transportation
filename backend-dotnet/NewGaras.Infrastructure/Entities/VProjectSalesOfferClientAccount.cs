using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectSalesOfferClientAccount
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [StringLength(250)]
    public string Status { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? OfferAmount { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [StringLength(20)]
    public string ContactPersonMobile { get; set; }

    [StringLength(50)]
    public string ContactPersonEmail { get; set; }

    [StringLength(500)]
    public string ContactPersonName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProjectStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProjectEndDate { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(50)]
    public string ContractType { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    public bool Closed { get; set; }

    public int Revision { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallEndDate { get; set; }

    [StringLength(10)]
    public string InstallDuration { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

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

    public bool ProjectActive { get; set; }

    public long? ClientAccountId { get; set; }

    [Column("DailyAdjustingEntryID")]
    public long? DailyAdjustingEntryId { get; set; }

    public long? AccountClientId { get; set; }

    [Column("AccountID")]
    public long? AccountId { get; set; }

    [StringLength(50)]
    public string AccountType { get; set; }

    [StringLength(50)]
    public string AmountSign { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public string Description { get; set; }

    public bool? AccountActive { get; set; }
}
