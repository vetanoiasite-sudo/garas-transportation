using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Email")]
public partial class Email
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [Column("EmailID")]
    [StringLength(450)]
    public string EmailId { get; set; }

    [Required]
    public string EmailBody { get; set; }

    [Required]
    public string EmailSubject { get; set; }

    [Required]
    [StringLength(150)]
    public string SenderEmail { get; set; }

    [Required]
    [StringLength(250)]
    public string SenderName { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool HasAttachment { get; set; }

    public int EmailType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceivedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public string EmailComment { get; set; }

    [InverseProperty("Email")]
    public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = new List<EmailAttachment>();

    [InverseProperty("Email")]
    public virtual ICollection<EmailCategory> EmailCategories { get; set; } = new List<EmailCategory>();

    [InverseProperty("EmailNavigation")]
    public virtual ICollection<EmailCc> EmailCcs { get; set; } = new List<EmailCc>();

    [InverseProperty("EmailNavigation")]
    public virtual ICollection<EmailReceiver> EmailReceivers { get; set; } = new List<EmailReceiver>();

    [ForeignKey("EmailType")]
    [InverseProperty("Emails")]
    public virtual EmailType EmailTypeNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Emails")]
    public virtual User User { get; set; }
}
