using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Notification
{
    public class GetNotificationsFilters
    {
        [FromHeader]
        public long FromUserID { get; set; }
        [FromHeader]
        public long NotificationProcessID { get; set; }
        [FromHeader]
        public long NotificationID { get; set; }
        [FromHeader]
        public string Title { get; set; }
        [FromHeader]
        public DateTime? DateFrom { get; set; }
        [FromHeader]
        public DateTime? DateTo { get; set; }
        [FromHeader]
        public string SearchKey { get; set; }
        [FromHeader]
        public int CurrentPage { get; set; } = 1;
        [FromHeader]
        public int NumberOfItemsPerPage { get; set; } = 10;
        [FromHeader]
        public bool? New { get; set; }
    }
}
