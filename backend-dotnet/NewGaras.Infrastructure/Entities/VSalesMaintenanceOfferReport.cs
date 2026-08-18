using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesMaintenanceOfferReport
{
    [Column("MaintenanceSalesOfferID")]
    public long MaintenanceSalesOfferId { get; set; }

    [Column("LinkedSalesOfferID")]
    public long? LinkedSalesOfferId { get; set; }

    [Required]
    [StringLength(50)]
    public string SalesMaintenanceOfferType { get; set; }

    [StringLength(250)]
    public string SalesOfferStatus { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(250)]
    public string ProjectSerial { get; set; }

    public bool? ProjectActive { get; set; }

    [StringLength(50)]
    public string MaintenanceType { get; set; }

    public bool? ProjectClosed { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    public string InventoryItemName { get; set; }

    [StringLength(50)]
    public string InventoryItemCode { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [StringLength(1000)]
    public string InventoryItemCategoryName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PeriodStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PeriodEndDate { get; set; }

    public int? NumberOfVisits { get; set; }

    [Column("CountryID")]
    public int? CountryId { get; set; }

    [StringLength(500)]
    public string CountryName { get; set; }

    [Column("GovernorateID")]
    public int? GovernorateId { get; set; }

    [StringLength(500)]
    public string GovernorateName { get; set; }

    [Column("AreaID")]
    public long? AreaId { get; set; }

    [StringLength(1000)]
    public string AreaName { get; set; }

    public string ProjectContactPersonAddress { get; set; }

    [StringLength(500)]
    public string ProjectContactPersonName { get; set; }

    [StringLength(100)]
    public string ProjectContactPersonMobile { get; set; }

    [Column("ManagementOfMaintenanceOrderID")]
    public long? ManagementOfMaintenanceOrderId { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [Column("SalesPersonBranchID")]
    public int? SalesPersonBranchId { get; set; }
}
