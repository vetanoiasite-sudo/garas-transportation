using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskAttachment")]
public partial class TaskAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskInfoID")]
    public long TaskInfoId { get; set; }

    [Required]
    [StringLength(500)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtension { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    [StringLength(250)]
    public string Description { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("TaskAttachments")]
    public virtual AttachmentCategory Category { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskInfoId")]
    [InverseProperty("TaskAttachments")]
    public virtual TaskInfo TaskInfo { get; set; }
}
