using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure.DTO.HrUser;
using NewGaras.Infrastructure.DTO.VacationType;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.HrUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface IHrUserService
    {
        public HearderVaidatorOutput Validation { get; set; }
        public Task<BaseResponseWithData<UserEmployeeResponse>> AddHrEmployeeToUserAsync(AddHrEmployeeToUserDTO InData, long userId, string key);
        public Task<BaseResponseWithData<GetHrUserDto>> GetHrUser(long HrUserId, long systemUserId);

        public Task<BaseResponseWithDataAndHeader<List<HrUserCardDto>>> GetAll(int CurrentPage, int NumberOfItemsPerPage, string? name,
            bool? active, int? DepId, int? jobTilteId, int? BranchId, bool? isUser, string? Email, string? mobile, bool? isDeleted
            , bool? ActiveUser  ,int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus);

        public BaseResponseWithDataAndHeader<HrUserListDDL> GetHrUserListDDl(int CurrentPage, int NumberOfItemsPerPage, string? searchKey, long? DoctorSpecialtyId);
        public Task<BaseResponseWithId<long>> CreateHrUser(HrUserDto NewHrUser, long UserId,string CompanyName);

        public Task<BaseResponseWithData<UserDataResponse>> EditHrEmployee(EditHrEmployeeDto NewHrData, long userId, string CompanyName, string key);

        public BaseResponseWithData<List<GetHrTeamUsersDto>> GetHrTeamUsers(long TeamId);

        public Task<BaseResponseWithData<List<HrUserJobTitleDto>>> GetAllUsersWithJobTitle(int? JobTitleId = null);

        public Task<BaseResponseWithData<List<HrUsersWithJobTitleNameImage>>> GetHrUsersWithJobTitleNameImage(int JobTitleId);
        public Task<BaseResponseWithId<long>> RetriveDeletedUser(long id);
        public BaseResponseWithData<GetAbsenceHistoryModel> GetAbsenceHistoryForUser(GetAbsenceHistoryRequest request);

        public BaseResponseWithData<string> GetUsersReportExcell(bool? Active, int? DeptID, long? teamID, string CompName, bool? IsUser);




        
        public ActionResult<SelectDDLResponse> GetAbsenceTypeList();

      

        public Task<BaseResponseWithId<long>> CreateHrUserWorker(AddHrUserWorker Worker, long UserId);
    }
}
