using NewGaras.Infrastructure.DTO.VacationOverTimeAndDeductionRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationDay
{
    public class GetVacationDayDto
    {
        public long Id { get; set; }
        public string ArName { get; set; }
        public string EngName { get; set; }
        public string OtherName { get; set; }
        public string Reason { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDeducted { get; set; }
        public bool AllowOverTime { get; set; }
        public bool AllowDelayingDeduction { get; set; }
        public bool AllowAutomaticCalculations { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int BranchId { get; set; }

        public bool Active { get; set; }
        public List<VacationOverTimeDeductionRateDto> vacationOverTimes { get; set; }
    }
}
