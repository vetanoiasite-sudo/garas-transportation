using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class AddHrEmployeeToUserDTO
    {
        public long HrUserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set;}
        public string ConfirmPass { get; set; }
    }
}
