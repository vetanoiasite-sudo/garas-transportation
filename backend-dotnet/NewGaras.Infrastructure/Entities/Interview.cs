using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Interview")]
public partial class Interview
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("InterviewerID")]
    public long InterviewerId { get; set; }

    [Column("JobTitleID")]
    public int JobTitleId { get; set; }

    [Column("InterviewResultID")]
    public int? InterviewResultId { get; set; }

    public string Comment { get; set; }

    [StringLength(1000)]
    public string AttatchmentPath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InterviewCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InterviewerId")]
    [InverseProperty("InterviewInterviewers")]
    public virtual User Interviewer { get; set; }

    [ForeignKey("JobTitleId")]
    [InverseProperty("Interviews")]
    public virtual JobTitle JobTitle { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InterviewModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("InterviewUsers")]
    public virtual User User { get; set; }
}
