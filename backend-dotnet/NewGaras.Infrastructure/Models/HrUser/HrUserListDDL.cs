using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.HrUser
{
    public class HrUserListDDL
    {
        public List<HrUserListDDLModel> HrUserLists { get; set; }

    }
    public class HrUserListDDLModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string ImgPath { get; set; }

    }
}
