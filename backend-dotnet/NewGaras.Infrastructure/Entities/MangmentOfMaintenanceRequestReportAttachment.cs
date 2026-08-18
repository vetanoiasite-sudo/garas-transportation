using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class MangmentOfMaintenanceRequestReportAttachment
{
    [Key]
    public long Id { get; set; }

    public long MangmentOfMaintenanceRequestReportId { get; set; }

    [Required]
    public string FilePath { get; set; }

    [StringLength(250)]
    public string FileName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MangmentOfMaintenanceRequestReportAttachments")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("MangmentOfMaintenanceRequestReportId")]
    [InverseProperty("MangmentOfMaintenanceRequestReportAttachments")]
    public virtual MangmentOfMaintenanceRequestReport MangmentOfMaintenanceRequestReport { get; set; }
}
