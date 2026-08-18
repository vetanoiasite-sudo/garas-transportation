namespace NewGarasAPI.Models
{
    public partial class proc_BranchLoadByPrimaryKey_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CountryID { get; set; }
        public int GovernorateID { get; set; }
    }
}
