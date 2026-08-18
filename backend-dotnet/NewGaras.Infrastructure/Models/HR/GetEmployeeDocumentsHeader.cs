using Microsoft.AspNetCore.Mvc;

namespace NewGarasAPI.Models.HR
{
    public class GetEmployeeDocumentsHeader
    {
        [FromHeader]
        public long EmployeeId { get; set; } = 0;
        [FromHeader]
        public string category { get; set; } = "";
        [FromHeader]
        public DateTime expireDate { get; set; } = DateTime.MinValue;
    }
}
