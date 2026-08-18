using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ManagementOfRentOrder")]
public partial class ManagementOfRentOrder
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("RentOfferID")]
    public long RentOfferId { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReleaseDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlannedReceivingDate { get; set; }

    public int Period { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ActualReceivingDate { get; set; }

    public int Delay { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostOfExtraDays { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalCostOfExtraDays { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ExtraRequired { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    public bool DamageOrPenaltiesStatus { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DamageOrPenaltiesCost { get; set; }

    public string DamageOrPenaltiesDesc { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalRequiredExtraCost { get; set; }

    public bool FinFeedBackConfirmed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinFeedBackReplyDate { get; set; }

    [Column("FinFeedBackUserID")]
    public long? FinFeedBackUserId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ManagementOfRentOrderCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("ManagementOfRentOrder")]
    public virtual ICollection<ManagementOfRentOrderAttachment> ManagementOfRentOrderAttachments { get; set; } = new List<ManagementOfRentOrderAttachment>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ManagementOfRentOrderModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ManagementOfRentOrders")]
    public virtual Project Project { get; set; }

    [ForeignKey("RentOfferId")]
    [InverseProperty("ManagementOfRentOrders")]
    public virtual SalesOffer RentOffer { get; set; }
}
