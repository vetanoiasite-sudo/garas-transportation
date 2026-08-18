using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class MangmentOfMaintenanceRequestAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long MangmentOfMaintenanceRequestId { get; set; }

    [Required]
    public string FilePath { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MangmentOfMaintenanceRequestAttachments")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("MangmentOfMaintenanceRequestId")]
    [InverseProperty("MangmentOfMaintenanceRequestAttachments")]
    public virtual MangmentOfMaintenanceRequest MangmentOfMaintenanceRequest { get; set; }
}
