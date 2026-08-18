using System.Text.Json.Serialization;

namespace NewGarasAPI.Models.Notification
{
    public class UserNotificationRequest
    {
        private long iD;
        private long toUserID;
        private string toUserName;
        private string toUserPhoto;
        private string title;
        private string createdBy;
        private string description;
        private string date;
        private string uRL;
        private bool @new;
        private long fromUserID;
        private string fromUserName;
        private string fromUserPhoto;
        private int notificationProcessID;

        public long ID { get => iD; set => iD = value; }
        public long ToUserID { get => toUserID; set => toUserID = value; }
       
       
        public string Title { get => title; set => title = value; }
        
       
        public string Description { get => description; set => description = value; }
       
        
        public string URL { get => uRL; set => uRL = value; }
        public bool New { get => @new; set => @new = value; }
        public long FromUserID { get => fromUserID; set => fromUserID = value; }

      
        public int NotificationProcessID { get => notificationProcessID; set => notificationProcessID = value; }
    }
}
