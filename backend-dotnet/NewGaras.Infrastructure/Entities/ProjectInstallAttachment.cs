using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallAttachment")]
public partial class ProjectInstallAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

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

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectInstallAttachments")]
    public virtual Project Project { get; set; }
}
