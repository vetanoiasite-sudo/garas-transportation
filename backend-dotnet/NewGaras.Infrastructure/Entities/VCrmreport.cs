using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VCrmreport
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("CRMUserID")]
    public long CrmuserId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReportDate { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    [Column("ClientContactPersonID")]
    public long ClientContactPersonId { get; set; }

    [StringLength(500)]
    public string ClientContactPersonName { get; set; }

    [StringLength(100)]
    public string ClientContactPersonMobile { get; set; }

    public bool IsNew { get; set; }

    [Column("CRMContactTypeID")]
    public int? CrmcontactTypeId { get; set; }

    [StringLength(250)]
    public string ContactName { get; set; }

    [StringLength(250)]
    public string OtherContactName { get; set; }

    [Column("CRMRecievedTypeID")]
    public int? CrmrecievedTypeId { get; set; }

    [StringLength(250)]
    public string RecievedName { get; set; }

    [StringLength(250)]
    public string OtherRecievedName { get; set; }

    [StringLength(1000)]
    public string ReasonName { get; set; }

    [Required]
    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(500)]
    public string ClientContactPersonLocation { get; set; }

    public bool? SupportedByCompany { get; set; }

    [StringLength(250)]
    public string SupportedBy { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CustomerSatisfaction { get; set; }

    [Column("CRMReportReasonID")]
    public int? CrmreportReasonId { get; set; }
}
