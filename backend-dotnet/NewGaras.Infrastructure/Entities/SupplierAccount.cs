using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class SupplierAccount
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyAdjustingEntryID")]
    public long DailyAdjustingEntryId { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    [Column("PurchasePOID")]
    public long? PurchasePoid { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountType { get; set; }

    [Required]
    [StringLength(50)]
    public string AmountSign { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column("AccountOfJEId")]
    public long? AccountOfJeid { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("SupplierAccounts")]
    public virtual Account Account { get; set; }

    [ForeignKey("AccountOfJeid")]
    [InverseProperty("SupplierAccounts")]
    public virtual AccountOfJournalEntry AccountOfJe { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SupplierAccountCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SupplierAccountModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PurchasePoid")]
    [InverseProperty("SupplierAccounts")]
    public virtual PurchasePo PurchasePo { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierAccounts")]
    public virtual Supplier Supplier { get; set; }

    [InverseProperty("SupplierAccount")]
    public virtual ICollection<SupplierAccountReviewed> SupplierAccountRevieweds { get; set; } = new List<SupplierAccountReviewed>();
}
