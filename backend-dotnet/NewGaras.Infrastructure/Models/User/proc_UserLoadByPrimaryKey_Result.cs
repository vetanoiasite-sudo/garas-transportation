namespace NewGarasAPI.Models.User
{
    public  class proc_UserLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public byte[] Photo { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Nullable<int> Age { get; set; }
        public string Gender { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> JobTitleID { get; set; }
        public Nullable<int> OldID { get; set; }
    }
}
