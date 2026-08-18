using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HrUser
{
    public class UserEmployeeResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
