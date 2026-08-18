using Microsoft.AspNetCore.Mvc;

namespace NewGarasAPI.Models.HR
{
    public class GetEmployeeAttendenceHeader
    {
        [FromHeader]
        public long EmployeeId { set; get; } = 0;
        [FromHeader]
        public int CurrentPage { set; get; } = 1;
        [FromHeader]
        public int NumberOfItemsPerPage { set; get; } = 10;
        [FromHeader]
        public DateTime fromDate { set; get; } = DateTime.Now;
        [FromHeader]
        public DateTime toDate { set; get; } = DateTime.Now;
    }
}
