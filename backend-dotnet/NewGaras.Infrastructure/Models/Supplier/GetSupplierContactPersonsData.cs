using NewGaras.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class GetSupplierContactPersonsData
    {
        public List<GetSupplierContactPerson> SupplierContactPersonData { get; set; }

        public bool Result { get; set; }
        public List<Error> Errors { get; set; }
    }
}
