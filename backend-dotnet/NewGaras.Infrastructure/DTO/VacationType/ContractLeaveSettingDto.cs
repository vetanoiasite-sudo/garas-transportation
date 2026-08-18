using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationType
{
    public class ContractLeaveSettingDto
    {
        public long? Id { get; set; }
        public string HolidayName { get; set; }
        public decimal? BalancePerYear { get; set; }
        public decimal? BalancePerMonth { get; set; }
        public string Notes { get; set; }
    }
}
