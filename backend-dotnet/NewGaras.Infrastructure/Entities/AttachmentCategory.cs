using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AttachmentCategory")]
public partial class AttachmentCategory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ProjectTmattachment> ProjectTmattachments { get; set; } = new List<ProjectTmattachment>();

    [InverseProperty("Category")]
    public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();

    [InverseProperty("Category")]
    public virtual ICollection<TaskCommentAttachment> TaskCommentAttachments { get; set; } = new List<TaskCommentAttachment>();
}
