using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetTeamResponse
    {
        bool result;
        List<Error> errors;
        List<TeamData> teamList;



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
        public List<TeamData> TeamList
        {
            get
            {
                return teamList;
            }

            set
            {
                teamList = value;
            }
        }
    }
}
