namespace NewGarasAPI.Models.User
{
    public class UserDDL
    {
        private long iD;
        private string name;
        private string email;
        private string jobTitleName;
        private string department;
        private int? branchId;
        private string branchName;
        private string image;
        private long teamId;
        private string teamName;
        private List<Roles> roleList;
        private List<GroupRoles> groupList;

        public long TeamId { get => teamId; set => teamId = value; }
        public string TeamName { get => teamName; set => teamName = value; }
        public long ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string JobTitleName { get => jobTitleName; set => jobTitleName = value; }
        public string Department { get => department; set => department = value; }
        public int? BranchId { get => branchId; set => branchId = value; }
        public string BranchName { get => branchName; set => branchName = value; }
        public string Image { get => image; set => image = value; }
        public List<Roles> RoleList { get => roleList; set => roleList = value; }
        public List<GroupRoles> GroupList { get => groupList; set => groupList = value; }
    }
}
