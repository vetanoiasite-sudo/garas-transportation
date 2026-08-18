using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models
{
    public class BaseResponseWithDataAndHeader<ViewModel>
    {
        public bool Result { get; set; }
        public List<Error> Errors { get; set; }
        public ViewModel Data { get; set; }

        public PaginationHeader PaginationHeader { get; set; }
    }
}
