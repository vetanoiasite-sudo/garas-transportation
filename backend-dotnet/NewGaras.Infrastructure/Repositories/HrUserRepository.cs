using NewGaras.Domain.Interfaces.Repositories;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Repositories
{
    public class HrUserRepository : BaseRepository<HrUser, long>, IHrUserRepository
    {
        private readonly GarasTestContext _context;

        public HrUserRepository(GarasTestContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<List<List<HrDuplicatesDto>>> CheckDuplicates(CheckDuplicatesDto H)
        //{
        //    List<HrDuplicatesDto> duplicatesArNames, duplicatesNames, duplicatesEmails, duplicatesMobiles;
        //    duplicatesArNames = duplicatesNames = duplicatesEmails = duplicatesMobiles = new List<HrDuplicatesDto>();
        //    if (H.FirstName is not null && H.MiddleName is not null && H.LastName is not null)
        //    {
        //        duplicatesNames = _context.HrUsers.Where(x => x.LastName.Equals(H.LastName) && x.FirstName.Equals(H.FirstName) && x.MiddleName.Equals(H.MiddleName)).Select(x => new HrDuplicatesDto { Email = x.Email, Id = x.Id }).ToList();
        //    }
        //    if (H.ARFirstName is not null && H.ARMiddleName is not null && H.ARLastName is not null)
        //    {
        //        duplicatesArNames = _context.HrUsers.Where(x => x.ARFirstName == H.ARFirstName && x.ARMiddleName == H.ARMiddleName && x.ARLastName == H.ARLastName).Select(x => new HrDuplicatesDto { Email = x.Email, Id = x.Id }).ToList();
        //    }
        //    if (H.Email is not null)
        //    {
        //        duplicatesEmails = _context.HrUsers.Where(x => x.Email == H.Email).Select(x => new HrDuplicatesDto { Email = x.Email, Id = x.Id }).ToList();
        //    }
        //    if (H.Mobile is not null)
        //    {
        //        duplicatesMobiles = _context.HrUsers.Where(x => x.Mobile.Equals(H.Mobile)).Select(x => new HrDuplicatesDto { Email = x.Email, Id = x.Id }).ToList();
        //    }
        //    return new List<List<HrDuplicatesDto>> { duplicatesNames, duplicatesArNames, duplicatesEmails, duplicatesMobiles };
        //}

        /*public List<HrUser> CheckDuplicates_ARName(string arfirstname, string armiddlename, string arlastname)
        {
           var users =  _context.HrUsers.Where(x => x.ARFirstName == arfirstname && x.ARMiddleName == armiddlename && x.ARLastName == arlastname);
            return users.ToList();
        }

        public List<HrUser> CheckDuplicates_Email(string email)
        {
            var users = _context.HrUsers.Where(x => x.Email == email);
            return users.ToList();
        }

        public List<HrUser> CheckDuplicates_Mobile(string mobile)
        {
            var users = _context.HrUsers.Where(x => x.Mobile == mobile);
            return users.ToList();
        }

        public List<HrUser> CheckDuplicates_Name(string firstname, string middlename, string lastname)
        {
            var users = _context.HrUsers.Where(x => x.LastName.Equals(lastname) && x.FirstName.Equals(firstname) && x.MiddleName.Equals(middlename));
            return users.ToList();
        }*/
    }

}
