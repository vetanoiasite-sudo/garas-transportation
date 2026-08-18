using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HR
{
    public class EmployeeInfoData : EmployeeBasicInfo
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }

        public int expiredDocumentsCount { get; set; }

        public int? OldID { get; set; }
        public int? BranchID { get; set; }
        public int? Age { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
        public long? HrUserId { get; set; }
        public string LandLine { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitud { get; set; }


    }

    public class EmployeeBasicInfo
    {
        public int? ID { get; set; }
        public string IDEnc { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitleName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string BranchName { get; set; }
        public bool Status { get; set; }
        public string Photo { get; set; }
        public int? JobTitleID { get; set; }
        public int? DepartmentID { get; set; }
    }
}
