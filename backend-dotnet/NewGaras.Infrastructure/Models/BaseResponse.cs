using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models
{
    public class BaseResponse
    {
        public bool Result { get; set; }
        public List<Error> Errors { get; set; }
    }
}
