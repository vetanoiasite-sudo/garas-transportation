using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Domain.Models
{
    public class BaseResponseWithId<T>
    {
        public bool Result { get; set; }
        public List<Error> Errors { get; set; }
        public T ID { get; set; }
    }
}
