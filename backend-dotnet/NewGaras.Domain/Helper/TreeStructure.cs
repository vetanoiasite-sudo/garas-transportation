using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Domain.Helper
{
    public class TreeStructure
    {
        public class TreeViewDto
        {
            public string id { get; set; }
            public string title { get; set; }
            public string parentId { get; set; }
            public bool? HasChild { get; set; }
            public IList<TreeViewDto> subs { get; set; }
        }
        /*public class TreeViewDto2
        {
            public string id { get; set; }
            public string title { get; set; }
            public string parentId { get; set; }
            public bool? HasChild { get; set; }
            public int? ItemCount { get; set; }
            public bool? HaveItem { get; set; }
            public decimal? SumOfRemainBalanceCostwithMainCu { get; set; }
            public decimal? SumOfRemainBalanceCostwithEgp { get; set; }
            public IList<TreeViewDto2> subs { get; set; }
        }*/

    }
}
