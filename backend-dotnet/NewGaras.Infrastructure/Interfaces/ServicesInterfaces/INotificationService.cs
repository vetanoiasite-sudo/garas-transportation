using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Models.Notification;
using NewGarasAPI.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface INotificationService
    {
        public HearderVaidatorOutput Validation { get; set; }

        public GetNotificationsResponse GetNotifications(GetNotificationsFilters filters);
        public long CreateNotification(long ToUserID, string _Title, string _Description, string _URL, bool _New, long? _FromUserID, int? _NotificationProcessID);

        public Task<BaseResponseWithId<long>> EditNotifications(UserNotification request);

        public Task<GetNotificationProcessDDLResponse> GetNotificationProcessDDL();
    }
}
