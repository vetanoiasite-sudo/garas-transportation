using NewGaras.Infrastructure.Models.HR;
using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetListOfEmployeeResponse
    {
        bool result;
        List<Error> errors;
        List<EmployeeInfoData> employeeInfoList;
        PaginationHeader paginationHeader;
        string userImageURL;
        string password;

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

        [DataMember]
        public List<EmployeeInfoData> EmployeeInfoList
        {
            get
            {
                return employeeInfoList;
            }

            set
            {
                employeeInfoList = value;
            }
        }
        [DataMember]
        public string UserImageURL
        {
            get
            {
                return userImageURL;
            }

            set
            {
                userImageURL = value;
            }
        }
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
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
    }
}
