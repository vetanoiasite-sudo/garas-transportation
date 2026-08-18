using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class UserDDLResponse
    {
        List<UserDDL> dDLList;
        bool result;
        List<Error> errors;


        [DataMember]
        public List<UserDDL> DDLList
        {
            get
            {
                return dDLList;
            }

            set
            {
                dDLList = value;
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
