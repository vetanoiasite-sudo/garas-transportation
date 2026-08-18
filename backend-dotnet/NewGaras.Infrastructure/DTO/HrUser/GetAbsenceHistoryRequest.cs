using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class GetAbsenceHistoryRequest
    {
        [FromHeader]
        public long HrUserId { get; set; }

        [FromHeader]
        public int AbsenceTypeId { get; set; }
    }
}
