using NewGaras.Infrastructure.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.User
{
    public class LoginResponse
    {
        bool result;
        List<NewGarasAPI.Models.Common.Error> errors;
        int Count;
        string data;
        string userID;
        long? emplyeeId;
        long userIDNO;
        int notificationCount;
        int taskCount;
        string userImageURL;
        string userName;
        string departmentName;
        string branchName;
        int? branchID;
        string jobtitle;
        List<Roles> roleList;
        List<GroupRoles> groupList;
        List<SelectDDL> specialityList;
        long countryId;
        string countryName;
        string companyInfo;
        string companyImg;
       
        long clientId;
        int? localCurrencyId;
        string localCurrencyName;
        bool allowLocationTracking;


        [DataMember]
        public bool AllowLocationTracking
        {
            get
            {
                return allowLocationTracking;
            }

            set
            {
                allowLocationTracking = value;
            }
        }

        [DataMember]
        public int? LocalCurrencyId
        {
            get
            {
                return localCurrencyId;
            }

            set
            {
                localCurrencyId = value;
            }
        }


        [DataMember]
        public string LocalCurrencyName
        {
            get
            {
                return localCurrencyName;
            }

            set
            {
                localCurrencyName = value;
            }
        }


        [DataMember]
        public long ClientId
        {
            get
            {
                return clientId;
            }

            set
            {
                clientId = value;
            }
        }
        [DataMember]
        public long? EmplyeeId
        {
            get
            {
                return emplyeeId;
            }

            set
            {
                emplyeeId = value;
            }
        }

        [DataMember]
        public string CompanyInfo
        {
            get
            {
                return companyInfo;
            }

            set
            {
                companyInfo = value;
            }
        }
        [DataMember]
        public string CompanyImg
        {
            get
            {
                return companyImg;
            }

            set
            {
                companyImg = value;
            }
        }

        [DataMember]
        public List<SelectDDL> SpecialityList
        {
            get
            {
                return specialityList;
            }

            set
            {
                specialityList = value;
            }
        }


        [DataMember]
        public string DepartmentName
        {
            get
            {
                return departmentName;
            }

            set
            {
                departmentName = value;
            }
        }

        [DataMember]
        public string BranchName
        {
            get
            {
                return branchName;
            }

            set
            {
                branchName = value;
            }
        }

        [DataMember]
        public int? BranchID
        {
            get
            {
                return branchID;
            }

            set
            {
                branchID = value;
            }
        }

        [DataMember]
        public int NotificationCount
        {
            get
            {
                return notificationCount;
            }

            set
            {
                notificationCount = value;
            }
        }

        [DataMember]
        public int TaskCount
        {
            get
            {
                return taskCount;
            }

            set
            {
                taskCount = value;
            }
        }


        [DataMember]
        public string Jobtitle
        {
            get
            {
                return jobtitle;
            }

            set
            {
                jobtitle = value;
            }
        }

        [DataMember]
        public List<Roles> RoleList
        {
            get
            {
                return roleList;
            }

            set
            {
                roleList = value;
            }
        }

        [DataMember]
        public List<GroupRoles> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
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
        public List<NewGarasAPI.Models.Common.Error> Errors
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
        public string UserID
        {
            get
            {
                return userID;
            }

            set
            {
                userID = value;
            }
        }

        [DataMember]
        public long UserIDNO
        {
            get
            {
                return userIDNO;
            }

            set
            {
                userIDNO = value;
            }
        }

        [DataMember]
        public string UserImageURL
        {
            get
            {
                return userImageURL;
            }

            set
            {
                userImageURL = value;
            }
        }

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
        public long CountryId
        {
            get
            {
                return countryId;
            }

            set
            {
                countryId = value;
            }
        }
        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
            }
        }
      
    }

    public class Roles
    {
        public int roleID;
        public string roleName;

        [DataMember]
        public int RoleID
        {
            get
            {
                return roleID;
            }

            set
            {
                roleID = value;
            }
        }

        [DataMember]
        public string RoleName
        {
            get
            {
                return roleName;
            }

            set
            {
                roleName = value;
            }
        }
    }
    public class GroupRoles
    {
        public long groupID;
        public string groupName;

        [DataMember]
        public long GroupID
        {
            get
            {
                return groupID;
            }

            set
            {
                groupID = value;
            }
        }
        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
            }
        }
        
    }

    public class SelectDDL
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public string Name{ get; set; }
    }

    public class SelectPosNumberDDL
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? StoreId { get; set; }
    }

}
