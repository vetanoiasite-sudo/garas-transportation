using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetEmployeeExpiredDocumentsResponse
    {
        List<HrEmployeeAttachment> attachments;
        bool result;
        List<Error> errors;
        [DataMember]
        public List<HrEmployeeAttachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }
        [DataMember]
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        [DataMember]
        public List<Error> Errors
        {
            get { return errors; }
            set { errors = value; }
        }
    }
}
