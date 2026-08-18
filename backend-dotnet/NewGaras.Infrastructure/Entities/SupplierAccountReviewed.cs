using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SupplierAccountReviewed")]
public partial class SupplierAccountReviewed
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("IsPO")]
    public bool IsPo { get; set; }

    [Column("POID")]
    public long? Poid { get; set; }

    [Column("SupplierAccountID")]
    public long? SupplierAccountId { get; set; }

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    public bool IsReviewed { get; set; }

    public int? StatusReviewed { get; set; }

    public long CreatedyBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedyBy")]
    [InverseProperty("SupplierAccountReviewedCreatedyByNavigations")]
    public virtual User CreatedyByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SupplierAccountReviewedModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("SupplierAccountRevieweds")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierAccountRevieweds")]
    public virtual Supplier Supplier { get; set; }

    [ForeignKey("SupplierAccountId")]
    [InverseProperty("SupplierAccountRevieweds")]
    public virtual SupplierAccount SupplierAccount { get; set; }
}
