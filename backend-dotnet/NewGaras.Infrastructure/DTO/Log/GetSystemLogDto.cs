using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.Log
{
    public class GetSystemLogDto
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public int? TableId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string LogDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public long? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}
