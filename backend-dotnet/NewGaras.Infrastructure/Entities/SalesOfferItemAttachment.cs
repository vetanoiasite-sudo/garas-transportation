using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferItemAttachment")]
public partial class SalesOfferItemAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

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

    [StringLength(250)]
    public string Category { get; set; }

    public long? SalesOfferProductId { get; set; }

    [ForeignKey("InventoryItemId")]
    [InverseProperty("SalesOfferItemAttachments")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("SalesOfferItemAttachments")]
    public virtual SalesOffer Offer { get; set; }

    [ForeignKey("SalesOfferProductId")]
    [InverseProperty("SalesOfferItemAttachments")]
    public virtual SalesOfferProduct SalesOfferProduct { get; set; }
}
