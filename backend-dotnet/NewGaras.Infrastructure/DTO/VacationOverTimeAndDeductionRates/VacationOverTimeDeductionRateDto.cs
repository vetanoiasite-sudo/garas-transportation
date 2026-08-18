using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationOverTimeAndDeductionRates
{
    public class VacationOverTimeDeductionRateDto
    {
        public long? Id { get; set; }

        public TimeOnly From { get; set; } = new TimeOnly(0, 0, 0);

        public TimeOnly To { get; set; } = new TimeOnly(0, 0, 0);

        public decimal Rate { get; set; }

        public long VacationDayId { get; set; }
        public int BranchId { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
