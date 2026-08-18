using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryReport")]
public partial class InventoryReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [Column("ByUserID")]
    public long ByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [StringLength(500)]
    public string ReportSubject { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }

    public bool Closed { get; set; }

    public bool Approved { get; set; }

    public bool Active { get; set; }

    [ForeignKey("ByUserId")]
    [InverseProperty("InventoryReports")]
    public virtual User ByUser { get; set; }

    [InverseProperty("InventoryReport")]
    public virtual ICollection<InventoryReportAttachment> InventoryReportAttachments { get; set; } = new List<InventoryReportAttachment>();

    [InverseProperty("InventoryReport")]
    public virtual ICollection<InventoryReportItem> InventoryReportItems { get; set; } = new List<InventoryReportItem>();

    [ForeignKey("InventoryStoreId")]
    [InverseProperty("InventoryReports")]
    public virtual InventoryStore InventoryStore { get; set; }
}
