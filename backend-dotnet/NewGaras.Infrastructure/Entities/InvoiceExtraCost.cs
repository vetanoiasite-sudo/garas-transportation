using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InvoiceExtraCost")]
public partial class InvoiceExtraCost
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InvoiceID")]
    public long InvoiceId { get; set; }

    [Column("ExtraCostTypeID")]
    public long ExtraCostTypeId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InvoiceExtraCostCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InvoiceExtraCostModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
