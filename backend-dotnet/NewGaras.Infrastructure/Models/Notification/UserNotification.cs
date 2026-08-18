using System.Text.Json.Serialization;

namespace NewGarasAPI.Models.Notification
{
    public class UserNotification
    {
        private long? iD;
        private long? toUserID;
        private string toUserName;
        private string toUserPhoto;
        private string title;
        private string createdBy;
        private string description;
        private string date;
        private string uRL;
        private bool @new;
        private long? fromUserID;
        private string fromUserName;
        private string fromUserPhoto;
        private int? notificationProcessID;
        private bool? approval;
        private string approvedBy;

        public long? ID { get => iD; set => iD = value; }
        public long? ToUserID { get => toUserID; set => toUserID = value; }
        public string ToUserName { get => toUserName; set => toUserName = value; }
       
        public string ToUserPhoto { get => toUserPhoto; set => toUserPhoto = value; }
        public string Title { get => title; set => title = value; }
        
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string Description { get => description; set => description = value; }
       
        public string Date { get => date; set => date = value; }
        public string URL { get => uRL; set => uRL = value; }
        public bool New { get => @new; set => @new = value; }
        public long? FromUserID { get => fromUserID; set => fromUserID = value; }

        public string FromUserName { get => fromUserName; set => fromUserName = value; }
        
        public string FromUserPhoto { get => fromUserPhoto; set => fromUserPhoto = value; }
        public int? NotificationProcessID { get => notificationProcessID; set => notificationProcessID = value; }
        public bool? Approval { get => approval; set => approval = value; }
        public string ApprovedBy { get => approvedBy; set => approvedBy = value; }
    }
}
