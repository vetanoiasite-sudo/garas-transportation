using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PricingClarificationAttachment")]
public partial class PricingClarificationAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PricingClarificationID")]
    public long PricingClarificationId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtenssion { get; set; }

    [StringLength(250)]
    public string Category { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PricingClarificationAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PricingClarificationAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PricingClarificationId")]
    [InverseProperty("PricingClarificationAttachments")]
    public virtual PricingClearfication PricingClarification { get; set; }
}
