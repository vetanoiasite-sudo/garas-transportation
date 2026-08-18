using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.User.Filters
{
    public class GetAccountAndFinanceInventoryStoreItemMovementReportListFilters
    {
        [FromHeader]
        public long? InventoryStoreID { get; set; }
        [FromHeader]
        public long PriorityID { get; set; }
        [FromHeader]
        public long InventoryItemID { get; set; }
        [FromHeader]
        public long InventoryItemCategoryID { get; set; }
        [FromHeader]
        public long? LowAfterNOMonths { get; set; }
        [FromHeader]
        public string FromDate { get; set; }
        [FromHeader]
        public string ToDate { get; set; }
        [FromHeader]
        public string Exported { get; set; }
        [FromHeader]
        public string ItemSerial { get; set; }
        [FromHeader]
        public string SearchKey { get; set; }
        [FromHeader]
        public string SortBy { get; set; }
        [FromHeader]
        public int CurrentPage { get; set; } = 1;
        [FromHeader]
        public int NumberOfItemsPerPage { get; set; } = 10;
    }
}
