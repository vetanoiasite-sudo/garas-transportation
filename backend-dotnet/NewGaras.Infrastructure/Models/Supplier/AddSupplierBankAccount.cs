namespace NewGaras.Infrastructure.Models.Supplier
{
    public class AddSupplierBankAccount
    {
        public long? ID { get; set; }
        public string BankDetails { get; set; }
        public string BeneficiaryName { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string Account { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}