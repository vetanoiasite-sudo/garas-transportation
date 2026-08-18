using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRCustodyReportAttachment")]
public partial class HrcustodyReportAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CustodyID")]
    public long CustodyId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtension { get; set; }

    [StringLength(250)]
    public string Category { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("HrcustodyReportAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CustodyId")]
    [InverseProperty("HrcustodyReportAttachments")]
    public virtual Hrcustody Custody { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("HrcustodyReportAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
