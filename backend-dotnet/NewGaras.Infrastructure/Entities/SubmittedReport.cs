using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SubmittedReport")]
public partial class SubmittedReport
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ReportID")]
    public int ReportId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public string Note { get; set; }

    public string AttachmentPath { get; set; }

    [Column("FIleName")]
    [StringLength(500)]
    public string FileName { get; set; }

    [StringLength(5)]
    public string FileExtenssion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SubmittedReportCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ReportId")]
    [InverseProperty("SubmittedReports")]
    public virtual Report Report { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SubmittedReportUsers")]
    public virtual User User { get; set; }
}
