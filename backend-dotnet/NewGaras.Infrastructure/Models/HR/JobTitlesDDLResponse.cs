using NewGarasAPI.Models.Common;
using NewGarasAPI.Models.User;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class JobTitlesDDLResponse
    {
        List<SelectDDL> jobTitlesDDL;
        bool result;
        List<Error> errors;


        [DataMember]
        public List<SelectDDL> JobTitlesDDL
        {
            get
            {
                return jobTitlesDDL;
            }

            set
            {
                jobTitlesDDL = value;
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
