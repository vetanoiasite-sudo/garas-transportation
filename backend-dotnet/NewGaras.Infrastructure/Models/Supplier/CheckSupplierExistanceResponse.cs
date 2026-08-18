using NewGarasAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class CheckSupplierExistanceResponse
    {
        public List<SelectDDL> SuppliersList { get; set; }
        public int SuppliersCount { get; set; }
        public bool IsExist { get; set; }

        public bool Result {  get; set; }
        public List<Error> Errors { get; set; }
    }
}
