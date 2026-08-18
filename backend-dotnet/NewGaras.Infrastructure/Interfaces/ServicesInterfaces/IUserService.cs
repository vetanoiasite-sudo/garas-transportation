using NewGaras.Domain.Models;
using NewGaras.Infrastructure.DTO.User;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models.User;
using NewGarasAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        public Task<BaseResponseWithData<List<UserWithJobTitleDDL>>> GetUserWithJobTitleDDL(string UserName, int? BranchID, long? projectId);

        public Task<GetUserTargetDistributionResponse> GetUserTargetDistribution([FromHeader] int BranchId, [FromHeader] long Year);

        public UserDDLResponse GetUserList(int BranchId, int RoleId, long GroupId, string JobTitleId, bool NotActiveUser = false, bool WithTeam = false);
        public Task<LoginResponse> GetUserData(long userID, string UserToken);
    }
}
