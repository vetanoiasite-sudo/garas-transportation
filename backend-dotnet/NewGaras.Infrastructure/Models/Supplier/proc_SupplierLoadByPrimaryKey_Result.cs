namespace NewGarasAPI.Models.Supplier
{
    public  class proc_SupplierLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Note { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<System.DateTime> FirstContractDate { get; set; }
        public byte[] Logo { get; set; }
        public Nullable<bool> HasLogo { get; set; }
        public bool Active { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
