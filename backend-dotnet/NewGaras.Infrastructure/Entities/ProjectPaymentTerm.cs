using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ProjectPaymentTerm
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("PaymentTermID")]
    public int PaymentTermId { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Percentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("currencyID")]
    public int CurrencyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DueDate { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Collected { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CollectionDate { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Remain { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectPaymentTermCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("ProjectPaymentTerms")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectPaymentTermModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PaymentTermId")]
    [InverseProperty("ProjectPaymentTerms")]
    public virtual PaymentTerm PaymentTerm { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectPaymentTerms")]
    public virtual Project Project { get; set; }
}
