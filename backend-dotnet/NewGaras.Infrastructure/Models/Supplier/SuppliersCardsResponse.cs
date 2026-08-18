using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SuppliersCardsResponse
    {
        public List<SupplierCardData> SuppliersList { get; set; }
        public PaginationHeader PaginationHeader { get; set; }

        public bool Result {  get; set; }
        public List<Error> Errors { get; set; }
    }
}
