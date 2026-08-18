using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class GetSupplierData
    {
        public SupplierData SuppliersData { get; set; }
        public SupplierConsultantData SupplierConsultantData { get; set; }
        public List<AddSupplierAddress> SupplierAddressData { get; set; }
        public List<GetSupplierContactPerson> SupplierContactPersonData { get; set; }
        public List<AddSupplierMobile> SupplierMobileData { get; set; }
        public List<AddSupplierLandLine> SupplierLandLineData { get; set; }
        public List<AddSupplierFax> SupplierFaxData { get; set; }
        public List<AddSupplierPaymentTerm> SupplierPaymentTermData { get; set; }
        public List<AddSupplierBankAccount> SupplierBankAccountData { get; set; }

        public List<Attachment> LicenseAttachements {  get; set; }
        public List<Attachment> BussinessCardsAttachments { get; set; }
        public List<Attachment> BrochuresAttachments { get; set; }
        public Attachment TaxCardAttachment { get; set; }
        public Attachment CommercialRecordAttachment { get; set; }
        public List<Attachment> OtherAttachments { get; set; }

        public bool? IsExistSupplierProfile { get; set; }
        public bool Result {  get; set; }
        public List<Error> Errors { get; set; }
    }
}
