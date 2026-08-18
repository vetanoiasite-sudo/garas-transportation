using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransprotationUserAttedanceVM
    {
        public long Id { get; set; }

        public string Type { get; set; }

     
        public string Serial { get; set; }


        public bool CheckInOrCheckOut { get; set; }

        
        public DateTime CreationDate { get; set; }

        public long CreationBy { get; set; }

      
        public decimal? CheckInLatitude { get; set; }

        
        public decimal? CheckInLongtitud { get; set; }

        public long? CheckInRouteDirectionId { get; set; }

       
        public decimal? CheckOutLatitude { get; set; }

       
        public decimal? CheckOutLongtitud { get; set; }

        public long? CheckOutRouteDirectionId { get; set; }
    }
}
