using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskUserReply")]
public partial class TaskUserReply
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Column("RecieverUserID")]
    public long RecieverUserId { get; set; }

    public bool? IsFinished { get; set; }

    public string CommentReply { get; set; }

    public string ApprovalComment { get; set; }

    public bool? Approval { get; set; }

    public string CreatorAttach { get; set; }

    public string ReplyAttach { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskUserReplyCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskUserReplyModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("RecieverUserId")]
    [InverseProperty("TaskUserReplyRecieverUsers")]
    public virtual User RecieverUser { get; set; }
}
