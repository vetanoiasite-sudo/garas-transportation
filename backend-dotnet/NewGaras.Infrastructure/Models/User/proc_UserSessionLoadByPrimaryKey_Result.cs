namespace NewGarasAPI.Models.User
{
    public class proc_UserSessionLoadByPrimaryKey_Result

    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
