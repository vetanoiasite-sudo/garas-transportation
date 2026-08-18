namespace NewGarasAPI.Models
{
    public  class proc_CurrencyLoadByPrimaryKey_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<bool> IsLocal { get; set; }
    }
}
