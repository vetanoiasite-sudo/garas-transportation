using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectAttachment")]
public partial class ProjectAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Required]
    [StringLength(250)]
    public string AttachmentPath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(50)]
    public string FileName { get; set; }

    [StringLength(10)]
    public string FileExtenssion { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectAttachments")]
    public virtual Project Project { get; set; }
}
