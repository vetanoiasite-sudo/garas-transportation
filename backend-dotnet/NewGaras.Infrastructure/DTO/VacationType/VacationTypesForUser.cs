using NewGaras.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.VacationType
{
    public class VacationTypesForUser
    {
        public long Id { get; set; }

        public string HolidayName { get; set; }
        public long ContractID { get; set; }

        public int ContractLeaveSettingID {  get; set; }

        public bool Active { get; set; }
        public string LeaveAllowed { get; set; }

        public int Balance { get; set; }

        public decimal BalancePerMonth { get; set; }
        public decimal BalancePerYear { get; set; }

        public int? Used {  get; set; }

        public int? Remain {  get; set; }

        public long HrUserId { get; set; }

        public string Comment { get; set; }
    }
}
