using Microsoft.AspNetCore.Mvc;
using NewGaras.Infrastructure.Entities;
using NewGarasAPI.Helper;
using NewGarasAPI.Models.Common;
using NewGarasAPI.Models.Notification;
using static Azure.Core.HttpHeader;
using System.Net;
using System.Web;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Models.Notification;
using NewGaras.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewGarasAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private GarasTestContext _Context;
        private Helper.Helper _helper;
        static string key;
        private readonly ITenantService _tenantService;
        private readonly INotificationService _notificationService;

        public NotificationController(ITenantService tenantService,INotificationService notificationService) 
        {
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            _helper = new Helper.Helper();
            key = "SalesGarasPass";
            _notificationService = notificationService;
        }

        // GET: <Notification>/GetNotifications
        [HttpGet("GetNotifications")]
        public GetNotificationsResponse GetNotifications(GetNotificationsFilters filters)
        {
            GetNotificationsResponse response = new GetNotificationsResponse();
            response.Result = true;
            response.Errors = new List<Error>();
            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers,ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    _notificationService.Validation = validation;
                    response = _notificationService.GetNotifications(filters);
                }
                return response; 
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }
        [HttpPost("AddNewNotification")]
        public BaseResponseWithID AddNewNotification(UserNotificationRequest UserNotification)
        {
            BaseResponseWithID Response = new BaseResponseWithID();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                //IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                //WebHeaderCollection headers = request.Headers;
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers,ref _Context);
                Response.Errors = validation.errors;
                Response.Result = validation.result;

                if (Response.Result)
                {
                    /*
 ##########################################################################################
########################### NOTE ########################################################
Using ToUserID as FromUserID 
Using FromUserID as ToUserID

       because must be there sender (From UserID ) but reciver not mandatory if null (meaning send this notification to all)

       ##########################################################################################
###############################################################################################
 */
                    //check sent data
                    if (UserNotification == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }

                    



                    if (string.IsNullOrEmpty(UserNotification.Title))
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Title Is Mandatory";
                        Response.Errors.Add(error);
                    }

                   


                    if (Response.Result)
                    {
                        // Ignore From User in Request
                        long NotificationId = _notificationService.CreateNotification(validation.userID, UserNotification.Title, UserNotification.Description, UserNotification.URL, UserNotification.New, UserNotification.ToUserID, UserNotification.NotificationProcessID);

                        if (NotificationId > 0)
                        {
                            Response.ID = NotificationId;
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }

        [HttpPost("EditNotifications")]
        public async Task<BaseResponseWithId<long>> EditNotifications(UserNotification request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {               
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers,ref _Context);
                Response.Errors = validation.errors;
                Response.Result = validation.result;

                if (Response.Result)
                {
                    _notificationService.Validation = validation;
                    Response = await _notificationService.EditNotifications(request);
                }

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }


        [HttpGet("GetNotificationProcessDDL")]
        public async Task<GetNotificationProcessDDLResponse> GetNotificationProcessDDL()
        {
            GetNotificationProcessDDLResponse response = new GetNotificationProcessDDLResponse();
            response.Result = true;
            response.Errors = new List<Error>();
            try
            {
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;
                if (response.Result)
                {
                    response = await _notificationService.GetNotificationProcessDDL();

                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }
        }
    }
}
