using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SupplierData : SupplierMainData
    {
        public string Type { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
        public int? Rate { get; set; }
        public string FirstContractDate { get; set; }
        public decimal? OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public string OpeningBalanceDate { get; set; }
        public int? OpeningBalanceCurrencyId { get; set; }
        public string OpeningBalanceCurrencyName { get; set; }
        public int? DefaultDelivaryAndShippingMethodId { get; set; }
        public string DefaultDelivaryAndShippingMethodName { get; set; }
        public string OtherDelivaryAndShippingMethodName { get; set; }
        public string CommercialRecord {  get; set; }
        public string TaxCard {  get; set; }
        public string ChangeSalesPersonComment { get; set; }
        public string RegistrationNumber { get; set; }
        public string Logo { get; set; }
        public List<AddSupplierSpeciality> SupplierSpecialities { get; set; }
        public List<AddSupplierAddress> SupplierAddresses { get; set; }
        public List<AddSupplierLandLine> SupplierLandLines { get; set; }
        public List<AddSupplierMobile> SupplierMobiles { get; set; }
        public List<AddSupplierFax> SupplierFaxes { get; set; }
        public List<AddSupplierPaymentTerm> SupplierPaymentTerms { get; set; }
        public List<AddSupplierBankAccount> SupplierBankAccounts;
    }
}
