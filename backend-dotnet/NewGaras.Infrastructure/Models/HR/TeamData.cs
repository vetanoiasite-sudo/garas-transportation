namespace NewGarasAPI.Models.HR
{
    public class TeamData
    {
        public long ID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
