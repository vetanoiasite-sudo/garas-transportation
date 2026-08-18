using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOShipmentDocuments")]
public partial class PurchasePoshipmentDocument
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PurchasePOShipmentID")]
    public long PurchasePoshipmentId { get; set; }

    [Required]
    [StringLength(1000)]
    public string AttachmentPath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtenssion { get; set; }

    [StringLength(50)]
    public string Category { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceivedIn { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoshipmentDocuments")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("PurchasePoshipmentDocuments")]
    public virtual Currency Currency { get; set; }
}
