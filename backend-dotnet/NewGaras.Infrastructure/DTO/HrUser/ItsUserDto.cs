using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.DTO.HrUser
{
    public class ItsUserDto
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? Modified { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int? Age { get; set; }

        public string Gender { get; set; }

        public long? CreatedBy { get; set; }

        public int? BranchId { get; set; }

        public int? DepartmentId { get; set; }

        public int? JobTitleId { get; set; }

        public string PhotoUrl { get; set; }
    }
}
