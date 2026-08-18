using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Log
{
    public class LogFilters
    {
        [FromHeader]
        public string ActionName { get; set; }
        [FromHeader]
        public string TableName { get; set; }
        [FromHeader]
        public string ColumnName { get; set; }
        [FromHeader]
        public string LogDate { get; set; }
        [FromHeader]
        public string OldValue { get; set; }
        [FromHeader]
        public string NewValue { get; set; }
        [FromHeader]
        public long? CreatedBy { get; set; }

        [FromHeader]
        public int PageNumber { get; set; } = 1;
        [FromHeader]
        public int PageSize { get; set; } = 10;
    }
}
