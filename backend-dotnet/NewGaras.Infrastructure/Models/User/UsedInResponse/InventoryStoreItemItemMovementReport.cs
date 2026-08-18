using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.User.UsedInResponse
{
    public class InventoryStoreItemItemMovementReport : InventoryStoreItem
    {

        public decimal? AverageUnitPrice { get; set; }
        public decimal? LastUnitPrice { get; set; }
        public decimal? MaxUnitPrice { get; set; }
        public decimal? NoMonths { get; set; }
        public decimal? ReleaseRate { get; set; }
        public decimal? ReleaseQty { get; set; }
        public decimal? LowStockAfter {  get; set; }
        //public decimal Balance;
        public long InventoryItemID { get; set; }
        public long InventoryStoreID { get; set; }
        public string MarketName { get; set; }
        public string CommercialName { get; set; }
        public decimal StockBalance { get; set; }

        //public string MarketName;
        public string OperationType { get; set; }
        // public string CommercialName;
        public string Exported {  get; set; }
        public string CategoryName { get; set; }
        public int? InventoryItemCategoryID { get; set; }
        public int? PriorityID { get; set; }
        public DateTime CreationDate { get; set; }
        public List<StoreMovement> MovementList { get; set; }
    }
}
