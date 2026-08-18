using AutoMapper;

using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.DTO.HrUser;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.HR;
using System.Web;

namespace NewGarasAPI.Controllers.HR
{
    // Profile CRUD for HR users (the transportation passengers). Routes, header
    // names and response envelopes are the same as the CoreApi's HrUserController;
    // only the endpoints the transportation module uses are exposed here —
    // vacations, attachments, documents and job titles belong to the HR module.
    [Route("[controller]")]
    [ApiController]
    public class HrUserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private Helper.Helper _helper;
        static string key;
        private GarasTestContext _Context;
        private readonly IMapper _mapper;
        private readonly IHrUserService _hrUserService;
        private readonly ITenantService _tenantService;

        public HrUserController(IUnitOfWork unitOfWork, IWebHostEnvironment host, IMapper mapper, IHrUserService hrUserService, ITenantService tenantService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _host = host;
            _helper = new Helper.Helper();
            key = "SalesGarasPass";
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            _hrUserService = hrUserService;
        }

        [HttpGet("GetHrUser")]  //services Added
        public async Task<BaseResponseWithData<GetHrUserDto>> GetHrUser([FromHeader] long HrUserId, [FromHeader] long systemUserId)
        {
            var response = new BaseResponseWithData<GetHrUserDto>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            try
            {
                if (response.Result)
                {
                    response = await _hrUserService.GetHrUser(HrUserId, systemUserId);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        [HttpGet("GetUserCards")]   //services Added
        public async Task<BaseResponseWithDataAndHeader<List<HrUserCardDto>>> GetAll([FromHeader] string userName, [FromHeader] bool? active
            , [FromHeader] int? DepId, [FromHeader] int? jobTilteId, [FromHeader] int? BranchId, [FromHeader] bool? IsUser, [FromHeader] string Email, [FromHeader] string mobile
            , [FromHeader] bool? isDeleted, [FromHeader] bool? ActiveUser, [FromHeader] int TransportionlineId, [FromHeader] long SupplierId, [FromHeader] long supplierContactPersonId
            , [FromHeader] DateTime? DateSerach, [FromHeader] string serialBus, [FromHeader] int currentPage = 1, [FromHeader] int numberOfItemsPerPage = 10)
        {   //userName is the serachKey in the view
            var response = new BaseResponseWithDataAndHeader<List<HrUserCardDto>>();
            response.Result = true;
            response.Errors = new List<Error>();

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            try
            {
                if (response.Result)
                {
                    var userNameDecode = HttpUtility.UrlDecode(userName);
                    response = await _hrUserService.GetAll(currentPage, numberOfItemsPerPage, userNameDecode, active,
                        DepId, jobTilteId, BranchId, IsUser, Email, mobile, isDeleted, ActiveUser, TransportionlineId, SupplierId, supplierContactPersonId, DateSerach, serialBus);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        [HttpPost("CreateHrUser")]      //services Added
        public async Task<BaseResponseWithId<long>> CreateHrUsers([FromForm] HrUserDto NewHrUser)
        {
            var response = new BaseResponseWithId<long>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            try
            {
                if (response.Result)
                {
                    response = await _hrUserService.CreateHrUser(NewHrUser, validation.userID, validation.CompanyName);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        [HttpPost("AddHrEmployeeToUser")]    //services Added
        public async Task<BaseResponseWithID> AddHrEmployeeToUser([FromBody] AddHrEmployeeToUserDTO InData)
        {
            var response = new BaseResponseWithID()
            {
                Result = true,
                Errors = new List<Error>()
            };

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            try
            {
                if (response.Result)
                {
                    var responseService = await _hrUserService.AddHrEmployeeToUserAsync(InData, validation.userID, key);
                    response.Result = responseService.Result;
                    response.Errors = responseService.Errors;
                    response.ID = responseService.Data?.Id ?? 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        [HttpPost("EditHrEmployee")]    //services Added
        public async Task<BaseResponseWithId<long>> EditHrEmployee([FromForm] EditHrEmployeeDto NewHrData)
        {
            var response = new BaseResponseWithId<long>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            try
            {
                if (response.Result)
                {
                    var UserCheck = await _hrUserService.GetHrUser(NewHrData.HrUserId, 0);
                    if (!UserCheck.Result)
                    {
                        response.Result = false;
                        response.Errors.AddRange(UserCheck.Errors);
                        return response;
                    }
                    var responseData = await _hrUserService.EditHrEmployee(NewHrData, validation.userID, validation.CompanyName, key);

                    response.ID = responseData.Data?.HRUserId ?? 0;
                    response.Result = responseData.Result;
                    response.Errors = responseData.Errors;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }
    }
}
