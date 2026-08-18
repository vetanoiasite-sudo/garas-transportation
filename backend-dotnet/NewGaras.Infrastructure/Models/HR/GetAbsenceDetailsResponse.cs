using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetAbsenceDetailsResponse
    {
        List<AbsenceDetailsViewModel> data;
        bool result;
        List<Error> errors;

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
        public List<AbsenceDetailsViewModel> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }
    }
}
