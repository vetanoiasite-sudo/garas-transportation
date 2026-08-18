using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePOInvoice")]
public partial class PurchasePoinvoice
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InvoiceDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InvoiceCollectionDueDate { get; set; }

    [Column("InvoiceAttachementID")]
    public long? InvoiceAttachementId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalInvoicePrice { get; set; }

    [Column("PurchasePOInvoiceTypeID")]
    public long PurchasePoinvoiceTypeId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TotalInvoiceCost { get; set; }

    public bool IsClosed { get; set; }

    public bool? IsFinalPriced { get; set; }

    public bool? IsSentToAcc { get; set; }

    public int? TansactionId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoinvoiceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("InvoiceAttachementId")]
    [InverseProperty("PurchasePoinvoices")]
    public virtual PurchasePoinvoiceAttachment InvoiceAttachement { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PurchasePoinvoiceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchasePoinvoices")]
    public virtual PurchasePo Po { get; set; }

    [InverseProperty("PurchasePoinvoice")]
    public virtual ICollection<PurchasePoinvoiceAttachment> PurchasePoinvoiceAttachments { get; set; } = new List<PurchasePoinvoiceAttachment>();

    [InverseProperty("PurchasePoinvoice")]
    public virtual ICollection<PurchasePoinvoiceCalculatedShipmentValue> PurchasePoinvoiceCalculatedShipmentValues { get; set; } = new List<PurchasePoinvoiceCalculatedShipmentValue>();

    [InverseProperty("Poinvoice")]
    public virtual ICollection<PurchasePoinvoiceClosedPayment> PurchasePoinvoiceClosedPayments { get; set; } = new List<PurchasePoinvoiceClosedPayment>();

    [InverseProperty("PurchasePoinvoice")]
    public virtual ICollection<PurchasePoinvoiceFinalExpensi> PurchasePoinvoiceFinalExpensis { get; set; } = new List<PurchasePoinvoiceFinalExpensi>();

    [InverseProperty("Poinvoice")]
    public virtual ICollection<PurchasePoinvoiceTaxIncluded> PurchasePoinvoiceTaxIncludeds { get; set; } = new List<PurchasePoinvoiceTaxIncluded>();

    [InverseProperty("PurchasePoinvoice")]
    public virtual ICollection<PurchasePoinvoiceTotalOrderCustomFee> PurchasePoinvoiceTotalOrderCustomFees { get; set; } = new List<PurchasePoinvoiceTotalOrderCustomFee>();

    [ForeignKey("PurchasePoinvoiceTypeId")]
    [InverseProperty("PurchasePoinvoices")]
    public virtual PurchasePoinvoiceType PurchasePoinvoiceType { get; set; }

    [InverseProperty("PurchasePoinvoice")]
    public virtual ICollection<PurchasePoinvoiceUnloadingFee> PurchasePoinvoiceUnloadingFees { get; set; } = new List<PurchasePoinvoiceUnloadingFee>();
}
