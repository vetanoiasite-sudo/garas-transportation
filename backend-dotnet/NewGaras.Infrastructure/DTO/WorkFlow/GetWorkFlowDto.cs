using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.WorkFlow
{
    public class GetWorkFlowDto
    {
        public int Id { get; set; }
        public string WorkFlowName { get; set; }
        public long ProjectID { get; set; }
        public int OrderNo { get; set; }
        public bool Active {  get; set; }
    }
}
