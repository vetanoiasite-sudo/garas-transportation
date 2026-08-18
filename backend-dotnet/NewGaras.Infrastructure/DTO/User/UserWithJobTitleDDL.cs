using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.User
{
    public class UserWithJobTitleDDL
    {
        public long ID { get; set; }
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string JobTitleName { get; set; }
        public string ImgPath { get; set; }

    }
}
