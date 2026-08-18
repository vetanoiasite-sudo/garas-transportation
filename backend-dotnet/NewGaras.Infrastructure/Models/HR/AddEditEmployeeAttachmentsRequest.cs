using NewGaras.Infrastructure.Entities;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class AddEditEmployeeAttachmentsRequest
    {
        long employeeId;
        List<HrEmployeeAttachment> attachments;
        [DataMember]
        public List<HrEmployeeAttachment> Attachments
        {
            set { attachments = value; }
            get { return attachments; }
        }
        [DataMember]
        public long EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }
    }
}
