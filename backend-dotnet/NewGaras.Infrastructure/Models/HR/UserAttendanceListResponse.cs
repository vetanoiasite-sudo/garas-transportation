using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class UserAttendanceListResponse
    {
        List<UserAttendance> userAttendanceList;
        bool result;
        List<Error> errors;

        PaginationHeader paginationHeader;

        [DataMember]
        public PaginationHeader PaginationHeader
        {
            get
            {
                return paginationHeader;
            }

            set
            {
                paginationHeader = value;
            }
        }
        [DataMember]
        public List<UserAttendance> UserAttendanceList
        {
            get
            {
                return userAttendanceList;
            }

            set
            {
                userAttendanceList = value;
            }
        }
        [DataMember]
        public bool Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        [DataMember]
        public List<Error> Errors
        {
            get
            {
                return errors;
            }

            set
            {
                errors = value;
            }
        }
    }
}
