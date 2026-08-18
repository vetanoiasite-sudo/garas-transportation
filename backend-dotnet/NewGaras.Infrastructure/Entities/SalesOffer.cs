using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOffer")]
public partial class SalesOffer
{
    [Key]
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

    [Column(TypeName = "datetime")]
    public DateTime? ClientApprovalDate { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ProductType { get; set; }

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
    public int BranchId { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [StringLength(50)]
    public string ContractType { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    public bool? ClientNeedsDiscount { get; set; }

    [StringLength(250)]
    public string RejectionReason { get; set; }

    public bool? NeedsInvoice { get; set; }

    public bool? NeedsExtraCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OfferExpirationDate { get; set; }

    public int? OfferExpirationPeriod { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ExtraOrDiscountPriceBySalesPerson { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReminderDate { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("SalesOffers")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("SalesOffers")]
    public virtual Client Client { get; set; }

    [InverseProperty("Offer")]
    public virtual ICollection<ClientAccount> ClientAccounts { get; set; } = new List<ClientAccount>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("RelatedToSalesOffer")]
    public virtual ICollection<Crmreport> Crmreports { get; set; } = new List<Crmreport>();

    [InverseProperty("RelatedToSalesOffer")]
    public virtual ICollection<DailyReportLine> DailyReportLines { get; set; } = new List<DailyReportLine>();

    [InverseProperty("ParentSalesOffer")]
    public virtual ICollection<InvoiceCnandDn> InvoiceCnandDnParentSalesOffers { get; set; } = new List<InvoiceCnandDn>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<InvoiceCnandDn> InvoiceCnandDnSalesOffers { get; set; } = new List<InvoiceCnandDn>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<MaintenanceFor> MaintenanceFors { get; set; } = new List<MaintenanceFor>();

    [InverseProperty("MaintenanceOffer")]
    public virtual ICollection<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrders { get; set; } = new List<ManagementOfMaintenanceOrder>();

    [InverseProperty("RentOffer")]
    public virtual ICollection<ManagementOfRentOrder> ManagementOfRentOrders { get; set; } = new List<ManagementOfRentOrder>();

    [InverseProperty("Offer")]
    public virtual ICollection<MedicalReservation> MedicalReservations { get; set; } = new List<MedicalReservation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("SalesOffer")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [InverseProperty("Offer")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [InverseProperty("LinkedSalesOffer")]
    public virtual ICollection<SalesMaintenanceOffer> SalesMaintenanceOfferLinkedSalesOffers { get; set; } = new List<SalesMaintenanceOffer>();

    [InverseProperty("MaintenanceSalesOffer")]
    public virtual ICollection<SalesMaintenanceOffer> SalesMaintenanceOfferMaintenanceSalesOffers { get; set; } = new List<SalesMaintenanceOffer>();

    [InverseProperty("Offer")]
    public virtual ICollection<SalesOfferAttachment> SalesOfferAttachments { get; set; } = new List<SalesOfferAttachment>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferDiscount> SalesOfferDiscounts { get; set; } = new List<SalesOfferDiscount>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferEditHistory> SalesOfferEditHistories { get; set; } = new List<SalesOfferEditHistory>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferExtraCost> SalesOfferExtraCosts { get; set; } = new List<SalesOfferExtraCost>();

    [InverseProperty("Offer")]
    public virtual ICollection<SalesOfferGroupPermission> SalesOfferGroupPermissions { get; set; } = new List<SalesOfferGroupPermission>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferInternalApproval> SalesOfferInternalApprovals { get; set; } = new List<SalesOfferInternalApproval>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferInvoiceTax> SalesOfferInvoiceTaxes { get; set; } = new List<SalesOfferInvoiceTax>();

    [InverseProperty("Offer")]
    public virtual ICollection<SalesOfferItemAttachment> SalesOfferItemAttachments { get; set; } = new List<SalesOfferItemAttachment>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferLocation> SalesOfferLocations { get; set; } = new List<SalesOfferLocation>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferPdfTemplate> SalesOfferPdfTemplates { get; set; } = new List<SalesOfferPdfTemplate>();

    [InverseProperty("Offer")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProducts { get; set; } = new List<SalesOfferProduct>();

    [InverseProperty("SalesOffer")]
    public virtual ICollection<SalesOfferTermsAndCondition> SalesOfferTermsAndConditions { get; set; } = new List<SalesOfferTermsAndCondition>();

    [InverseProperty("Offer")]
    public virtual ICollection<SalesOfferUserPermission> SalesOfferUserPermissions { get; set; } = new List<SalesOfferUserPermission>();

    [ForeignKey("SalesPersonId")]
    [InverseProperty("SalesOfferSalesPeople")]
    public virtual User SalesPerson { get; set; }

    [InverseProperty("SalesOffer")]
    public virtual ICollection<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistories { get; set; } = new List<VehicleMaintenanceJobOrderHistory>();

    [InverseProperty("Offer")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenances { get; set; } = new List<VisitsScheduleOfMaintenance>();
}
