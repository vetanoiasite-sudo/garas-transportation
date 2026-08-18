using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetTeamUserResponse
    {
        bool result;
        List<Error> errors;
        TeamUserData teamObj;



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
        public TeamUserData TeamUserObj
        {
            get
            {
                return teamObj;
            }

            set
            {
                teamObj = value;
            }
        }
    }
}
