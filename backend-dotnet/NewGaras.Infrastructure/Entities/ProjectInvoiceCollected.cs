using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInvoiceCollected")]
public partial class ProjectInvoiceCollected
{
    [Key]
    public long Id { get; set; }

    public long ProjectInvoiceId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(500)]
    public string Status { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    public int? PaymentMethodId { get; set; }

    public string AttachmentPath { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInvoiceCollectedCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInvoiceCollectedModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("ProjectInvoiceCollecteds")]
    public virtual PaymentMethod PaymentMethod { get; set; }

    [ForeignKey("ProjectInvoiceId")]
    [InverseProperty("ProjectInvoiceCollecteds")]
    public virtual ProjectInvoice ProjectInvoice { get; set; }
}
