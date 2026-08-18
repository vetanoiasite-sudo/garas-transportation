using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class CheckSupplierExistanceFilters
    {
        [FromHeader]
        public string SupplierName { get; set; }
        [FromHeader]
        public string SupplierMobile {  get; set; }
        [FromHeader]
        public string SupplierPhone { get; set; }
        [FromHeader]
        public string SupplierFax { get; set; }
        [FromHeader]
        public string SupplierEmail { get; set;}
        [FromHeader]
        public string SupplierWebsite { get; set;}
        [FromHeader]
        public string ContactPersonMobile { get; set; }

    }
}
