using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOPaymentSwift")]
public partial class PurchasePopaymentSwift
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SwiftAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SwiftDate { get; set; }

    [Column("PurchasePOAttachmentID")]
    public long? PurchasePoattachmentId { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentCategory { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePopaymentSwiftCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PurchasePopaymentSwiftModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchasePopaymentSwifts")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("PurchasePoattachmentId")]
    [InverseProperty("PurchasePopaymentSwifts")]
    public virtual PurchasePoattachment PurchasePoattachment { get; set; }
}
