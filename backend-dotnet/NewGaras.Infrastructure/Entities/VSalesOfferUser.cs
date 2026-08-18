using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesOfferUser
{
    [Column("ID")]
    public long Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Note { get; set; }

    [Column("SalesPersonID")]
    public long SalesPersonId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [StringLength(250)]
    public string Status { get; set; }

    public bool Completed { get; set; }

    public string TechnicalInfo { get; set; }

    public string ProjectData { get; set; }

    public string FinancialInfo { get; set; }

    public string PricingComment { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? OfferAmount { get; set; }

    public bool? SendingOfferConfirmation { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SendingOfferDate { get; set; }

    [StringLength(500)]
    public string SendingOfferBy { get; set; }

    [StringLength(1000)]
    public string SendingOfferTo { get; set; }

    public string SendingOfferComment { get; set; }

    public bool? ClientApprove { get; set; }

    public string ClientComment { get; set; }

    public int? VersionNumber { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClientApprovalDate { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OfferExpirationDate { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReminderDate { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }
}
