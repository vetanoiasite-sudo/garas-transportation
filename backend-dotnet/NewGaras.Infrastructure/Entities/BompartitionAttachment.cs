using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMPartitionAttachments")]
public partial class BompartitionAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMPartitionID")]
    public long BompartitionId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtenssion { get; set; }

    [StringLength(250)]
    public string Category { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BompartitionId")]
    [InverseProperty("BompartitionAttachments")]
    public virtual Bompartition Bompartition { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BompartitionAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BompartitionAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
