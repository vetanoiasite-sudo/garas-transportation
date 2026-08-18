using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class ClientLoginRequest
    {
        string userName;
        string password;
        string companyName;

        [DataMember]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }
        [DataMember]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        [DataMember]
        public string CompanyName
        {
            get
            {
                return companyName;
            }

            set
            {
                companyName = value;
            }
        }

    }
}
