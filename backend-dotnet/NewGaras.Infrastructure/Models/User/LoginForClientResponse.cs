using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class LoginForClientResponse
    {
        bool result;
        List<Error> errors;
        string data;
        long clientID;
        string clientIDEnc;
        string clientImageURL;
        string clientName;
        string contactPersonName;
        string clientMobile;
        string contactPersonMobile;
        long maintenanceID;


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
        public string Data
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

        [DataMember]
        public long ClientID
        {
            get
            {
                return clientID;
            }

            set
            {
                clientID = value;
            }
        }

        [DataMember]
        public string ClientIDEnc
        {
            get
            {
                return clientIDEnc;
            }

            set
            {
                clientIDEnc = value;
            }
        }


        [DataMember]
        public string ClientImageURL
        {
            get
            {
                return clientImageURL;
            }

            set
            {
                clientImageURL = value;
            }
        }

        [DataMember]
        public string ClientName
        {
            get
            {
                return clientName;
            }

            set
            {
                clientName = value;
            }
        }
        [DataMember]
        public string ContactPersonName
        {
            get
            {
                return contactPersonName;
            }

            set
            {
                contactPersonName = value;
            }
        }

        [DataMember]
        public long MaintenanceID
        {
            get
            {
                return maintenanceID;
            }

            set
            {
                maintenanceID = value;
            }
        }

        [DataMember]
        public string ClientMobile
        {
            get
            {
                return clientMobile;
            }

            set
            {
                clientMobile = value;
            }
        }
        [DataMember]
        public string ContactPersonMobile
        {
            get
            {
                return contactPersonMobile;
            }

            set
            {
                contactPersonMobile = value;
            }
        }
    }
}
