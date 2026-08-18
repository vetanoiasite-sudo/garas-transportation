using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferAttachment")]
public partial class SalesOfferAttachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

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

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesOfferAttachmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesOfferAttachmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("SalesOfferAttachments")]
    public virtual SalesOffer Offer { get; set; }

    [InverseProperty("OfferAttachment")]
    public virtual ICollection<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissions { get; set; } = new List<SalesOfferAttachmentGroupPermission>();

    [InverseProperty("OfferAttachment")]
    public virtual ICollection<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissions { get; set; } = new List<SalesOfferAttachmentUserPermission>();
}
