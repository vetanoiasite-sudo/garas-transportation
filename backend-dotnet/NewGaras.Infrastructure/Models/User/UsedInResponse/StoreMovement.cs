using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.User.UsedInResponse
{
    public class StoreMovement
    {
        public int InventoryStoreId { get; set; }
        public string InventoryStoreName { get; set; }
        public decimal MovementQTY { get; set; }
        public string Operation { get; set; }
    }

}
