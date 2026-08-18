
namespace NewGaras.Domain.DTO.HrUser
{
    public class HrUserCardDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
        public string JobTitle { get; set; }
        public bool IsUser { get; set; }
        public string ImgPath { get; set; }
    }
}
