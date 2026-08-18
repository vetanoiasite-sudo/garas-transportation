using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Common
{
    public class BaseResponseWithId
    {
        bool result;
        List<Error> errors;
        // List<Galary> data;
        // Galary data;
        //[DataMember]
        //public Galary Data
        //{
        //    get
        //    {
        //        return data;
        //    }

        //    set
        //    {
        //        data = value;
        //    }
        //}
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
