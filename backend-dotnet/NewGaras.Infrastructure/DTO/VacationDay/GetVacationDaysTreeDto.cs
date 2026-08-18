using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationDay
{
    public class GetVacationDaysTreeDto
    {
        public long Id { get; set; }
        public DateOnly Day {  get; set; }
        public string DayName { get; set; }
        public string VacationDayName { get; set; }

    }
}
