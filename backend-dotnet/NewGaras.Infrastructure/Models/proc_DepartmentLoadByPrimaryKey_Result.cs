namespace NewGarasAPI.Models
{
    public partial class proc_DepartmentLoadByPrimaryKey_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int BranchID { get; set; }
    }
}
