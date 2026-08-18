using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class LogoutRequest
    {

        string sessionID;

        [DataMember]
        public string SessionID
        {
            get
            {
                return sessionID;
            }

            set
            {
                sessionID = value;
            }
        }

    }
}
