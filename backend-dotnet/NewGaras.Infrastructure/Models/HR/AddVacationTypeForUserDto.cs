using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HR
{
    public class AddVacationTypeForUserDto
    {
        public long HrUserId { get; set; }

        public List<int> VacationTypesIds { get; set; }
    }
}
