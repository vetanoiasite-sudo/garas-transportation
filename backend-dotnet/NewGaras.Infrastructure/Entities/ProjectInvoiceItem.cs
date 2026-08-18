using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInvoiceItem")]
public partial class ProjectInvoiceItem
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string UserName { get; set; }

    [Required]
    [StringLength(500)]
    public string JobtitleName { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Rate { get; set; }

    [Column("ProjectInvoiceID")]
    public long ProjectInvoiceId { get; set; }

    public long CreatedBy { get; set; }

    public long ModifedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Total { get; set; }

    [Column("ItemID")]
    public long? ItemId { get; set; }

    [Column("UOMID")]
    public int? Uomid { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInvoiceItemCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifedBy")]
    [InverseProperty("ProjectInvoiceItemModifedByNavigations")]
    public virtual User ModifedByNavigation { get; set; }

    [ForeignKey("ProjectInvoiceId")]
    [InverseProperty("ProjectInvoiceItems")]
    public virtual ProjectInvoice ProjectInvoice { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("ProjectInvoiceItems")]
    public virtual InventoryUom Uom { get; set; }
}
