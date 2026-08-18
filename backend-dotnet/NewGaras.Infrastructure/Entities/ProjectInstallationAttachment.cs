using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallationAttachment")]
public partial class ProjectInstallationAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectInstallationID")]
    public long ProjectInstallationId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtenssion { get; set; }

    [Required]
    [StringLength(250)]
    public string Category { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallationAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectInstallationId")]
    [InverseProperty("ProjectInstallationAttachments")]
    public virtual ProjectInstallation ProjectInstallation { get; set; }
}
