using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Common
{
    public class BaseResponseWithID
    {
        long iD;
        bool result;
        List<Error> errors;


        [DataMember]
        public long ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
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
