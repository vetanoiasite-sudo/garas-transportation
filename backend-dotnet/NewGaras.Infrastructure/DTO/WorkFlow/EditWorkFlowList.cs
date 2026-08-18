using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.WorkFlow
{
    public class EditWorkFlowList
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int OrderNum { get; set; }
        public bool Active { get; set; }
    }
}
