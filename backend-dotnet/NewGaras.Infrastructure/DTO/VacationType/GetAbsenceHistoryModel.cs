using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationType
{
    public class GetAbsenceHistoryModel
    {
        public List<AbsenceHistory> PastAbsence { get; set; }

        public List<AbsenceHistory> PlannedAbsencee { get; set; }
    }
}
