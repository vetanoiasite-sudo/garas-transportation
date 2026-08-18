using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VMaintenanceForDetail
{
    [Column("ID")]
    public long Id { get; set; }

    public string ProductType { get; set; }

    public string ProductSerial { get; set; }

    public int? NumOfVisitsPerProduct { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("FabOrderID")]
    public int? FabOrderId { get; set; }

    public string ProductName { get; set; }

    public string ProductBrand { get; set; }

    public string ProductFabricator { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [Column("VichealID")]
    public long? VichealId { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    [Required]
    [StringLength(500)]
    public string ClientName { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ContractStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ContractEndDate { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? ContractPrice { get; set; }

    public int? NumberOfVisits { get; set; }

    [Column("ManagementOfMaintenanceOrderID")]
    public long? ManagementOfMaintenanceOrderId { get; set; }

    [Column("VisitOfMaintenanceID")]
    public long? VisitOfMaintenanceId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlannedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VisitDate { get; set; }

    public bool? VisitsScheduleOfMaintenanceStatus { get; set; }

    public bool? VisitsScheduleOfMaintenanceActive { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }
}
