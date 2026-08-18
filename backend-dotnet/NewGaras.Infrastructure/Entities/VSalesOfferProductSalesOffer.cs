using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesOfferProductSalesOffer
{
    [Column("ID")]
    public long Id { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [Column("OfferID")]
    public long OfferId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    public long? SalesOfferCreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferCreationDate { get; set; }

    public long? SalesOfferModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SalesOfferModified { get; set; }

    public bool? SalesOfferActive { get; set; }

    [StringLength(250)]
    public string Status { get; set; }

    public bool? Completed { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? OfferAmount { get; set; }

    public bool? SendingOfferConfirmation { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SendingOfferDate { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProjectStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProjectEndDate { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [StringLength(250)]
    public string OfferSerial { get; set; }

    [StringLength(50)]
    public string ContractType { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReminderDate { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    public double? Quantity { get; set; }

    [Column("InventoryItemID")]
    public long? InventoryItemId { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool? InventoryItemActive { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [Column("_MinBalance")]
    public double? MinBalance { get; set; }

    [Column("_MaxBalance")]
    public double? MaxBalance { get; set; }

    public long? InventoryitemCreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InventoryItemCreationDate { get; set; }

    public long? InventoryItemModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ItemPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClientApprovalDate { get; set; }
}
