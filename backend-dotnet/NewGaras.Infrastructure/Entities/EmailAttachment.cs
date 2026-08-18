using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EmailAttachment")]
public partial class EmailAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    public string AttachmentPath { get; set; }

    [Column("EmailID")]
    public long EmailId { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("EmailAttachments")]
    public virtual Email Email { get; set; }
}
