using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInvoice")]
public partial class ProjectInvoice
{
    [Key]
    public long Id { get; set; }

    public long ProjectId { get; set; }

    [Required]
    [StringLength(450)]
    public string InvoiceSerial { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InvoiceDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Collected { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    public string AttachmentPath { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInvoiceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInvoiceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectInvoices")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectInvoice")]
    public virtual ICollection<ProjectInvoiceCollected> ProjectInvoiceCollecteds { get; set; } = new List<ProjectInvoiceCollected>();

    [InverseProperty("ProjectInvoice")]
    public virtual ICollection<ProjectInvoiceItem> ProjectInvoiceItems { get; set; } = new List<ProjectInvoiceItem>();
}
