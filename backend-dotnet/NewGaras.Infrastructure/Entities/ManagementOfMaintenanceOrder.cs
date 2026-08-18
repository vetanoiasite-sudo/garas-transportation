using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ManagementOfMaintenanceOrder")]
public partial class ManagementOfMaintenanceOrder
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MaintenanceOfferID")]
    public long MaintenanceOfferId { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    public int NumberOfVisits { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? ContractPrice { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? RateToLocalCu { get; set; }

    public string ContractAttachment { get; set; }

    public string WarrentyCertificateAttachment { get; set; }

    public string ContractStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column("MaintenanceForID")]
    public long? MaintenanceForId { get; set; }

    [StringLength(250)]
    public string ContractType { get; set; }

    [StringLength(250)]
    public string ClosingContractType { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ClosingMileageCounter { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? CurrentMileageCounter { get; set; }

    public string ClosingReason { get; set; }

    [StringLength(250)]
    public string ContractNumber { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ManagementOfMaintenanceOrderCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("ManagementOfMaintenanceOrders")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("MaintenanceForId")]
    [InverseProperty("ManagementOfMaintenanceOrders")]
    public virtual MaintenanceFor MaintenanceFor { get; set; }

    [ForeignKey("MaintenanceOfferId")]
    [InverseProperty("ManagementOfMaintenanceOrders")]
    public virtual SalesOffer MaintenanceOffer { get; set; }

    [InverseProperty("ManagementOfMaintenanceOrder")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequests { get; set; } = new List<MangmentOfMaintenanceRequest>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ManagementOfMaintenanceOrderModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ManagementOfMaintenanceOrders")]
    public virtual Project Project { get; set; }

    [InverseProperty("MaintenanceOrder")]
    public virtual ICollection<ProjectCheque> ProjectCheques { get; set; } = new List<ProjectCheque>();

    [InverseProperty("ManagementOfMaintenanceOrder")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenances { get; set; } = new List<VisitsScheduleOfMaintenance>();
}
