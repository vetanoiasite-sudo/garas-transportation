using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Pricing")]
public partial class Pricing
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMPriceTotal", TypeName = "decimal(18, 2)")]
    public decimal? BompriceTotal { get; set; }

    public double? QuantityTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ExtraPriceTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PricingDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Total { get; set; }

    [StringLength(1000)]
    public string FilePath { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(100)]
    public string Status { get; set; }

    public bool SalesHeadApprove { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesHeadApprovalDate { get; set; }

    public bool PricingHeadApprove { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PricingHeadApprovalDate { get; set; }

    [StringLength(250)]
    public string CurrentPerformer { get; set; }

    [Column("PricingPersonID")]
    public long? PricingPersonId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Completed { get; set; }

    [Column("RefID")]
    public long? RefId { get; set; }

    [Required]
    [StringLength(250)]
    public string PricingType { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Pricings")]
    public virtual Branch Branch { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PricingCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PricingModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Pricing")]
    public virtual ICollection<PricingClearfication> PricingClearfications { get; set; } = new List<PricingClearfication>();

    [InverseProperty("Pricing")]
    public virtual ICollection<PricingExtraCost> PricingExtraCosts { get; set; } = new List<PricingExtraCost>();

    [ForeignKey("PricingPersonId")]
    [InverseProperty("PricingPricingPeople")]
    public virtual User PricingPerson { get; set; }

    [InverseProperty("Pricing")]
    public virtual ICollection<PricingProduct> PricingProducts { get; set; } = new List<PricingProduct>();

    [InverseProperty("Pricing")]
    public virtual ICollection<PricingTerm> PricingTerms { get; set; } = new List<PricingTerm>();
}
