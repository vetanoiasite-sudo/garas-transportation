namespace NewGarasAPI.Models
{
    public partial class proc_ClientLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public long SalesPersonID { get; set; }
        public string Note { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<System.DateTime> FirstContractDate { get; set; }
        public byte[] Logo { get; set; }
        public string GroupName { get; set; }
        public string BranchName { get; set; }
        public string Consultant { get; set; }
        public int FollowUpPeriod { get; set; }
        public string ConsultantType { get; set; }
        public Nullable<bool> SupportedByCompany { get; set; }
        public string SupportedBy { get; set; }
        public Nullable<bool> HasLogo { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<System.DateTime> LastReportDate { get; set; }
        public Nullable<int> NeedApproval { get; set; }
        public Nullable<long> ClientSerialCounter { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public Nullable<System.DateTime> OpeningBalanceDate { get; set; }
        public Nullable<int> OpeningBalanceCurrencyId { get; set; }
        public Nullable<int> DefaultDelivaryAndShippingMethodId { get; set; }
        public string OtherDelivaryAndShippingMethodName { get; set; }
        public string CommercialRecord { get; set; }
        public string TaxCard { get; set; }
        public Nullable<bool> OwnerCoProfile { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<int> ClientClassificationId { get; set; }
        public string ClassificationComment { get; set; }
    }
}
