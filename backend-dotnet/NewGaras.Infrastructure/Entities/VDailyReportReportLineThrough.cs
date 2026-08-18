using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VDailyReportReportLineThrough
{
    [Column("DailyReportID")]
    public long? DailyReportId { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [Column("DailyReportThroughID")]
    public int? DailyReportThroughId { get; set; }

    public bool? New { get; set; }

    public bool? Reviewed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    public long? ReviewedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReprotDate { get; set; }

    [StringLength(500)]
    public string Name { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Column("ID")]
    public long? Id { get; set; }

    [Column("FromTIme")]
    public double? FromTime { get; set; }

    public double? ToTime { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    public string Reason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public string NewClientAddress { get; set; }

    [StringLength(50)]
    public string NewClientTel { get; set; }

    [StringLength(1000)]
    public string NewClientName { get; set; }

    [StringLength(500)]
    public string ContactPerson { get; set; }

    [StringLength(20)]
    public string ContactPersonMobile { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    public int? ReasonTypeId { get; set; }

    [StringLength(1000)]
    public string ReasonTypeName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CustomerSatisfaction { get; set; }
}
