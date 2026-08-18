namespace NewGarasAPI.Models
{
    public class proc_AccountsLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTypeName { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<long> ParentCategory { get; set; }
        public int DataLevel { get; set; }
        public int AccountOrder { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Haveitem { get; set; }
        public decimal Accumulative { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Comment { get; set; }
        public bool Havetax { get; set; }
        public Nullable<long> TaxID { get; set; }
        public string TaxName { get; set; }
        public Nullable<decimal> TaxPercentage { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long CreatedBy { get; set; }
        public long AccountCategoryID { get; set; }
        public bool AdvanciedSettingsStatus { get; set; }
        public Nullable<bool> TranactionStatus { get; set; }
    }
}
