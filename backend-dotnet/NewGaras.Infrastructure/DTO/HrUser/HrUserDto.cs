
namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class HrUserDto
    {
        public string FirstName { get; set; }

        public string ARFirstName { get; set; }

        public string MiddleName { get; set; } = string.Empty;

        public string ARMiddleName { get; set; }

        public string LastName { get; set; }

        public string ARLastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool? Active { get; set; }

        public IFormFile? Photo { get; set; }

        //public DateTime CreationDate { get; set; }

        //public int? Age { get; set; }

        public string? Gender { get; set; }

        public string DateOfBirth { get; set; }

        public string LandLine { get; set; }

        public long? NationalityId { get; set; }
        public int? MaritalStatusId { get; set; }

        public int? BranchID { get; set; }
        public int? DepartmentID { get; set; }
        public long? TeamId { get; set; }
        public int? JobTitleID { get; set; }

        public bool IsDeleted { get; set; } = false;
        public decimal? Latitude { get; set; }

        public decimal? Longtitud { get; set; }
    }
}
