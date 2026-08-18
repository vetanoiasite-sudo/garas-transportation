using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class HrUserAddressDto
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int AreaaId { get; set; }
        public int Floor { get; set; }
        public int Building { get; set; }
        public string Street { get; set; }
        public string Description { get; set; }
    }
}
