using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("CRMReport")]
public partial class Crmreport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CRMUserID")]
    public long CrmuserId { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReportDate { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    [Column("ClientContactPersonID")]
    public long ClientContactPersonId { get; set; }

    public bool IsNew { get; set; }

    [Column("CRMContactTypeID")]
    public int? CrmcontactTypeId { get; set; }

    [StringLength(250)]
    public string OtherContactName { get; set; }

    [Column("CRMRecievedTypeID")]
    public int? CrmrecievedTypeId { get; set; }

    [StringLength(250)]
    public string OtherRecievedName { get; set; }

    [Column("CRMReportReasonID")]
    public int? CrmreportReasonId { get; set; }

    [Required]
    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CustomerSatisfaction { get; set; }

    public long? DailyReportId { get; set; }

    public long? DailyReportLineId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReminderDate { get; set; }

    public long? RelatedToInventoryItemId { get; set; }

    public long? RelatedToSalesOfferProductId { get; set; }

    public long? RelatedToSalesOfferId { get; set; }

    public string Hint { get; set; }

    public bool? ReminderIsClosed { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Crmreports")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("Crmreports")]
    public virtual Client Client { get; set; }

    [ForeignKey("ClientContactPersonId")]
    [InverseProperty("Crmreports")]
    public virtual ClientContactPerson ClientContactPerson { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CrmreportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CrmcontactTypeId")]
    [InverseProperty("Crmreports")]
    public virtual CrmcontactType CrmcontactType { get; set; }

    [ForeignKey("CrmrecievedTypeId")]
    [InverseProperty("Crmreports")]
    public virtual CrmrecievedType CrmrecievedType { get; set; }

    [ForeignKey("CrmreportReasonId")]
    [InverseProperty("Crmreports")]
    public virtual CrmreportReason CrmreportReason { get; set; }

    [ForeignKey("CrmuserId")]
    [InverseProperty("CrmreportCrmusers")]
    public virtual User Crmuser { get; set; }

    [ForeignKey("DailyReportId")]
    [InverseProperty("Crmreports")]
    public virtual DailyReport DailyReport { get; set; }

    [ForeignKey("DailyReportLineId")]
    [InverseProperty("Crmreports")]
    public virtual DailyReportLine DailyReportLine { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("CrmreportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("RelatedToInventoryItemId")]
    [InverseProperty("Crmreports")]
    public virtual InventoryItem RelatedToInventoryItem { get; set; }

    [ForeignKey("RelatedToSalesOfferId")]
    [InverseProperty("Crmreports")]
    public virtual SalesOffer RelatedToSalesOffer { get; set; }

    [ForeignKey("RelatedToSalesOfferProductId")]
    [InverseProperty("Crmreports")]
    public virtual SalesOfferProduct RelatedToSalesOfferProduct { get; set; }
}
