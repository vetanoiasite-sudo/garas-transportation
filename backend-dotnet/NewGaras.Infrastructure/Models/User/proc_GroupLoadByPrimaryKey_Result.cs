namespace NewGarasAPI.Models.User
{
    public  class proc_GroupLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public long ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
