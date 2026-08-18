using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Common
{
    public class UploadAttachment
    {
        [FromForm]
        public long? Id { get; set; }
        [FromForm]
        public IFormFile FileContent { get; set; }
        [FromForm]
        public bool Active { get; set; }
        public string Category { get; set; }
    }

}
