using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.WorkFlow
{
    public class AddWorkFlowDto
    {
        public long ProjectID { get; set; }
        public List<AddWorkFlowList> WorkFlowList { get; set; }
    }
}
