using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Models.Notification;
using NewGarasAPI.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewGaras.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private GarasTestContext _Context;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;
        private readonly IUnitOfWork _unitOfWork;
        private HearderVaidatorOutput validation;
        public HearderVaidatorOutput Validation
        {
            get
            {
                return validation;
            }
            set
            {
                validation = value;
            }
        }
        public NotificationService(ITenantService tenantService, IMapper mapper, IWebHostEnvironment host, IUnitOfWork unitOfWork)
        {

            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            _mapper = mapper;
            _host = host;
            _unitOfWork = unitOfWork;
        }
        public long CreateNotification(long ToUserID, string _Title, string _Description, string _URL, bool _New, long? _FromUserID, int? _NotificationProcessID)
        {
            /*
            ##########################################################################################
            ########################### NOTE ########################################################
            Using ToUserID column as FromUserID 
            Using FromUserID column as ToUserID

            because must be there sender (From UserID ) but reciver not mandatory if null (meaning send this notification to all)

            ##########################################################################################
            ###############################################################################################
            */
            long NotificationID = 0;
            SqlParameter IDNotification = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.BigInt,
                ParameterName = "ID",
                Direction = System.Data.ParameterDirection.Output
            };
            SqlParameter UserID = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.BigInt,
                ParameterName = "UserID",
                Value = ToUserID
            };
            SqlParameter Title = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.NVarChar,
                ParameterName = "Title",
                Value = _Title
            };
            SqlParameter Description = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.NText,
                ParameterName = "Description",
                Value = _Description
            };
            SqlParameter Date = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.DateTime,
                ParameterName = "Date",
                Value = DateTime.Now
            };
            SqlParameter URL = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.NVarChar,
                ParameterName = "URL",
                Value = _URL
            };
            SqlParameter New = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.Bit,
                ParameterName = "New",
                Value = _New
            };
            SqlParameter FromUserId = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.BigInt,
                ParameterName = "FromUserId",
                Value = _FromUserID == 0 ? DBNull.Value : _FromUserID
            };
            SqlParameter NotificationProcessId = new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.Int,
                ParameterName = "NotificationProcessId",
                Value = _NotificationProcessID == 0 ? DBNull.Value : _NotificationProcessID
            };
            object[] param = new object[] {IDNotification,UserID,Title,Description,Date,URL,
                            New,FromUserId,NotificationProcessId};
            var NotificationInsert = _Context.Database.SqlQueryRaw<long>("Exec proc_NotificationInsert @ID output,@UserID ,@Title ,@Description ,@Date ,@URL ,@New ,@FromUserId ,@NotificationProcessId ", param).AsEnumerable().FirstOrDefault();
            //var NotificationInsert = _Context.proc_NotificationInsert(IDNotification,
            //                                                                ToUserID, // From USer ID
            //                                                                Title,
            //                                                                Description,
            //                                                                DateTime.Now,
            //                                                                URL,
            //                                                                New,
            //                                                                FromUserID, // To USerID
            //                                                                NotificationProcessID);


            if (IDNotification.Value != null)
            {
                NotificationID = (long)IDNotification.Value;
            }

            return NotificationID;
        }

        public async Task<BaseResponseWithId<long>> EditNotifications(UserNotification request)
        {
            BaseResponseWithId<long> Response = new BaseResponseWithId<long>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                if (Response.Result)
                {
                    if (request == null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "Please Insert a Valid Data.";
                        Response.Errors.Add(error);
                        return Response;
                    }
                    if (string.IsNullOrEmpty(request.ID.ToString()))
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Notification ID Is Mandatory";
                        Response.Errors.Add(error);
                    }
                    if (Response.Result)
                    {
                        var modifiedUser = NewGaras.Infrastructure.Common.GetUserName(validation.userID, _Context);

                        if (request.ID != null && request.ID != 0)
                        {
                            SqlParameter IDNotification = new SqlParameter
                            {
                                SqlDbType = System.Data.SqlDbType.BigInt,
                                ParameterName = "ID",
                                Value = request.ID
                            };
                            object[] param = new object[] { IDNotification };
                            var JobTitleDB = _Context.Database.SqlQueryRaw<proc_NotificationLoadByPrimaryKey_Result>("Exec proc_NotificationLoadByPrimaryKey @ID ", param).AsEnumerable().FirstOrDefault();
                            if (JobTitleDB != null)
                            {
                                var NotificationsUpdateDB = (await _unitOfWork.Notifications.FindAllAsync(x => x.Id == request.ID)).FirstOrDefault();


                                if (NotificationsUpdateDB != null)
                                {
                                    NotificationsUpdateDB.New = request.New;

                                    _unitOfWork.Complete();
                                }
                                else
                                {
                                    Response.Result = false;
                                    Error error = new Error();
                                    error.ErrorCode = "Err25";
                                    error.ErrorMSG = "Faild To Update this Notification!!";
                                    Response.Errors.Add(error);
                                }
                            }
                            else
                            {
                                Response.Result = false;
                                Error error = new Error();
                                error.ErrorCode = "Err25";
                                error.ErrorMSG = "This Notification Doesn't Exist!!";
                                Response.Errors.Add(error);
                            }
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
                error.ErrorMSG = ex.InnerException.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public async Task<GetNotificationProcessDDLResponse> GetNotificationProcessDDL()
        {
            GetNotificationProcessDDLResponse response = new GetNotificationProcessDDLResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                var GetNotificationProcessDDLList = new List<NotificationProcessDDLData>();

                if (response.Result)
                {

                    var GetNotificationProcessDDLListDB = await _unitOfWork.NotificationProcesses.GetAllAsync();

                    if (GetNotificationProcessDDLListDB != null)
                    {

                        foreach (var GetNotificationProcessDDLOBJ in GetNotificationProcessDDLListDB)
                        {
                            var GetNotificationProcessDDLResponse = new NotificationProcessDDLData();

                            GetNotificationProcessDDLResponse.ID = (int)GetNotificationProcessDDLOBJ.Id;

                            GetNotificationProcessDDLResponse.Name = GetNotificationProcessDDLOBJ.ProcessName;

                            GetNotificationProcessDDLList.Add(GetNotificationProcessDDLResponse);
                        }
                    }
                }
                response.NotificationProcessDDLlist = GetNotificationProcessDDLList;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException.Message;
                response.Errors.Add(error);

                return response;
            }

        }

        public GetNotificationsResponse GetNotifications(GetNotificationsFilters filters)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            GetNotificationsResponse response = new GetNotificationsResponse();
            response.Result = true;
            response.Errors = new List<Error>();
            try
            {
                if (response.Result)
                {
                    DateTime DateTemp = DateTime.Now;
                    DateTime? DateFrom = null;
                    if (filters.DateFrom!=null)
                    {
                        DateTemp = (DateTime)filters.DateFrom;
                        DateFrom = DateTemp;
                    }

                    DateTime? DateTo = null;
                    if (filters.DateTo!=null)
                    {
                        DateTemp = (DateTime)filters.DateTo;
                        DateTo = DateTemp;
                    }
                    if (response.Result)
                    {
                        var UserNotificationDB = _unitOfWork.VNotificationsDetails.FindAllQueryable(x => x.FromUserId == validation.userID || x.FromUserId == null).AsQueryable();
                        if (!string.IsNullOrWhiteSpace(filters.Title))
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.Title.Contains(filters.Title)).AsQueryable();
                        } 
                        if (filters.New != null)
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.New == filters.New).AsQueryable();
                        }
                        if (filters.FromUserID != 0) // Condition for Get From Who  FromUserId => ToUserId
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.ToUserId == filters.FromUserID).AsQueryable();
                        }

                        if (DateFrom != null)
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.Date >= DateFrom).AsQueryable();
                        }
                        if (DateTo != null)
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.Date <= DateTo).AsQueryable();
                        }
                        // Filter by searchKey in Title Subject - body - description
                        if (!string.IsNullOrWhiteSpace(filters.SearchKey))
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.Description.Contains(filters.SearchKey) || x.Title.Contains(filters.SearchKey)).AsQueryable();
                        }

                        if (filters.NotificationProcessID != 0)
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.NotificationProcessId == filters.NotificationProcessID).AsQueryable();
                        }
                        if (filters.NotificationID != 0)
                        {
                            UserNotificationDB = UserNotificationDB.Where(x => x.Id == filters.NotificationID).AsQueryable();
                        }
                        var NotificationPagingList = PagedList<VNotificationsDetail>.Create(UserNotificationDB.OrderByDescending(x => x.Date), filters.CurrentPage, filters.NumberOfItemsPerPage);

                        response.PaginationHeader = new PaginationHeader
                        {
                            CurrentPage = NotificationPagingList.CurrentPage,
                            TotalPages = NotificationPagingList.TotalPages,
                            ItemsPerPage = NotificationPagingList.PageSize,
                            TotalItems = NotificationPagingList.TotalCount
                        }; 
                        if (NotificationPagingList != null)
                        {
                            var NotificationList = new List<UserNotification>();
                            
                            foreach (var UserNotificationObjDB in NotificationPagingList)
                            {
                                var UserNotificationObjResponse = new UserNotification
                                {
                                    ID = UserNotificationObjDB.Id,
                                    ToUserID = UserNotificationObjDB.FromUserId, // ToUserID => FromUserID because always ToUSerId Must be not null but FromUserID can be nullable
                                    ToUserName = (UserNotificationObjDB.FromUserFirstName != null ? UserNotificationObjDB.FromUserFirstName +" " : "") + 
                                                 (UserNotificationObjDB.FromUserMiddleName != null ? UserNotificationObjDB.FromUserMiddleName + " " : "") + 
                                                 (UserNotificationObjDB.FromUserLastName != null ? UserNotificationObjDB.FromUserLastName : ""),
                                    Title = UserNotificationObjDB.Title,
                                    Description = UserNotificationObjDB.Description,
                                    Date = UserNotificationObjDB.Date.ToString(),
                                    URL = UserNotificationObjDB.Url != null ? UserNotificationObjDB.Url : "",
                                    New = UserNotificationObjDB.New,
                                    FromUserID = UserNotificationObjDB.ToUserId,
                                    FromUserName = (UserNotificationObjDB.ToUserFirstName != null ? UserNotificationObjDB.ToUserFirstName + " " : "") +
                                                 (UserNotificationObjDB.ToUserMiddleName != null ? UserNotificationObjDB.ToUserMiddleName + " " : "") +
                                                 (UserNotificationObjDB.ToUserFlastName != null ? UserNotificationObjDB.ToUserFlastName : ""),
                                    NotificationProcessID = UserNotificationObjDB.NotificationProcessId,
                                };
                                if(UserNotificationObjDB.Title.ToLower().Contains("vacation need approval") || UserNotificationObjDB.Description.ToLower().Contains("approval is sent"))
                                {
                                    var leaveRequest = _unitOfWork.LeaveRequests.FindAll(a =>a.Id == long.Parse(UserNotificationObjDB.Url), includes: new[] { "FirstApprovedByNavigation", "SecondApprovedByNavigation" }).FirstOrDefault();
                                    if (leaveRequest != null) 
                                    {


                                        UserNotificationObjResponse.Approval = (leaveRequest.FirstApproval == null && leaveRequest.SecondApproval == null) ? null : (leaveRequest.FirstApproval ?? false) || (leaveRequest.SecondApproval ?? false);
                                        if (leaveRequest.FirstApproval != null)
                                        {
                                            var FirstApprovedBy = leaveRequest.FirstApprovedByNavigation;
                                            UserNotificationObjResponse.ApprovedBy = FirstApprovedBy?.FirstName + " " + (FirstApprovedBy?.MiddleName != null ? FirstApprovedBy?.MiddleName + " " : null) + FirstApprovedBy?.LastName;
                                        }
                                        else if (leaveRequest.SecondApproval != null)
                                        {
                                            var SecondApprovedBy = leaveRequest.SecondApprovedByNavigation;
                                            UserNotificationObjResponse.ApprovedBy = SecondApprovedBy?.FirstName + " " + (SecondApprovedBy?.MiddleName != null ? SecondApprovedBy?.MiddleName + " " : null) + SecondApprovedBy?.LastName;
                                        }
                                        //request.FirstApproval;

                                        //UserNotificationObjResponse.ApprovedBy = request.FirstApprovedByNavigation?.FirstName+" "+(request.FirstApprovedByNavigation?.MiddleName!=null? request.FirstApprovedByNavigation?.MiddleName+" ":null)+ 
                                        //    request.FirstApprovedByNavigation?.LastName;
                                    }
                                }
                                else if(UserNotificationObjDB.Title.ToLower().Contains("task progress need approval"))
                                {
                                    var progress = _unitOfWork.WorkingHoursTrackings.FindAll(a =>a.Id == long.Parse(UserNotificationObjDB.Url), includes: new[] { "ApprovedByNavigation" }).FirstOrDefault();
                                    if (progress != null)
                                    {
                                        UserNotificationObjResponse.Approval = progress.WorkingHoursApproval;
                                        UserNotificationObjResponse.ApprovedBy = progress.ApprovedByNavigation?.FirstName + " " + (progress.ApprovedByNavigation?.MiddleName != null ? progress.ApprovedByNavigation?.MiddleName + " " : null) +
                                            progress.ApprovedByNavigation?.LastName;
                                    }
                                }
                                    var FromUserPhoto = NewGaras.Infrastructure.Common.GetUserPhoto((long)UserNotificationObjDB.ToUserId, _Context);
                                if (FromUserPhoto != null)
                                {
                                    UserNotificationObjResponse.FromUserPhoto = Globals.baseURL + "/" + FromUserPhoto;
                                }
                                if (UserNotificationObjDB.FromUserId != null)
                                {

                                    var ToUserPhoto = NewGaras.Infrastructure.Common.GetUserPhoto((long)UserNotificationObjDB.FromUserId, _Context);
                                    if (ToUserPhoto != null)
                                    {
                                        UserNotificationObjResponse.ToUserPhoto = Globals.baseURL + "/" + ToUserPhoto;
                                    }
                                }
                                NotificationList.Add(UserNotificationObjResponse);
                            }

                            response.UserNotificationsList = NotificationList;


                        }

                    }

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
