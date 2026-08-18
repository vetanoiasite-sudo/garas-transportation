using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Domain.Models
{
    public class BaseResponseWithData<ViewModel>
    {
        public bool Result { get; set; }
        public List<Error> Errors { get; set; }
        public ViewModel Data { get; set; }
    }
}
