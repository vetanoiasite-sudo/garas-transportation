using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SupplierLandLineData
    {
        public long SupplierId { get; set; }
        public List<AddSupplierLandLine> SupplierLandLines { get; set; }
    }
}
