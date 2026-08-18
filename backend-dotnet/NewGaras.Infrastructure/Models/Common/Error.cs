using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Common
{
    public class Error
    {
      
        public string errorMSG;
       
        public string errorCode;

        [DataMember]
        public string ErrorMSG
        {
            get
            {
                return errorMSG;
            }

            set
            {
                errorMSG = value;
            }
        }
        [DataMember]
        public string ErrorCode
        {
            get
            {
                return errorCode;
            }

            set
            {
                errorCode = value;
            }
        }
    }
}
