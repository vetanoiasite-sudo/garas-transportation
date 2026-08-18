using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetUserRolegAndRoleResponse
    {
        bool result;
        List<Error> errors;
        List<GetUserRoleData> userRoleList;
        List<GetUserGroupData> userGroupList;



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
        public List<GetUserRoleData> UserRoleList
        {
            get
            {
                return userRoleList;
            }

            set
            {
                userRoleList = value;
            }
        }
        [DataMember]
        public List<GetUserGroupData> UserGroupList
        {
            get
            {
                return userGroupList;
            }

            set
            {
                userGroupList = value;
            }
        }
    }
}
