
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class EmployeeBasicInfoVM
    {
        public long ID { get; set; }
       // public string IDEnc { get; set; }
        public string FirstNane { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long HrUserId { get; set; }

        //public string DepartmentName { get; set; }
        //  public string JobTitleName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
     //   public string BranchName { get; set; }
       // public bool Status { get; set; }
        public string? Photo { get; set; }
       // public int? JobTitleID { get; set; }
       // public int? DepartmentID { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitud { get; set; }
       public string? DirectionName { get; set; }
       public long? DirectionId { get; set; }
        public decimal? DirectionLatitude { get; set; }
        public decimal? DirectionLongtitud { get; set; }
        public bool RegisteredInline { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
