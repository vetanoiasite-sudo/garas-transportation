namespace NewGarasAPI.Models.Notification
{
    public class proc_NotificationLoadByPrimaryKey_Result
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public string URL { get; set; }
        public bool New { get; set; }
        public Nullable<long> FromUserId { get; set; }
        public Nullable<int> NotificationProcessId { get; set; }
    }
}
