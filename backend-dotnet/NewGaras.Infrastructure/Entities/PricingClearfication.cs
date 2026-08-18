using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PricingClearfication")]
public partial class PricingClearfication
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PricingID")]
    public long PricingId { get; set; }

    public long CrreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Subject { get; set; }

    [Required]
    public string Body { get; set; }

    public long SentTo { get; set; }

    public string Reply { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReplyDate { get; set; }

    [ForeignKey("CrreatedBy")]
    [InverseProperty("PricingClearficationCrreatedByNavigations")]
    public virtual User CrreatedByNavigation { get; set; }

    [ForeignKey("PricingId")]
    [InverseProperty("PricingClearfications")]
    public virtual Pricing Pricing { get; set; }

    [InverseProperty("PricingClarification")]
    public virtual ICollection<PricingClarificationAttachment> PricingClarificationAttachments { get; set; } = new List<PricingClarificationAttachment>();

    [ForeignKey("SentTo")]
    [InverseProperty("PricingClearficationSentToNavigations")]
    public virtual User SentToNavigation { get; set; }
}
