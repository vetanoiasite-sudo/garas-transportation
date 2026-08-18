using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.DDL.UsedInResponse
{
    public class OfferItemDDL
    {
        public long ID { get; set; }
        public string ProductName { get; set; }
        public string ItemSerial { get; set; }
        public long FabID { get; set; }
        public string FabNo { get; set; }
        public string FabSerial { get; set; }
        public long ProjectID { get; set; }
        public string ProjectSerial { get; set; }
        public string ProjectName { get; set; }
        public string FabOrderItemSerial { get; set; }
        public long FabOrderItemID { get; set; }

    }
}
