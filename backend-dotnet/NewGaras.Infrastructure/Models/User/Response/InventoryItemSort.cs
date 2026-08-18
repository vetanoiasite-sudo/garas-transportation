using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.User.Response
{
    public class InventoryItemSort
    {
        public long InventoryItemID { set; get; }
        public string InventoryItemName { set; get; }
        public decimal? LowStockAfter {  set; get; }
        public decimal? AverageUnitPrice { set; get; }
        public decimal? LastUnitPrice { set; get; }
        public decimal? MaxUnitPrice { set; get; }
        public decimal? StockBalance {  set; get; }
    }
}
