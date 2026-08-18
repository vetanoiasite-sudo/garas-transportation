using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PricingTerm")]
public partial class PricingTerm
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column("TermGroupID")]
    public int TermGroupId { get; set; }

    [Column("PricingID")]
    public long PricingId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PricingTermCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PricingTermModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PricingId")]
    [InverseProperty("PricingTerms")]
    public virtual Pricing Pricing { get; set; }

    [ForeignKey("TermGroupId")]
    [InverseProperty("PricingTerms")]
    public virtual TermsGroup TermGroup { get; set; }
}
