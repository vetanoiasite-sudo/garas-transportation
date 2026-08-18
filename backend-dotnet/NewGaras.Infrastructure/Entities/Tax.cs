using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Tax")]
public partial class Tax
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string TaxName { get; set; }

    [StringLength(250)]
    public string TaxCode { get; set; }

    [StringLength(250)]
    public string TaxType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TaxPercentage { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("isPercentage")]
    public bool? IsPercentage { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaxCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaxModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<SalesOfferProductTax> SalesOfferProductTaxes { get; set; } = new List<SalesOfferProductTax>();
}
