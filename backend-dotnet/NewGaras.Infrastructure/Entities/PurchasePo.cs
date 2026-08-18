using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PurchasePO")]
public partial class PurchasePo
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ToSupplierID")]
    public long ToSupplierId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("POTypeID")]
    public long? PotypeId { get; set; }

    [Column("AssignedAccountantID")]
    public long? AssignedAccountantId { get; set; }

    [StringLength(50)]
    public string ApprovalStatus { get; set; }

    [StringLength(500)]
    public string AccountantReplyNotes { get; set; }

    [StringLength(50)]
    public string TechApprovalStatus { get; set; }

    [StringLength(500)]
    public string TechReplyNotes { get; set; }

    [StringLength(50)]
    public string FinalApprovalStatus { get; set; }

    [StringLength(500)]
    public string FinalApprovalReplyNotes { get; set; }

    [Column("UserIDForTechApprove")]
    public long? UserIdforTechApprove { get; set; }

    [Column("UserIDForFinalApprove")]
    public long? UserIdforFinalApprove { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TechApproveDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinalApproveDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? TotalEstimatedCost { get; set; }

    public bool? SentToSupplier { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SupplierDeliveryDate { get; set; }

    [Column("AssignedPurchasingPersonID")]
    public long? AssignedPurchasingPersonId { get; set; }

    [StringLength(50)]
    public string SentToSupplierMethod { get; set; }

    [Column("SentToSupplierContactPersonID")]
    public long? SentToSupplierContactPersonId { get; set; }

    [ForeignKey("AssignedAccountantId")]
    [InverseProperty("PurchasePoAssignedAccountants")]
    public virtual User AssignedAccountant { get; set; }

    [ForeignKey("AssignedPurchasingPersonId")]
    [InverseProperty("PurchasePoAssignedPurchasingPeople")]
    public virtual User AssignedPurchasingPerson { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchasePoCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Po")]
    public virtual ICollection<InventoryAddingOrderItem> InventoryAddingOrderItems { get; set; } = new List<InventoryAddingOrderItem>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PurchasePoModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Po")]
    public virtual ICollection<PoapprovalStatus> PoapprovalStatuses { get; set; } = new List<PoapprovalStatus>();

    [InverseProperty("Po")]
    public virtual ICollection<PofinalSelecteSupplier> PofinalSelecteSuppliers { get; set; } = new List<PofinalSelecteSupplier>();

    [ForeignKey("PotypeId")]
    [InverseProperty("PurchasePos")]
    public virtual PurchasePotype Potype { get; set; }

    [InverseProperty("Po")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("Po")]
    public virtual ICollection<PrsupplierOffer> PrsupplierOffers { get; set; } = new List<PrsupplierOffer>();

    [InverseProperty("Po")]
    public virtual ICollection<PuchasePoshipment> PuchasePoshipments { get; set; } = new List<PuchasePoshipment>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchaseImportPosetting> PurchaseImportPosettings { get; set; } = new List<PurchaseImportPosetting>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePoamountPaymentMethod> PurchasePoamountPaymentMethods { get; set; } = new List<PurchasePoamountPaymentMethod>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePoattachment> PurchasePoattachments { get; set; } = new List<PurchasePoattachment>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePoinactiveTask> PurchasePoinactiveTasks { get; set; } = new List<PurchasePoinactiveTask>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePoinvoice> PurchasePoinvoices { get; set; } = new List<PurchasePoinvoice>();

    [InverseProperty("PurchasePo")]
    public virtual ICollection<PurchasePoitem> PurchasePoitems { get; set; } = new List<PurchasePoitem>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePopaymentSwift> PurchasePopaymentSwifts { get; set; } = new List<PurchasePopaymentSwift>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePopdfEditHistory> PurchasePopdfEditHistories { get; set; } = new List<PurchasePopdfEditHistory>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePopdfTemplate> PurchasePopdfTemplates { get; set; } = new List<PurchasePopdfTemplate>();

    [InverseProperty("Po")]
    public virtual ICollection<PurchasePopdf> PurchasePopdfs { get; set; } = new List<PurchasePopdf>();

    [ForeignKey("SentToSupplierContactPersonId")]
    [InverseProperty("PurchasePos")]
    public virtual SupplierContactPerson SentToSupplierContactPerson { get; set; }

    [InverseProperty("Po")]
    public virtual ICollection<SupplierAccountReviewed> SupplierAccountRevieweds { get; set; } = new List<SupplierAccountReviewed>();

    [InverseProperty("PurchasePo")]
    public virtual ICollection<SupplierAccount> SupplierAccounts { get; set; } = new List<SupplierAccount>();

    [ForeignKey("ToSupplierId")]
    [InverseProperty("PurchasePos")]
    public virtual Supplier ToSupplier { get; set; }

    [ForeignKey("UserIdforFinalApprove")]
    [InverseProperty("PurchasePoUserIdforFinalApproveNavigations")]
    public virtual User UserIdforFinalApproveNavigation { get; set; }

    [ForeignKey("UserIdforTechApprove")]
    [InverseProperty("PurchasePoUserIdforTechApproveNavigations")]
    public virtual User UserIdforTechApproveNavigation { get; set; }
}
