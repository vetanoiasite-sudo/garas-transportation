using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MaintenanceFor")]
public partial class MaintenanceFor
{
    [Key]
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

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    public string GeneralNote { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProductionDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InstallationDate { get; set; }

    [StringLength(250)]
    public string ContractNumber { get; set; }

    [Column("PRNumber")]
    [StringLength(250)]
    public string Prnumber { get; set; }

    [StringLength(100)]
    public string Stops { get; set; }

    [StringLength(100)]
    public string Capacity { get; set; }

    public int? NumberOfVisits { get; set; }

    public int? NumberOfWarrantyYears { get; set; }

    [StringLength(150)]
    public string CountryOfOrigin { get; set; }

    [StringLength(250)]
    public string MachineMadeInCountry { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("MaintenanceFors")]
    public virtual InventoryItemCategory Category { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("MaintenanceFors")]
    public virtual Client Client { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("MaintenanceFors")]
    public virtual InventoryItem InventoryItem { get; set; }

    [InverseProperty("MaintenanceFor")]
    public virtual ICollection<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrders { get; set; } = new List<ManagementOfMaintenanceOrder>();

    [InverseProperty("MaintenanceFor")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequests { get; set; } = new List<MangmentOfMaintenanceRequest>();

    [ForeignKey("ProjectId")]
    [InverseProperty("MaintenanceFors")]
    public virtual Project Project { get; set; }

    [InverseProperty("MaintenanceFor")]
    public virtual ICollection<ProjectCheque> ProjectCheques { get; set; } = new List<ProjectCheque>();

    [ForeignKey("SalesOfferId")]
    [InverseProperty("MaintenanceFors")]
    public virtual SalesOffer SalesOffer { get; set; }

    [InverseProperty("MaintenanceFor")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProducts { get; set; } = new List<SalesOfferProduct>();

    [InverseProperty("MaintenanceFor")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenances { get; set; } = new List<VisitsScheduleOfMaintenance>();
}
