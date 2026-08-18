using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HrUser
{
    public class UserDataResponse
    {
        public  long HRUserId { get; set; }
        public  long UserSystemId { get; set; }
        public  string UserSystemEmail { get; set; }
        public string UserSystemName { get; set; }
    }
}
