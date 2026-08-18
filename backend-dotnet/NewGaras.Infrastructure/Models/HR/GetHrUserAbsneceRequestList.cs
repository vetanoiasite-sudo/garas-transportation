using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HR
{
    public class GetHrUserAbsneceRequestList
    {
        public string AbsenceTypeName { get; set; }

        public int TotalBalance { get; set; }
        public int Requested { get; set; }
        public int Remain { get; set; }

        public List<AbsenceRequest> AbsenceRequestsList { get; set; }


    }
}
