using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Notification
{
    public class GetNotificationsResponse
    {
        bool result;
        List<Error> errors;
        List<UserNotification> userNotificationsList;
        PaginationHeader paginationHeader;



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
        public List<UserNotification> UserNotificationsList
        {
            get
            {
                return userNotificationsList;
            }

            set
            {
                userNotificationsList = value;
            }
        }
        [DataMember]
        public PaginationHeader PaginationHeader
        {
            get
            {
                return paginationHeader;
            }

            set
            {
                paginationHeader = value;
            }
        }

    }

   
}
