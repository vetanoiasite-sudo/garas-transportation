using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class UserLogin
    {
        string email;
        string password;
        string companyName;
        string externalLoginFrom;

        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
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

        [DataMember]
        public string ExternalLoginFrom
        {
            get
            {
                return externalLoginFrom;
            }

            set
            {
                externalLoginFrom = value;
            }
        }

    }

}
