using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class GetHrUserDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        //[EmailAddress]
        public string Email { get; set; }

        //[Phone]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }

        public string? Gender { get; set; }

        public string DateOfBirth { get; set; }

        public string LandLine { get; set; }

        public long HrNationalityId { get; set; }

        public string JobTitle { get; set; }

        public string BranchName { get; set; }

        public string DepName { get; set; }

        public string SystemEmail { get; set; }

        public bool IsUser { get; set; }
        public string ImgPath { get; set; }

        public bool Active { get; set; }

        public int? JobTitleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }

        public long? UserId { get;set; }
        public long? TeamId { get; set; }
        public string TeamName { get; set; }
        public bool IsDeleted { get; set; }
        public int? MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }

    }
}
