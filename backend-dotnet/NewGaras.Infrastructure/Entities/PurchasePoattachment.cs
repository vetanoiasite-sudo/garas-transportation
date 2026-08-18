using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOAttachment")]
public partial class PurchasePoattachment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

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

    [ForeignKey("Poid")]
    [InverseProperty("PurchasePoattachments")]
    public virtual PurchasePo Po { get; set; }

    [InverseProperty("PurchasePoattachment")]
    public virtual ICollection<PurchasePopaymentSwift> PurchasePopaymentSwifts { get; set; } = new List<PurchasePopaymentSwift>();
}
