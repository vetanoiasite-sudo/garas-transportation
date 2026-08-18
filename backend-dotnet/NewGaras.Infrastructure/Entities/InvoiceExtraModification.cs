using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InvoiceExtraModification")]
public partial class InvoiceExtraModification
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InvoiceID")]
    public long InvoiceId { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("FabricationOrderID")]
    public long FabricationOrderId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ApprovalPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ApprovalDate { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InvoiceExtraModificationCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("FabricationOrderId")]
    [InverseProperty("InvoiceExtraModifications")]
    public virtual ProjectFabrication FabricationOrder { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceExtraModifications")]
    public virtual Invoice Invoice { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InvoiceExtraModificationModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("InvoiceExtraModifications")]
    public virtual Project Project { get; set; }
}
