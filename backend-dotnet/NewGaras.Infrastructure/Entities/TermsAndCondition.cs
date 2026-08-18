using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class TermsAndCondition
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("TermsCategoryID")]
    public int TermsCategoryId { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TermsAndConditionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TermsAndConditionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TermsCategoryId")]
    [InverseProperty("TermsAndConditions")]
    public virtual TermsAndConditionsCategory TermsCategory { get; set; }
}
