using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    [Required]
    [StringLength(250)]
    public string Type { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [StringLength(250)]
    public string WebSite { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public string Note { get; set; }

    public int? Rate { get; set; }

    public DateOnly? FirstContractDate { get; set; }

    public byte[] Logo { get; set; }

    public bool? HasLogo { get; set; }

    public bool Active { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? SupplierSerialCounter { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? OpeningBalance { get; set; }

    [StringLength(50)]
    public string OpeningBalanceType { get; set; }

    public DateOnly? OpeningBalanceDate { get; set; }

    public int? OpeningBalanceCurrencyId { get; set; }

    public int? DefaultDelivaryAndShippingMethodId { get; set; }

    [StringLength(200)]
    public string OtherDelivaryAndShippingMethodName { get; set; }

    [StringLength(200)]
    public string CommercialRecord { get; set; }

    [StringLength(200)]
    public string TaxCard { get; set; }

    [StringLength(200)]
    public string RegistrationNumber { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SupplierCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DefaultDelivaryAndShippingMethodId")]
    [InverseProperty("Suppliers")]
    public virtual DeliveryAndShippingMethod DefaultDelivaryAndShippingMethod { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<DistributionSupplierPayment> DistributionSupplierPayments { get; set; } = new List<DistributionSupplierPayment>();

    [InverseProperty("Supplier")]
    public virtual ICollection<InventoryAddingOrder> InventoryAddingOrders { get; set; } = new List<InventoryAddingOrder>();

    [InverseProperty("Supplier")]
    public virtual ICollection<InventoryItemPrice> InventoryItemPrices { get; set; } = new List<InventoryItemPrice>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SupplierModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<PrsupplierOffer> PrsupplierOffers { get; set; } = new List<PrsupplierOffer>();

    [InverseProperty("ToSupplier")]
    public virtual ICollection<PurchasePo> PurchasePos { get; set; } = new List<PurchasePo>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierAccountReviewed> SupplierAccountRevieweds { get; set; } = new List<SupplierAccountReviewed>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierAccount> SupplierAccounts { get; set; } = new List<SupplierAccount>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; } = new List<SupplierAddress>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierAttachment> SupplierAttachments { get; set; } = new List<SupplierAttachment>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierBankAccount> SupplierBankAccounts { get; set; } = new List<SupplierBankAccount>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierContactPerson> SupplierContactPeople { get; set; } = new List<SupplierContactPerson>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierFax> SupplierFaxes { get; set; } = new List<SupplierFax>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierMobile> SupplierMobiles { get; set; } = new List<SupplierMobile>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierPaymentTerm> SupplierPaymentTerms { get; set; } = new List<SupplierPaymentTerm>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierPhone> SupplierPhones { get; set; } = new List<SupplierPhone>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierSpeciality> SupplierSpecialities { get; set; } = new List<SupplierSpeciality>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TransportationRoundForRoute> TransportationRoundForRoutes { get; set; } = new List<TransportationRoundForRoute>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TransportationVehicleRouteAccount> TransportationVehicleRouteAccounts { get; set; } = new List<TransportationVehicleRouteAccount>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRoutes { get; set; } = new List<TransportationVehicleRoute>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TransportionLineSupplierPayment> TransportionLineSupplierPayments { get; set; } = new List<TransportionLineSupplierPayment>();
}
