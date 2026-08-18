using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.WorkFlow
{
    public class AddWorkFlowList
    {
        public string Name { get; set; }
        public int orderNum { get; set; }
        public bool Active { get; set; }
    }
}
