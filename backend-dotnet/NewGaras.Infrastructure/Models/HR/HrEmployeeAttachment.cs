using System.Runtime.Serialization;
using NewGarasAPI.Models.Common;

namespace NewGarasAPI.Models.HR
{
    public class HrEmployeeAttachment
    {
        long? id { get; set; }
        Attachment attachment;
        string categoryName;
        bool? active;
        string otherValue;
        string expiredDate;
        [DataMember]
        public long? ID
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public Attachment Attachment
        {
            set { attachment = value; }
            get { return attachment; }
        }
        [DataMember]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        [DataMember]
        public string ExpiredDate
        {
            get { return expiredDate; }
            set { expiredDate = value; }
        }
        [DataMember]
        public bool? Active
        {
            get { return active; }
            set { active = value; }
        }
        [DataMember]
        public string OtherValue
        {
            set { otherValue = value; }
            get { return otherValue; }
        }
    }
}
