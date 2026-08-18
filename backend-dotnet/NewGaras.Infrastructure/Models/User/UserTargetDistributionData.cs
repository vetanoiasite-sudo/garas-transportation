namespace NewGaras.Infrastructure.Models.User
{
    public class UserTargetDistributionData
    {
        public long ID { get; set; }
        public int GroupID { get; set; }
        public int BranchID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}