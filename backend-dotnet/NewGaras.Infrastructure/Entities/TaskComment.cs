using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskComment")]
public partial class TaskComment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("TaskInfoID")]
    public long TaskInfoId { get; set; }

    [Column("ParentCommentID")]
    public int? ParentCommentId { get; set; }

    [Required]
    [StringLength(50)]
    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskCommentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("ParentComment")]
    public virtual ICollection<TaskComment> InverseParentComment { get; set; } = new List<TaskComment>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskCommentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ParentCommentId")]
    [InverseProperty("InverseParentComment")]
    public virtual TaskComment ParentComment { get; set; }

    [InverseProperty("Comment")]
    public virtual ICollection<TaskCommentAttachment> TaskCommentAttachments { get; set; } = new List<TaskCommentAttachment>();

    [ForeignKey("TaskInfoId")]
    [InverseProperty("TaskComments")]
    public virtual TaskInfo TaskInfo { get; set; }
}
