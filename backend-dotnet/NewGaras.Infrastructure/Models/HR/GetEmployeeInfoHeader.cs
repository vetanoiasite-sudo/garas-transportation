using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetEmployeeInfoHeader
    {
        [FromHeader]
        public string SearchKey { get; set; }
        [FromHeader]
        public int CurrentPage { get; set; } = 1;
        [FromHeader]
        public int NumberOfItemsPerPage { get; set; } = 10;
        [FromHeader]
        public long BranchID { get; set; } = 0;
        [DataMember]
        public long DepartmentID { get; set; } = 0;
        [FromHeader]
        public long UserID { get; set; } = 0;
        [FromHeader]
        public bool isExpired { get; set; }=false;
        [FromHeader]
        public DateTime expiredIn { get; set; } = DateTime.Now;
    }
}
