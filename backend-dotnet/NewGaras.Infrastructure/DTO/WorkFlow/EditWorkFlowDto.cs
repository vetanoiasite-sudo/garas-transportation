using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.WorkFlow
{
    public class EditWorkFlowDto
    {
        public long ProjectID { get; set; }
        public List<EditWorkFlowList> WorkFlowList { get; set; }
    }
}
