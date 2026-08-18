using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectSalesOffer
{
    [Column("ID")]
    public long Id { get; set; }

    public bool Closed { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

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

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column("ProjectManagerID")]
    public long? ProjectManagerId { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? OfferAmount { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(50)]
    public string MaintenanceType { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ExtraCost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalCost { get; set; }

    [Column("MaintenanceSalesOfferID")]
    public long? MaintenanceSalesOfferId { get; set; }

    [Column("LinkedSalesOfferID")]
    public long? LinkedSalesOfferId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ExtraOrDiscountPriceBySalesPerson { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    public bool? NeedsInvoice { get; set; }

    public bool Active { get; set; }

    [Column("SalesPersonBranchID")]
    public int? SalesPersonBranchId { get; set; }
}
