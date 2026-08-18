using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Project")]
public partial class Project
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

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

    [Column("BranchID")]
    public int BranchId { get; set; }

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

    public bool Active { get; set; }

    [Column("projectDescription")]
    public string ProjectDescription { get; set; }

    public bool? Billable { get; set; }

    public bool? TimeTracking { get; set; }

    [Column("PriortyID")]
    public int? PriortyId { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal? Budget { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    public bool? FromTaskManger { get; set; }

    public bool? AllowProjectChatting { get; set; }

    public bool? AllowTaskScreenMonitoring { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? BillingFactor { get; set; }

    [Column("BillingTypeID")]
    public int? BillingTypeId { get; set; }

    [Column("CostTypeID")]
    public int? CostTypeId { get; set; }

    public bool? MoveBySequenceTask { get; set; }

    public bool? MoveBySequenceType { get; set; }

    public bool? UnitRateService { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("BillingTypeId")]
    [InverseProperty("Projects")]
    public virtual BillingType BillingType { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Projects")]
    public virtual Branch Branch { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ClientAccount> ClientAccounts { get; set; } = new List<ClientAccount>();

    [ForeignKey("CostTypeId")]
    [InverseProperty("Projects")]
    public virtual ProjectCostingType CostType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("Projects")]
    public virtual Currency Currency { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<DailyTransactionCostCenter> DailyTransactionCostCenters { get; set; } = new List<DailyTransactionCostCenter>();

    [InverseProperty("Project")]
    public virtual ICollection<InventoryMatrialReleasePrintInfo> InventoryMatrialReleasePrintInfos { get; set; } = new List<InventoryMatrialReleasePrintInfo>();

    [InverseProperty("Project")]
    public virtual ICollection<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; } = new List<InventoryMatrialRequestItem>();

    [InverseProperty("Project")]
    public virtual ICollection<InvoiceExtraModification> InvoiceExtraModifications { get; set; } = new List<InvoiceExtraModification>();

    [InverseProperty("Project")]
    public virtual ICollection<InvoiceOfProject> InvoiceOfProjects { get; set; } = new List<InvoiceOfProject>();

    [InverseProperty("Project")]
    public virtual ICollection<MaintenanceFor> MaintenanceFors { get; set; } = new List<MaintenanceFor>();

    [InverseProperty("Project")]
    public virtual ICollection<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrders { get; set; } = new List<ManagementOfMaintenanceOrder>();

    [InverseProperty("Project")]
    public virtual ICollection<ManagementOfRentOrder> ManagementOfRentOrders { get; set; } = new List<ManagementOfRentOrder>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PriortyId")]
    [InverseProperty("Projects")]
    public virtual Priority Priorty { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ProjectAssignUser> ProjectAssignUsers { get; set; } = new List<ProjectAssignUser>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectAttachment> ProjectAttachments { get; set; } = new List<ProjectAttachment>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectCheque> ProjectCheques { get; set; } = new List<ProjectCheque>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectContactPerson> ProjectContactPeople { get; set; } = new List<ProjectContactPerson>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectFabrication> ProjectFabrications { get; set; } = new List<ProjectFabrication>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectInstallAttachment> ProjectInstallAttachments { get; set; } = new List<ProjectInstallAttachment>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectInstallation> ProjectInstallations { get; set; } = new List<ProjectInstallation>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectInvoice> ProjectInvoices { get; set; } = new List<ProjectInvoice>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectLetterOfCredit> ProjectLetterOfCredits { get; set; } = new List<ProjectLetterOfCredit>();

    [ForeignKey("ProjectManagerId")]
    [InverseProperty("ProjectProjectManagers")]
    public virtual User ProjectManager { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ProjectPaymentTerm> ProjectPaymentTerms { get; set; } = new List<ProjectPaymentTerm>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectSprint> ProjectSprints { get; set; } = new List<ProjectSprint>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectWorkFlow> ProjectWorkFlows { get; set; } = new List<ProjectWorkFlow>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectWorkshopStation> ProjectWorkshopStations { get; set; } = new List<ProjectWorkshopStation>();

    [InverseProperty("Project")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [ForeignKey("SalesOfferId")]
    [InverseProperty("Projects")]
    public virtual SalesOffer SalesOffer { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [InverseProperty("JobOrderProject")]
    public virtual ICollection<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistories { get; set; } = new List<VehicleMaintenanceJobOrderHistory>();

    [InverseProperty("Project")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackings { get; set; } = new List<WorkingHourseTracking>();
}
