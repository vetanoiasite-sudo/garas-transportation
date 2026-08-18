namespace NewGarasAPI.Models.HR
{
    public class TeamUserData
    {
        public long ID { get; set; }
        public long DepartmentID { get; set; }
        public long TeamID { get; set; }
        public string DepartmentName { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModificationDate { get; set; }

        public List<UserData> UserDataList;
    }
}
