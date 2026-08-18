using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MovementReport")]
public partial class MovementReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MovementAndDeliveryOrderID")]
    public long MovementAndDeliveryOrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    public int? Time { get; set; }

    [StringLength(50)]
    public string TimeInterval { get; set; }

    [StringLength(250)]
    public string ReceivedBy { get; set; }

    [StringLength(500)]
    public string CarStatus { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DriverEvaluation { get; set; }

    [StringLength(500)]
    public string DriverComment { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LeaveDate { get; set; }

    public int? LeaveTime { get; set; }

    [StringLength(50)]
    public string LeaveTimeInterval { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ArivalDate { get; set; }

    public int? ArivalTime { get; set; }

    [StringLength(50)]
    public string ArivalTimeInterval { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MovementReportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MovementReportModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("MovementAndDeliveryOrderId")]
    [InverseProperty("MovementReports")]
    public virtual MovementsAndDeliveryOrder MovementAndDeliveryOrder { get; set; }

    [InverseProperty("MovementReport")]
    public virtual ICollection<MovementReportAttachment> MovementReportAttachments { get; set; } = new List<MovementReportAttachment>();
}
