using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationDay
{
    public class GetGroupedVacationDays
    {
        public string Key { get; set; }

        public List<GetVacationDaysTreeDto> VacationDays { get; set; }
    }
}
