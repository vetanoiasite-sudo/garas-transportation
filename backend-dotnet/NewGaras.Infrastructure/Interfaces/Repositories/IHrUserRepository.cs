using NewGaras.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NewGaras.Domain.Interfaces.Repositories
{
    public interface IHrUserRepository : IBaseRepository<HrUser, long>
    {
        //Task<List<List<CheckHrUserDuplicatesResponse>>> CheckDuplicates(CheckDuplicatesModel H);

        //void AssignToTeam(long Userid, long TeamId);
    }
}
