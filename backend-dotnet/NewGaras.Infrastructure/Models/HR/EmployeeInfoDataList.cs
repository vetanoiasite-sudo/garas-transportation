namespace NewGarasAPI.Models.HR
{
        public class EmployeeInfoDataList
        {
            public int? ID { get; set; }
            public string EmployeeName { get; set; }
            public string DepartmentName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string Gender { get; set; }

            public int expiredDocumentsCount { get; set; }
            public int? DepartmentID { get; set; }
            public int? OldID { get; set; }
            public int? BranchID { get; set; }
            public string JobTitleName { get; set; }
            public string BranchName { get; set; }
            public int? JobTitleID { get; set; }
            public int? Age { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Photo { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedBy { get; set; }
            public bool Active { get; set; }
        //mod by Gerges Abdullah

        public string ArFirstName { get; set; }
        public string ArLastName { get; set; }
        public string ArMiddleName { get; set; }
        public string LandLine { get; set; }
        public long NationalityId { get; set; }

        public int MilitaryStatuId { get; set; }
         public int MaritalStatusId { get; set; }

        public string DateOfBirth { get; set; }
        //----------------------------------
            public string Password { get; set; }

        public bool? AddHrUser { get; set; }


        }
    }

