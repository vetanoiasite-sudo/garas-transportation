

using Microsoft.AspNetCore.Components.Routing;
using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.DTO.HrUser;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.TransportationLine;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using System.Threading.Tasks;
using System.Web;

namespace NewGarasAPI.Controllers.TransportationLine
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;
        private Helper.Helper _helper;
        private GarasTestContext _Context;
        private readonly ITenantService _tenantService;
        private readonly ITransportationLineService _transportationLineService;

        public TransportationController(IUnitOfWork unitOfWork , IWebHostEnvironment host , ITenantService tenantService  , ITransportationLineService transportationLineService)
        {
            _host=host;
            _unitOfWork=unitOfWork;
            _helper=new Helper.Helper();
            _tenantService=tenantService;
            _transportationLineService=transportationLineService;
            _Context=new GarasTestContext(_tenantService);
        }

        // VehicleType 
        [HttpGet("getAllVehicleType")]
        public BaseResponseWithData<List<VehicleType>> getAllVehicleType()
        {
            BaseResponseWithData<List<VehicleType>> response = new BaseResponseWithData<List<VehicleType>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {
                var v_Bus_Program = _unitOfWork.VBusProgram.FindAll(x=>true).ToList();
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    var data = _transportationLineService.getAllVehicleType();
                    response.Data=data.Data;

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        // TransportationLine
        [HttpGet("getAllTransportationLine")]
        public BaseResponseWithData<List<transportionLineNameVn>> getAllTransportationLine([FromHeader] string? Name)
        {
            BaseResponseWithData<List<transportionLineNameVn>> response = new BaseResponseWithData<List<transportionLineNameVn>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    var data = _transportationLineService.getAllTransportationLine(Name);
                    response.Data=data.Data;

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpGet("Bus_Program")]
        public BaseResponseWithData<List<VBusProgram>> Bus_Program()
        {
            BaseResponseWithData<List<VBusProgram>> response = new BaseResponseWithData<List<VBusProgram>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.Bus_Program();

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddVehicleType")]
        public BaseResponse AddVehicleType(VehicleTypeVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.AddVehicleType(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdateVehicleType")]
        public BaseResponse UpdateVehicleType(VehicleTypeVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.UpdateVehicleType(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpPost("DeleteVehicleType")]
        public BaseResponse DeleteVehicleType([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteVehicleType(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        // TransportationVehicle
        [HttpGet("getAllTransportationVehicle")]
        public IActionResult getAllTransportationVehicle([FromHeader] long RouteId , [FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                                       , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>> response = new BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllTransportationVehicle(RouteId , TransportionlineId , SupplierId , supplierContactPersonId , DateSerach , serialBus , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("AddTransportationVehicle")]
        public BaseResponse AddTransportationVehicle(TransportationVehicleVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddTransportationVehicle(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdateTransportationVehicle")]
        public BaseResponse UpdateTransportationVehicle(TransportationVehicleVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateTransportationVehicle(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }



        [HttpPost("UpdateRouteEmployee")]
        public BaseResponse UpdateRouteEmployee(UpdateRouteEmployeeVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateRouteEmployee(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("DeleteTransportationVehicle")]
        public BaseResponse DeleteTransportationVehicle([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteTransportationVehicle(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("DeleteRouteEmployee")]
        public BaseResponse DeleteRouteEmployee([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteRouteEmployee(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        // TransportationLine
        [HttpGet("getAllRoutes")]
        public IActionResult getAllRoutes([FromHeader] long RouteId , [FromHeader] string Name , [FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                          , [FromHeader] string serialBus , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20 , bool Active = true)
        {
            BaseResponseWithDataAndHeader<List<TransportationLineVM>> response = new BaseResponseWithDataAndHeader<List<TransportationLineVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllRoutes(RouteId , Name , TransportionlineId , SupplierId , supplierContactPersonId , serialBus , PageNo , NoOfItems , Active);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("AddTransportationLine")]
        public BaseResponse AddTransportationLine(TransportationLineVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddTransportationLine(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdateTransportationLine")]
        public BaseResponse UpdateTransportationLine(TransportationLineVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateTransportationLine(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("DeleteTransportationLine")]
        public BaseResponse DeleteTransportationLine([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteTransportationLine(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        // "Approve
        [HttpPost("ApproveRoute")]
        public BaseResponse ApproveRoute([FromHeader] long Id , [FromHeader] bool Approve)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.ApproveRoute(Id , Approve);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("ApproveTransportationVehicle")]
        public BaseResponse ApproveTransportationVehicle([FromHeader] int Id , [FromHeader] bool Approve)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.ApproveTransportationVehicle(Id , Approve);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        // TransportationVehicleRoute
        [HttpPost("AddTransportationRoute")]
        public async Task<BaseResponse> AddTransportationRoute(TransportationVehicleRouteVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.AddTransportationRoute(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("UpdateTransportationRoute")]
        public BaseResponse UpdateTransportationRoute(TransportationRouteUpdateVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateTransportationRoute(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("DeleteTransportationRoute")]
        public BaseResponse DeleteTransportationRoute([FromHeader] long Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteTransportationRoute(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddTransportationDirection")]
        public BaseResponse AddTransportationDirection(TransportationDirectionDto dto , [FromHeader] long TransportationVehicleRouteId)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddTransportationDirection(dto , TransportationVehicleRouteId);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdateTransportationDirection")]
        public BaseResponse UpdateTransportationDirection(UpdateTransportationDirectionVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateTransportationDirection(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("DeleteTransportationDirection")]
        public BaseResponse DeleteTransportationDirection([FromHeader] long Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteTransportationDirection(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpGet("getTransportationRouteDetails")]
        public IActionResult getTransportationRouteDetails([FromHeader] int RouteId , [FromHeader] DateTime FromDate , [FromHeader] DateTime ToDate)
        {
            BaseResponseWithData<TransportationRouteDetailsVM> response = new BaseResponseWithData<TransportationRouteDetailsVM>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getTransportationRouteDetails(RouteId , FromDate , ToDate);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }
        [HttpPost("AddTransportationEmployee")]
        public BaseResponse AddTransportationEmployee(TransportationEmployeeDto dto , [FromHeader] long TransportationVehicleRouteId)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddTransportationEmployee(dto , TransportationVehicleRouteId);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("AddRoutesForEmployee")]
        public BaseResponse AddRoutesForEmployee(AddRoutesForHrUserDto dto , [FromHeader] long HrUserId)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddRoutesForEmployee(dto , HrUserId);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("UpdateTransportationEmployee")]
        public BaseResponse UpdateTransportationEmployee(UpdateTransportationEmployeeVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateTransportationEmployee(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("DeleteTransportationEmployee")]
        public BaseResponse DeleteTransportationEmployee([FromHeader] long Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DeleteTransportationEmployee(Id);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdatePriceOfTransportationLine")]
        public IActionResult UpdatePriceOfTransportationLine([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdatePriceOfTransportationLine(Id);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("RejectUpdatePrice")]
        public IActionResult RejectUpdatePrice([FromHeader] int Id)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.RejectUpdatePrice(Id);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("ModifyPriceOfTransportationLine")]
        public async Task<BaseResponse> ModifyPriceOfTransportationLine(ModifyTransportationLinePriceVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.ModifyPriceOfTransportationLine(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpGet("getAllModifyPriceOfTransportationLine")]
        public IActionResult getAllModifyPriceOfTransportationLine([FromHeader] bool? Approve , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>> response = new BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllModifyPriceOfTransportationLine(PageNo , NoOfItems , Approve);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("DashBoard")]
        public IActionResult DashBoard([FromHeader] long RouteId , [FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                       , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus)
        {
            BaseResponseWithData<DashBoardVM> response = new BaseResponseWithData<DashBoardVM>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DashBoard(RouteId , TransportionlineId , SupplierId , supplierContactPersonId , DateSerach , serialBus);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("DashBoardForAttendence")]
        public IActionResult DashBoardForAttendence([FromHeader] int TransportionlineId , [FromHeader] int RouteId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                      , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] bool? AttendaceFlag , [FromHeader] long HrUser , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> response = new  BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DashBoardForAttendence(TransportionlineId , RouteId , SupplierId , supplierContactPersonId , DateSerach , serialBus , FromDate , ToDate , AttendaceFlag , HrUser , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }
        [HttpGet("DashBoardForAttendenceDuration")]
        public IActionResult DashBoardForAttendence99([FromHeader] int TransportionlineId , [FromHeader] int RouteId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                      , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] bool? AttendaceFlag , [FromHeader] long HrUser , [FromHeader] string? Email , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>> response = new  BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.DashBoardForAttendenceDuration(TransportionlineId , RouteId , SupplierId , supplierContactPersonId , DateSerach , serialBus , FromDate , ToDate , AttendaceFlag , HrUser , Email , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("getTransportationRoutesForHrUser")]
        public IActionResult getTransportationRoutesForHrUser([FromHeader] long HrUserId)
        {
            BaseResponseWithData<List<getTransportationRoutesForHrUserVm>> response = new BaseResponseWithData<List<getTransportationRoutesForHrUserVm>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getTransportationRoutesForHrUser(HrUserId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("AddUsersAttedance")]
        public BaseResponse AddUsersAttedance(TransprotationUserAttedanceVM dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddUsersAttedance(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("AddUsersAttedance44")]
        public BaseResponse AddUsersAttedance44()
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddUsersAttedance66();

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("AlterAddUsersAttendance")]
        public async Task<BaseResponse> AlterAddUsersAttendance()
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.AlterAddUsersAttendance();

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }
        [HttpPost("AddDeduction")]
        public BaseResponse AddDeduction(TransportationVehicleRouteDeduction dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddDeduction(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpGet("AttendanceExcell")]
        public async Task<IActionResult> AttendanceExcell([FromHeader] int Year , [FromHeader] int TransportionlineId , [FromHeader] long RouteId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                      , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] bool? AttendaceFlag , [FromHeader] long HrUser , [FromHeader] string? Email , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=await _transportationLineService.AttendanceExcell(Year , TransportionlineId , RouteId , SupplierId , supplierContactPersonId , DateSerach , serialBus , FromDate , ToDate , AttendaceFlag , HrUser , Email , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("ReportCostOfLines")]
        public IActionResult ReportCostOfLines([FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                    , [FromHeader] DateTime? dateSearch , [FromHeader] DateTime? dateFrom , [FromHeader] DateTime? dateTo , [FromHeader] string serialBus , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>> response = new BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.CostOfLine(TransportionlineId , SupplierId , supplierContactPersonId , dateSearch , dateFrom , dateTo , serialBus , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("ReportCostOfLinesExcell")]
        public IActionResult ReportCostOfLinesExcell([FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                   , [FromHeader] DateTime? dateSearch , [FromHeader] DateTime? dateFrom , [FromHeader] DateTime? dateTo , [FromHeader] string serialBus , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithData<string> response = new  BaseResponseWithData<string> ();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.ReportCostOfLinesExcell(TransportionlineId , SupplierId , supplierContactPersonId , dateSearch , dateFrom , dateTo , serialBus , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }



        // get pagelist route 
        [HttpGet("getAllTransportationRoute")]
        public IActionResult getAllTransportationRoute([FromHeader] long RouteId , [FromHeader] string Name , [FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                          , [FromHeader] string serialBus , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20 , bool Active = true)
        {
            BaseResponseWithDataAndHeader<List<TransportationRouteVM>> response = new BaseResponseWithDataAndHeader<List<TransportationRouteVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllTransportationRoute(RouteId , Name , TransportionlineId , SupplierId , supplierContactPersonId , serialBus , PageNo , NoOfItems , Active);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }
        [HttpGet("getTransportationRoute")]
        public IActionResult getTransportationRoute([FromHeader] long RouteId)
        {
            BaseResponseWithData<TransportationRouteVM> response = new BaseResponseWithData<TransportationRouteVM>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getTransportationRoute(RouteId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("GetTransportationDirection")]
        public IActionResult GetTransportationDirection([FromHeader] long routeId)
        {
            BaseResponseWithData<List<TransportationDirectionByRouteIdVm>> response = new BaseResponseWithData<List<TransportationDirectionByRouteIdVm>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.GetTransportationDirection(routeId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        // get pagelist route 
        [HttpGet("GetTransportationEmployeeList")]
        public IActionResult GetTransportationEmployeeList([FromHeader] long RouteId)
        {
            BaseResponseWithData<List<EmployeeBasicInfoVM>> response = new BaseResponseWithData<List<EmployeeBasicInfoVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.GetTransportationEmployeeList(RouteId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("getTransportationRouteByHrUser")]
        public IActionResult getTransportationRouteByHrUser([FromHeader] long HrUserId)
        {
            BaseResponseWithData<List<TransportationRouteVM>> response = new BaseResponseWithData<List<TransportationRouteVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getTransportationRouteByHrUser(HrUserId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("GetEmployeeException")]
        public IActionResult GetEmployeeException([FromHeader] long? HrUserId , [FromHeader] long? Id , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>> response = new BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.GetEmployeeException(HrUserId , Id , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("AddException")]
        public BaseResponse AddException(TransportationVehicleRouteEmployeeException dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddException(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("UpdateException")]
        public BaseResponse UpdateException(TransportationVehicleRouteEmployeeException dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateException(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpGet("GetCapacityNumbers")]
        public IActionResult GetCapacityNumbers([FromHeader] long RouteID , [FromHeader] string Period , [FromHeader] DateTime? DateSearch)
        {
            BaseResponseWithData<CapacityTransportionNumbersVM> response = new BaseResponseWithData<CapacityTransportionNumbersVM>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.GetCapacityNumbers(RouteID , Period , DateSearch);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("GetTransportationEmployees")]
        public IActionResult GetTransportationEmployees([FromHeader] long RouteID , [FromHeader] string Period , [FromHeader] DateTime? DateSearch)
        {
            BaseResponseWithData<List<EmployeeBasicInfoVM>> response = new BaseResponseWithData<List<EmployeeBasicInfoVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.GetTransportationEmployees(RouteID , Period , DateSearch);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        // get pagelist route 
        [HttpGet("getAllDeduction")]
        public IActionResult getAllDeduction([FromHeader] int Id , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] long SupplierId , [FromHeader] long RouteId , [FromHeader] string? Name , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<RouteDeductionVM>> response = new   BaseResponseWithDataAndHeader<List<RouteDeductionVM>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllDeduction(Id , FromDate , ToDate , SupplierId , RouteId , Name , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("UpdateDeduction")]
        public BaseResponse UpdateDeduction(TransportationVehicleRouteDeduction dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.UpdateDeduction(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpPost("InsertPharmaExcel")]
        public async Task<IActionResult> InsertPharmaExcel([FromForm] InsertHrUserExcelVM dto)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.InsertPharmaExcel(dto);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                response.Errors.Add(error);
                return BadRequest(response);
            }
        }



        [HttpGet("AttendanceExcellDur")]
        public async Task<IActionResult> AttendanceExcellDur([FromHeader] int Year , [FromHeader] int TransportionlineId , [FromHeader] long RouteId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                        , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] bool? AttendaceFlag , [FromHeader] long HrUser , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 20)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=await _transportationLineService.AttendanceExcellDur(Year , RouteId , SupplierId , supplierContactPersonId , DateSerach , serialBus , FromDate , ToDate , AttendaceFlag , HrUser , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpPost("AddUsersAttedanceUpdated")]
        public BaseResponse AddUsersAttedanceUpdated()
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddUsersAttedanceUpdated();

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }


        [HttpPost("CreateHrUserWithAllRoutes")]
        public async Task<BaseResponse> CreateHrUserWithAllRoutes([FromForm] CreateHrUserWithAllRoutesDto NewHrUser)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.CreateHrUserWithAllRoutes(NewHrUser , validation.userID , validation.CompanyName);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }



        [HttpGet("downloadExcelUserActiveTemplete")]
        public IActionResult downloadExcelUserActiveTemplete()
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.downloadExcelUserActiveTemplete();

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("downloadExcelRoutesTemplete")]
        public IActionResult downloadRoutesTemplete()
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.downloadExcelRoutesTemplete();

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("downloadExcelUserWithRoutesTemplete")]
        public IActionResult downloadExcelUserWithRoutesTemplete()
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.downloadExcelUserWithRoutesTemplete();

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("InsertUsersWithRoutesExcel")]
        public async Task<IActionResult> InsertUsersWithRoutesExcel([FromForm] IFormFile file)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.InsertUsersWithRoutesExcel(file);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpPost("InsertUserNotActiveExcel")]
        public async Task<IActionResult> InsertUserNotActiveExcel([FromForm] IFormFile file)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.InsertUsersWithRoutesExcel(file);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("AccountsAllMonthsForSupplier")]
        public IActionResult AccountsAllMonthsForSupplier([FromHeader] int Year , [FromHeader] long SupplierId , [FromHeader] long RouteId , [FromHeader] int Month , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountDto>> response = new BaseResponseWithDataAndHeader<List<AccountDto>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.AccountsAllMonths22(Year , SupplierId , RouteId , Month , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }



        [HttpGet("AccountsAllRoundsForSupplier")]
        public IActionResult AccountsAllRoundsForSupplier([FromHeader] int Year , [FromHeader] long SupplierId , [FromHeader] long RouteId , [FromHeader] int Month , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>> response = new BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.AccountsAllRoundsForSupplier(Year , SupplierId , RouteId , Month , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("getAllSupplierPayment")]
        public IActionResult getAllSupplierPayment([FromHeader] long SupplierId , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<SupplierPaymentDto>> response = new BaseResponseWithDataAndHeader<List<SupplierPaymentDto>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    response=_transportationLineService.getAllSupplierPayment(SupplierId , FromDate , ToDate , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpPost("AddSupplierPayment")]
        public BaseResponse AddSupplierPayment(TransportionLineSupplierPayment dto)
        {
            BaseResponse response = new BaseResponse();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=_transportationLineService.AddSupplierPayment(dto);

                }
                return response;
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return response;
            }
        }

        [HttpGet("UpdateRoutePrice")]
        public IActionResult UpdateRoutePrice()
        {
            BaseResponse response = new BaseResponse ();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    var test = DeviceIdentifier.GetUniqueId();
                    response=_transportationLineService.UpdateRoutePrice();

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;

                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("RoutesWithUsersExcell")]
        public async Task<IActionResult> RoutesWithUsersExcell([FromHeader] long? RouteId , [FromHeader] long? SupplierId)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.RoutesWithUsersExcell(RouteId , SupplierId);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }


        [HttpGet("getAllDeductionExcel")]
        public async Task<IActionResult> getAllDeductionExcel([FromHeader] int Id , [FromHeader] DateTime? FromDate , [FromHeader] DateTime? ToDate , [FromHeader] long SupplierId , [FromHeader] long RouteId , [FromHeader] string? Name , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 1000)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.getAllDeductionExcel(Id , FromDate , ToDate , SupplierId , RouteId , Name , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }
        [HttpGet("AccountsAllMonthsForSupplierExcel")]
        public async Task<IActionResult> AccountsAllMonthsForSupplierExcel([FromHeader] int Year , [FromHeader] long SupplierID , [FromHeader] long RouteID , [FromHeader] int Month , [FromHeader] int PageNo = 1 , [FromHeader] int NoOfItems = 5000)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.AccountsAllMonthsForSupplierExcel(Year , SupplierID , RouteID , Month , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

        [HttpGet("BusAttendance")]
        public async Task<IActionResult> BusAttendance([FromHeader] DateTime? dateSearch ,
        [FromHeader] DateTime? FromDate ,
        [FromHeader] DateTime? ToDate ,
        [FromHeader] long SupplierId = 0 ,
        [FromHeader] long supplierContactPersonId = 0 ,
        [FromHeader] long RouteId = 0 ,
        [FromHeader] string? Name = null ,
        [FromHeader] string? serialBus = null ,
        [FromHeader] int PageNo = 1 ,
        [FromHeader] int NoOfItems = 20)
        {
                BaseResponseWithDataAndHeader<List<BusAttendanceLineVm>> response = new BaseResponseWithDataAndHeader<List<BusAttendanceLineVm>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.BusAttendance(
        dateSearch ,
        FromDate ,
        ToDate ,
        SupplierId ,
        supplierContactPersonId ,
        RouteId ,
        Name ,
        serialBus ,
        PageNo ,
        NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }



        [HttpGet("BusAttendanceExcel")]
        public async Task<IActionResult> BusAttendanceExcel([FromHeader] DateTime? dateSearch ,
       [FromHeader] DateTime? FromDate ,
       [FromHeader] DateTime? ToDate ,
       [FromHeader] long SupplierId = 0 ,
       [FromHeader] long supplierContactPersonId = 0 ,
       [FromHeader] long RouteId = 0 ,
       [FromHeader] string? Name = null ,
       [FromHeader] string? serialBus = null ,
       [FromHeader] int PageNo = 1 ,
       [FromHeader] int NoOfItems = 10000)
        {
            BaseResponseWithData<string> response = new BaseResponseWithData<string>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    _transportationLineService.Validation=validation;
                    response=await _transportationLineService.BusAttendanceExcel(
        dateSearch ,
        FromDate ,
        ToDate ,
        SupplierId ,
        supplierContactPersonId ,
        RouteId ,
        Name ,
        serialBus ,
        PageNo ,
        NoOfItems);


                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }




        [HttpGet("getAllUsers")]
        public IActionResult getAllUsers([FromHeader] string userName , [FromHeader] string Email , [FromHeader] long RouteId , [FromHeader] int TransportionlineId , [FromHeader] long SupplierId , [FromHeader] long supplierContactPersonId
                                    , [FromHeader] DateTime? DateSerach , [FromHeader] string serialBus , [FromHeader] int PageNo , [FromHeader] int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<HrUserCardDto>> response = new BaseResponseWithDataAndHeader<List<HrUserCardDto>>();
            response.Result=true;
            response.Errors=new List<Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors=validation.errors;
                response.Result=validation.result;

                if(response.Result)
                {
                    var userNameDecode = HttpUtility.UrlDecode(userName);

                    response=_transportationLineService.getAllUsers(userNameDecode , Email , RouteId , TransportionlineId , SupplierId , supplierContactPersonId , DateSerach , serialBus , PageNo , NoOfItems);

                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Result=false;
                Error error = new Error();
                error.ErrorCode="Err10";
                error.ErrorMSG=ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                ;
                response.Errors.Add(error);

                return BadRequest(response);
            }
        }

    }
}
