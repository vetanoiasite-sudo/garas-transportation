using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Helper;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;

using NewGaras.Infrastructure.DTO.User;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.Mail;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using NewGaras.Infrastructure.Models.User;
using NewGarasAPI.Helper;
using NewGarasAPI.Models.Common;
using NewGarasAPI.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Drawing;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = NewGarasAPI.Models.Common.Error;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewGarasAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private GarasTestContext _Context;
        private Helper.Helper _helper;
        private readonly string key;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ITenantService _tenantService;
        public UserController(IUserService userService, IMailService mailSettingsOptions, ITenantService tenantService)
        {
            _tenantService = tenantService;
            _Context = new GarasTestContext(_tenantService);
            key = "SalesGarasPass";
            _helper = new Helper.Helper();
            _userService = userService;
            _mailService = mailSettingsOptions;

        }



        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(UserLogin login)

        {

            //DateTime Trxdate =DateTime.Parse("2023-05-22 00:00:00.000");
            //DateTime date = DateTime.Now;
            //TimeSpan time = new TimeSpan(0, DateTime.Now.Hour, 0, 0);
            //var test = (Trxdate).Add(time).AddHours(-2).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

            /////////////// request/////////////////////
            //            {
            //                "Username":"michael_milad",
            //"Password":"P@ssw0rd",
            //"ApplicationID":"G7N7wJhBXdc+M1XQsI2wgQ=="
            //          }

            ////////////////response/////////////////////
            //            {
            //                "ErrorCode": [
            //                   "Err9"
            //   ],
            //   "ErrorMSG": [
            //      "please write your user name and password"
            //   ],
            //   "Result": false,
            //   "UserID": null,
            //   "UserImage": null,
            //   "UserName": null
            //}
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");


            LoginResponse response = new LoginResponse();
            response.Result = true;
            response.Errors = new List<Models.Common.Error>();

            try
            {
                //check sent data
                if (login == null)
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P6";
                    error.ErrorMSG = "please write your Email and password";
                    response.Errors.Add(error);
                    return response;
                }
                if (string.IsNullOrEmpty(login.Email) || login.Email.Trim() == "")
                {

                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your Email";
                    response.Errors.Add(error);
                }
                if (login.ExternalLoginFrom?.ToLower() != "office365")  // Currently only temporarily
                {
                    if (string.IsNullOrEmpty(login.Password) || login.Password.Trim() == "")
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P8";
                        error.ErrorMSG = "please write your password";
                        response.Errors.Add(error);
                    }
                }
                #region  For license validation 
                if(login.CompanyName?.ToLower() =="pharma")  
                {
                    var authorizedmotherboard = "E9AD6AEF024D99188E10765EEBAB6E96FA2B9A7FC0CA55651375FFAC6755FFD1";
                    var authorizedUuid = "0EBC5C05A714034DE42FD2EB44F7E8CBC03B17550B5E33AE4AC3EB57C3E04503";
                    DateTime encodedExpiry = new DateTime(2027, 01, 01);

                    // 2. حساب هاش الجهاز الحالي (مرة واحدة فقط عند أول طلب لسرعة الأداء)

                    var deviceIdentifier = DeviceIdentifier.GetUniqueId();


                    // 3. التحقق من تطابق الجهاز
                    if(deviceIdentifier.motherboardId!=authorizedmotherboard||deviceIdentifier.uuid!=authorizedUuid)
                    {
                        response.Result=false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode="Err-P8";
                        error.ErrorMSG="Unauthorized Hardware ID.";
                        response.Errors.Add(error);
                    }


                    if(DateTime.Now>encodedExpiry)
                    {
                        response.Result=false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode="Err-P8";
                        error.ErrorMSG="License has expired.";
                        response.Errors.Add(error);
                    }

                   
                }
                #endregion
                #region For Shared Server
                ValidateCompanyName validator = new ValidateCompanyName();

                if (string.IsNullOrEmpty(login.CompanyName))
                {

                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your Company";
                    response.Errors.Add(error);
                    //if (TXT_CompanyName.Text.ToLower() == "marinaplt" || TXT_CompanyName.Text.ToLower() == "proauto" || TXT_CompanyName.Text.ToLower() == "piaroma")
                }
                else if (!validator.ValidateInput(login.CompanyName.ToString().ToLower()))
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "Invalid Company Name";
                    response.Errors.Add(error);
                }
                #endregion
                if (response.Result)
                {

                    #region For Shared Server

                    // Check Entity DB 
                    string CompName = login.CompanyName.ToLower();
                    //UpdateContext(CompName);

                    _Context.Database.SetConnectionString(_helper.GetConnectonString(CompName));
                    #endregion

                    //var CheckUserDB = _Context.proc_UserLoadAll().Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim()).FirstOrDefault();

                    var CheckUserDB = new NewGaras.Infrastructure.Entities.User();

                    Expression<Func<NewGaras.Infrastructure.Entities.User, bool>> Criteria = (x => true);


                    if (login.ExternalLoginFrom?.ToLower() != "office365")
                    {
                        string PassEncrypted = Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim();
                        Criteria = (x => x.Email.Trim() == login.Email.Trim() && x.Password.ToLower().Trim() == PassEncrypted);
                    }
                    else
                    {
                        Criteria = (x => x.Email.Trim() == login.Email.Trim());
                    }

                    CheckUserDB = await _Context.Users.Where(Criteria).Include(x => x.JobTitle).Include(x => x.Department).Include(x => x.Branch).FirstOrDefaultAsync();
                   
                    if (CheckUserDB == null && login.ExternalLoginFrom?.ToLower() != "office365")
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P11";
                        error.ErrorMSG = "Invalid Email or Password";
                        response.Errors.Add(error);
                        return response;
                    }
                    if (CheckUserDB != null)
                    {
                        if (CheckUserDB.Active)
                        {
                            //////////////////////

                            if (CheckUserDB.PhotoUrl != null)
                            {
                                // user.UserImage = DBUser.Photo;
                                //var baseURL = OperationContext.Current.Host.BaseAddresses[0].Authority;
                                response.UserImageURL = baseURL + CheckUserDB.PhotoUrl; // Common.GetUserPhoto(CheckUserDB.Id, _Context);
                                /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key)) + "&type=photo&CompName=" + CompName;*/
                            }
                            response.EmplyeeId = _Context.HrUsers.Where(x => x.UserId == CheckUserDB.Id).Select(x => x.Id).FirstOrDefault();
                            long UserSessionID = 0;
                            //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
                            //if (CheckSessionOpen == null)
                            //{
                            //    var UserSessionObj = new UserSession(); // DB
                            //    UserSessionObj.UserID = CheckUserDB.ID;
                            //    UserSessionObj.Active = true;
                            //    UserSessionObj.CreationDate = DateTime.Now;
                            //    UserSessionObj.EndDate = DateTime.Now.AddDays(1);
                            //    UserSessionObj.ModifiedBy = "System";
                            //    _Context.UserSessions.Add(UserSessionObj);

                            //      await _Context.SaveChangesAsync();
                            //    UserSessionID = (long)UserSessionObj.ID;
                            //}
                            //else
                            //{
                            //    UserSessionID = CheckSessionOpen.ID;
                            //}


                            //var UserSessionInsertion = _Context.proc_UserSessionInsert(IDUserSession,
                            //                                                              CheckUserDB.Id,
                            //                                                              true,
                            //                                                              DateTime.Now,
                            //                                                              DateTime.Now.AddDays(1),
                            //                                                              "System");

                            //SqlParameter test = new SqlParameter
                            //{
                            //    Direction = System.Data.ParameterDirection.Output,
                            //    Value = 1,

                            //};

                            SqlParameter IDUserSession = new SqlParameter("ID", System.Data.SqlDbType.BigInt);
                            IDUserSession.Direction = System.Data.ParameterDirection.Output;
                            var userIDParameter = new SqlParameter("UserID", System.Data.SqlDbType.BigInt);
                            userIDParameter.Value = CheckUserDB.Id;
                            var activeParameter = new SqlParameter("Active", System.Data.SqlDbType.Bit);
                            activeParameter.Value = 1;
                            var creationDateParameter = new SqlParameter("CreationDate", System.Data.SqlDbType.DateTime);
                            creationDateParameter.Value = DateTime.Now;
                            var endDateParameter = new SqlParameter("EndDate", System.Data.SqlDbType.DateTime);
                            endDateParameter.Value = DateTime.Now.AddDays(1);
                            var modifiedByParameter = new SqlParameter("ModifiedBy", System.Data.SqlDbType.NVarChar);
                            modifiedByParameter.Value = "System";

                            object[] param = new object[] { IDUserSession, userIDParameter , activeParameter , creationDateParameter
                             ,endDateParameter,modifiedByParameter};
                            //List<SqlParameter> param = new List<SqlParameter>();
                            //param.Add(IDUserSession);
                            //param.Add(userIDParameter); 
                            //param.Add(activeParameter);
                            //param.Add(creationDateParameter);
                            //param.Add(endDateParameter);
                            //param.Add(modifiedByParameter);

                            var UserSessionInsertion = _Context.Database.SqlQueryRaw<int>("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param).AsEnumerable().FirstOrDefault();

                            //var UserSessionInsertion = _Context.Database.ExecuteSqlRaw
                            //   ("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param);


                            //var UserSessionInsertion = _Context.Database.ExecuteSqlRaw
                            //    ("Exec proc_UserSessionInsert @ID , @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", IDUserSession,
                            //    userIDParameter,activeParameter, creationDateParameter, endDateParameter, modifiedByParameter);


                            UserSessionID = (long)IDUserSession.Value;
                            if (UserSessionID > 0)
                            {
                                /* Mark Shawky */
                                /* 2023-1-4 */
                                /* Create Daily Report If Not Exist For SalesMan User */
                                List<string> groups = new List<string>();
                                groups.Add("SalesMen");

                                var IsUserInGrp = await _Context.VGroupUsers.Where(a => groups.Contains(a.Name) && a.UserId == CheckUserDB.Id && a.Active == true && a.UserActive == true).ToListAsync();
                                if (IsUserInGrp != null && IsUserInGrp.Count > 0)
                                {

                                    List<string> WeekendList = new List<string>();
                                    var Weekends = await _Context.WeekEnds.ToListAsync();
                                    if (Weekends != null && Weekends.Count > 0)
                                    {
                                        foreach (var weekend in Weekends)
                                        {
                                            WeekendList.Add(weekend.Day);
                                        }
                                    }

                                    bool hasReport = false;
                                    DateTime lastReportDate = DateTime.Now.AddDays(-1).Date;
                                    DateTime MaxReportDate = DateTime.Now.AddDays(1).Date;
                                    var OldReport = await _Context.DailyReports.Where(a => a.UserId == CheckUserDB.Id && a.ReprotDate < MaxReportDate).OrderByDescending(a => a.ReprotDate).FirstOrDefaultAsync();
                                    if (OldReport != null)
                                    {
                                        hasReport = true;
                                        lastReportDate = OldReport.ReprotDate.Date;
                                    }

                                    bool createreport = false;
                                    if (hasReport)
                                    {
                                        if (DateTime.Compare(lastReportDate.Date, DateTime.Now.Date) != 0)
                                        {
                                            createreport = true;
                                        }
                                    }
                                    else
                                    {
                                        createreport = true;
                                    }

                                    if (createreport)
                                    {
                                        while (DateTime.Compare(lastReportDate.Date, DateTime.Now.Date) != 0)
                                        {
                                            lastReportDate = lastReportDate.AddDays(1);
                                            if (!WeekendList.Contains(lastReportDate.DayOfWeek.ToString()))
                                            {
                                                DailyReport newReport = new DailyReport();
                                                newReport.CreationDate = DateTime.Now;
                                                newReport.ModifiedBy = 1;
                                                newReport.ModifiedDate = DateTime.Now;
                                                newReport.ReprotDate = lastReportDate;
                                                newReport.Reviewed = false;
                                                newReport.Status = "Not Filled";
                                                newReport.UserId = CheckUserDB.Id;
                                                _Context.DailyReports.Add(newReport);
                                                await _Context.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                ///* End */
                                var MainCompanyProfile = await _Context.Clients.Where(a => a.OwnerCoProfile == true).FirstOrDefaultAsync();
                                var MainCompanyProfileAddress = MainCompanyProfile?.ClientAddresses?.FirstOrDefault();
                                if (MainCompanyProfileAddress != null)
                                {
                                    response.CountryId = MainCompanyProfileAddress.CountryId;
                                    response.CountryName = MainCompanyProfileAddress.Country?.Name;
                                }
                                response.Result = true;
                                response.UserIDNO = CheckUserDB.Id;
                                response.Data = HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(UserSessionID.ToString(), key));
                                response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key);
                                response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                                response.Jobtitle = CheckUserDB?.JobTitle?.Name; // CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                                response.DepartmentName = CheckUserDB.Department?.Name; // != null ? CheckUserDB.UserDepartmentName : "";
                                response.BranchName = CheckUserDB.Branch?.Name; // CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                                response.BranchID = CheckUserDB.BranchId;

                                //response.Jobtitle = CheckUserDB.JobTitleID != null ? Common.GetJobTitleName((int)CheckUserDB.JobTitleID) : "";
                                //response.DepartmentName = CheckUserDB.DepartmentID != null ? Common.GetDepartmentName((int)CheckUserDB.DepartmentID) : "";
                                //response.BranchName = CheckUserDB.BranchID != null ? Common.GetBranchName((int)CheckUserDB.BranchID) : "";

                                // Fill User role List 
                                var RoleList = new List<Roles>();
                                var RoleListDB = await _Context.VUserRoles.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
                                foreach (var UserRoleOBJ in RoleListDB)
                                {
                                    RoleList.Add(new Roles
                                    {
                                        RoleID = UserRoleOBJ.RoleId,
                                        RoleName = UserRoleOBJ.RoleName
                                    });
                                }

                                // Fill User Group List 
                                var GroupList = new List<GroupRoles>();
                                var GroupListDB = await _Context.VGroupUserBranches.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
                                foreach (var UserGroupOBJ in GroupListDB)
                                {
                                    GroupList.Add(new GroupRoles
                                    {
                                        GroupID = UserGroupOBJ.GroupId,
                                        GroupName = UserGroupOBJ.GroupName
                                    });
                                }

                                var SpecialityList = new List<SelectDDL>();
                                SpecialityList = await _Context.CompanySpecialties.Select(x => new SelectDDL
                                {
                                    ID = x.SpecialityId,
                                    Name = x.SpecialityName
                                }).ToListAsync();
                                //var GroupList = new List<GroupRoles>();
                                //var GroupListDB = _Context.proc_Group_UserLoadAll().Where(x => x.UserID == CheckUserDB.ID).ToList();
                                //foreach (var UserGroupOBJ in GroupListDB)
                                //{
                                //        GroupList.Add(new GroupRoles
                                //        {
                                //            GroupID = (long)UserGroupOBJ.GroupID,
                                //            GroupName = Common.GetGroupName(UserGroupOBJ.GroupID)
                                //        });
                                //}
                                //
                                var NotificationCount = _Context.Notifications.Where(x => (x.FromUserId == CheckUserDB.Id || x.FromUserId == null) && x.New == true).Count();
                                response.NotificationCount = NotificationCount;


                                var TaskCountFromTaskCount = _Context.Tasks.Where(x => x.TaskDetails.Where(y => y.Status == "Open").Any() &&
                                (x.CreatedBy == CheckUserDB.Id || x.TaskPermissions.Where(p => p.UserGroupId == CheckUserDB.Id).Any()
                                )).Count();

                                response.TaskCount = TaskCountFromTaskCount;


                                response.SpecialityList = SpecialityList;
                                response.RoleList = RoleList;
                                response.GroupList = GroupList;





                                //  for Working Hour Teacking and Task
                                var WorkingHourTrackingForHrUserList = _Context.WorkingHourseTrackings
                                    .Include(a => a.Task)
                                    .Where(a => a.HrUserId == response.EmplyeeId)
                                    .OrderByDescending(a => a.Id).ToList();



                                var LocalCurrency = await _Context.Currencies.Where(x => x.IsLocal == true).FirstOrDefaultAsync();
                                if (LocalCurrency != null)
                                {
                                    response.LocalCurrencyId = LocalCurrency.Id;
                                    response.LocalCurrencyName = LocalCurrency.Name;
                                }

                                if (response.EmplyeeId != null)
                                {

                                    var ContractDetails = _Context.ContractDetails.Where(x => x.HrUserId == response.EmplyeeId && x.IsCurrent == true).FirstOrDefault();
                                    if (ContractDetails != null)
                                    {
                                        response.AllowLocationTracking = ContractDetails.AllowLocationTracking ?? false;
                                    }
                                }
                                // from supplier Is Owner
                                var ClientInfo = _Context.Clients.Where(x => x.OwnerCoProfile == true).FirstOrDefault();
                                if (ClientInfo != null)
                                {
                                    if (ClientInfo.LogoUrl != null)
                                    {
                                        response.ClientId = ClientInfo.Id;
                                        response.CompanyImg = baseURL + "/" + ClientInfo.LogoUrl; //Common.GetUserPhoto(ClientInfo.Id, _Context);
                                        /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientInfo.Id.ToString(), key)) + "&type=client&CompName=" + login.CompanyName.ToString().ToLower();*/
                                    }
                                    response.CompanyInfo = ClientInfo.Name;
                                    //response.CompanyImg = baseURL + ClientInfo.Logo;
                                }
                                return response;
                            }

                        }
                        else
                        {
                            response.Result = false;
                            Models.Common.Error error = new Models.Common.Error();
                            error.ErrorCode = "Err-P9";
                            error.ErrorMSG = "This Email is not active ";
                            response.Errors.Add(error);

                        }

                    }
                    else
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P11";
                        error.ErrorMSG = "Invalid Email or password";
                        response.Errors.Add(error);

                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }

        [HttpPost("LoginForWeb")]
        public async Task<ActionResult<LoginResponse>> LoginForWeb(UserLogin login)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            LoginResponse response = new LoginResponse();
            response.Result = true;
            response.Errors = new List<Models.Common.Error>();

            try
            {
                //check sent data
                if (login == null)
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P6";
                    error.ErrorMSG = "please write your Email and password";
                    response.Errors.Add(error);
                    return response;
                }
                if (string.IsNullOrEmpty(login.Email) || login.Email.Trim() == "")
                {

                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your Email";
                    response.Errors.Add(error);
                }
                if (string.IsNullOrEmpty(login.Password) || login.Password.Trim() == "")
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P8";
                    error.ErrorMSG = "please write your password";
                    response.Errors.Add(error);
                }
                #region For Shared Server
                ValidateCompanyName validator = new ValidateCompanyName();
                if (string.IsNullOrEmpty(login.CompanyName))
                {

                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your Company";
                    response.Errors.Add(error);
                    //if (TXT_CompanyName.Text.ToLower() == "marinaplt" || TXT_CompanyName.Text.ToLower() == "proauto" || TXT_CompanyName.Text.ToLower() == "piaroma")
                }
                else if (!validator.ValidateInput(login.CompanyName.ToString().ToLower()))
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "Invalid Company Name";
                    response.Errors.Add(error);
                }
                #endregion
                if (response.Result)
                {

                    #region For Shared Server

                    // Check Entity DB 
                    string CompName = login.CompanyName.ToLower();
                    //if (CompName == "proauto")
                    //{
                    //    _Context = GlobalEntity.ContextGarasAuto;
                    //}
                    //else if (CompName == "marinaplt")
                    //{
                    //    _Context = GlobalEntity.ContextGarasMarina;
                    //}
                    //else if (CompName == "piaroma")
                    //{
                    //    _Context = GlobalEntity.ContextGarasAroma;
                    //}
                    //else if (CompName == "elsalam")
                    //{
                    //    _Context = GlobalEntity.ContextGarasElSalam;
                    //}
                    //else if (CompName == "elwaseem")
                    //{
                    //    _Context = GlobalEntity.ContextGarasElWaseem;
                    //}
                    //else if (CompName == "garastest")
                    //{
                    //    _Context = GlobalEntity.ContextGarasTest;
                    //}
                    //else if (CompName == "royaltent")
                    //{
                    //    _Context = GlobalEntity.ContextGarasRoyal;
                    //}
                    //else if (CompName == "marinapltq")
                    //{
                    //    _Context = GlobalEntity.ContextGarasMarinaQATAR;
                    //}
                    //else if (CompName == "vetanoia")
                    //{
                    //    _Context = GlobalEntity.ContextVETANOIA;
                    //}
                    //else if (CompName == "ramsissteel")
                    //{
                    //    _Context = GlobalEntity.ContextGARASRamsisSteel;
                    //}
                    _Context.Database.SetConnectionString(_helper.GetConnectonString(CompName));
                    #endregion

                    //var CheckUserDB = _Context.proc_UserLoadAll().Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim()).FirstOrDefault();
                    string PassEncrypted = Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim();
                    var CheckUserDB = await _Context.VUserInfos.Where(x => x.Email.Trim() == login.Email.Trim() && x.Password.ToLower().Trim() == PassEncrypted).FirstOrDefaultAsync();

                    if (CheckUserDB != null)
                    {
                        if (CheckUserDB.Active)
                        {
                            //////////////////////

                            //if (CheckUserDB.Photo != null)
                            //{
                            //    // user.UserImage = DBUser.Photo;
                            //    //var baseURL = OperationContext.Current.Host.BaseAddresses[0].Authority;

                            //    response.UserImageURL = baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key)) + "&type=photo&CompName=" + CompName;
                            //}
                            long UserSessionID = 0;
                            //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
                            //if (CheckSessionOpen == null)
                            //{
                            //    var UserSessionObj = new UserSession(); // DB
                            //    UserSessionObj.UserID = CheckUserDB.ID;
                            //    UserSessionObj.Active = true;
                            //    UserSessionObj.CreationDate = DateTime.Now;
                            //    UserSessionObj.EndDate = DateTime.Now.AddDays(1);
                            //    UserSessionObj.ModifiedBy = "System";
                            //    _Context.UserSessions.Add(UserSessionObj);

                            //    await _Context.SaveChangesAsync();
                            //    UserSessionID = (long)UserSessionObj.ID;
                            //}
                            //else
                            //{
                            //    UserSessionID = CheckSessionOpen.ID;
                            //}
                            //ObjectParameter IDUserSession = new ObjectParameter("ID", typeof(long));
                            //var UserSessionInsertion = _Context.proc_UserSessionInsert(IDUserSession,
                            //                                                              CheckUserDB.ID,
                            //                                                              true,
                            //                                                              DateTime.Now,
                            //                                                              DateTime.Now.AddDays(1),
                            //                                                              "System");
                            SqlParameter IDUserSession = new SqlParameter("ID", System.Data.SqlDbType.BigInt);
                            IDUserSession.Direction = System.Data.ParameterDirection.Output;
                            var userIDParameter = new SqlParameter("UserID", System.Data.SqlDbType.BigInt);
                            userIDParameter.Value = CheckUserDB.Id;
                            var activeParameter = new SqlParameter("Active", System.Data.SqlDbType.Bit);
                            activeParameter.Value = 1;
                            var creationDateParameter = new SqlParameter("CreationDate", System.Data.SqlDbType.DateTime);
                            creationDateParameter.Value = DateTime.Now;
                            var endDateParameter = new SqlParameter("EndDate", System.Data.SqlDbType.DateTime);
                            endDateParameter.Value = DateTime.Now.AddDays(1);
                            var modifiedByParameter = new SqlParameter("ModifiedBy", System.Data.SqlDbType.NVarChar);
                            modifiedByParameter.Value = "System";

                            object[] param = new object[] { IDUserSession, userIDParameter , activeParameter , creationDateParameter
                             ,endDateParameter,modifiedByParameter};
                            //List<SqlParameter> param = new List<SqlParameter>();
                            //param.Add(IDUserSession);
                            //param.Add(userIDParameter);
                            //param.Add(activeParameter);
                            //param.Add(creationDateParameter);
                            //param.Add(endDateParameter);
                            //param.Add(modifiedByParameter);

                            var UserSessionInsertion = _Context.Database.SqlQueryRaw<int>("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param).AsEnumerable().FirstOrDefault();

                            //var UserSessionInsertion = _Context.Database.ExecuteSqlRaw
                            //    ("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param);


                            UserSessionID = (long)IDUserSession.Value;
                            if (UserSessionID > 0)
                            {

                                response.Result = true;
                                response.Data = HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(UserSessionID.ToString(), key));
                                response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key);
                                response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                                response.Jobtitle = CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                                response.DepartmentName = CheckUserDB.UserDepartmentName != null ? CheckUserDB.UserDepartmentName : "";
                                response.BranchName = CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                                return response;
                            }

                        }
                        else
                        {
                            response.Result = false;
                            Models.Common.Error error = new Models.Common.Error();
                            error.ErrorCode = "Err-P9";
                            error.ErrorMSG = "This Email is not active ";
                            response.Errors.Add(error);
                        }

                    }
                    else
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P11";
                        error.ErrorMSG = "Invalid Email or password";
                        response.Errors.Add(error);

                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }

        [HttpPost("LoginForClient")]
        public async Task<ActionResult<LoginForClientResponse>> LoginForClient(ClientLoginRequest login)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            LoginForClientResponse response = new LoginForClientResponse();
            response.Result = true;
            response.Errors = new List<Models.Common.Error>();

            try
            {
                if (login == null)
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P6";
                    error.ErrorMSG = "please write your User Name and password";
                    response.Errors.Add(error);
                    return response;
                }
                if (string.IsNullOrEmpty(login.UserName) || login.UserName.Trim() == "")
                {

                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your User Name";
                    response.Errors.Add(error);
                }
                if (string.IsNullOrEmpty(login.Password) || login.Password.Trim() == "")
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P8";
                    error.ErrorMSG = "please write your password";
                    response.Errors.Add(error);
                }
                #region For Shared Server
                ValidateCompanyName validator = new ValidateCompanyName();
                if (string.IsNullOrEmpty(login.CompanyName))
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "please write your Company";
                    response.Errors.Add(error);
                    //if (TXT_CompanyName.Text.ToLower() == "marinaplt" || TXT_CompanyName.Text.ToLower() == "proauto" || TXT_CompanyName.Text.ToLower() == "piaroma")
                }
                else if (!validator.ValidateInput(login.CompanyName.ToString().ToLower()))
                {
                    response.Result = false;
                    Models.Common.Error error = new Models.Common.Error();
                    error.ErrorCode = "Err-P7";
                    error.ErrorMSG = "Invalid Company Name";
                    response.Errors.Add(error);
                }
                #endregion
                if (response.Result)
                {
                    #region For Shared Server
                    // Check Entity DB 
                    string CompName = login.CompanyName.ToLower();
                    // UpdateContext(CompName);
                    _Context.Database.SetConnectionString(_helper.GetConnectonString(CompName));
                    #endregion

                    long ClientID = 0;
                    string ClientName = "";
                    string ContactPersonName = "";
                    string ClientMobile = "";
                    string ContactPersonMobile = "";
                    bool ClientHasLogo = false;
                    long MaintenanceID = 0;
                    var CheckClientEmailDB = await _Context.Clients.Where(x => x.Email.Trim().ToLower() == login.UserName.Trim().ToLower()).FirstOrDefaultAsync();

                    if (CheckClientEmailDB != null)
                    {
                        ClientID = CheckClientEmailDB.Id;
                        ClientName = CheckClientEmailDB.Name;
                        ClientMobile = CheckClientEmailDB.ClientMobiles.Select(x => x.Mobile).FirstOrDefault();
                        ClientHasLogo = CheckClientEmailDB.HasLogo != null ? (bool)CheckClientEmailDB.HasLogo : false;
                    }
                    else
                    {
                        string last7Digits = login.UserName.Substring(login.UserName.Length - 7, 7);
                        var CheckClientMobileDB = await _Context.ClientMobiles.Where(x => x.Mobile.Trim().ToLower().Substring((x.Mobile.Length - 7), 7) == last7Digits.Trim().ToLower()).Include(a=>a.Client).FirstOrDefaultAsync();
                        var CheckClientContactMobileDB = await _Context.ClientContactPeople.Where(x => x.Mobile.Trim().ToLower().Substring(x.Mobile.Length - 7, 7) == last7Digits.Trim().ToLower()).Include(a=>a.Client).FirstOrDefaultAsync();
                        if (CheckClientMobileDB != null || CheckClientContactMobileDB != null)
                        {
                            ClientID = CheckClientMobileDB?.ClientId ?? CheckClientContactMobileDB?.ClientId ?? 0;
                            if (CheckClientMobileDB?.Client != null || CheckClientContactMobileDB?.Client != null)
                            {
                                ClientName = CheckClientMobileDB?.Client?.Name ?? CheckClientContactMobileDB?.Client?.Name;
                                ContactPersonName = CheckClientMobileDB?.Client?.ClientContactPeople?.FirstOrDefault()?.Name ?? CheckClientContactMobileDB?.Name;
                                ClientMobile = CheckClientMobileDB?.Mobile ?? CheckClientContactMobileDB?.Mobile;
                                ContactPersonMobile = CheckClientMobileDB?.Client?.ClientContactPeople?.FirstOrDefault()?.Mobile ?? CheckClientContactMobileDB?.Mobile;
                                ClientHasLogo = CheckClientMobileDB?.Client?.HasLogo != null ? (bool)CheckClientMobileDB?.Client?.HasLogo :
                                    CheckClientContactMobileDB?.Client?.HasLogo != null ? (bool)CheckClientContactMobileDB?.Client?.HasLogo : false;
                            }
                        }
                    }
                    if (ClientID != 0)
                    {
                        // Check Is Maintenance Serial exist as password for the same supplier
                        var MantinanceForObj = await _Context.MaintenanceFors.Where(x => x.ClientId == ClientID &&
                                                                                      x.ProductSerial.Trim() == login.Password.Trim()).FirstOrDefaultAsync();
                        if (MantinanceForObj != null)
                        {
                            MaintenanceID = MantinanceForObj.Id;

                            var ClientSessionObj = new ClientSession();
                            ClientSessionObj.ClientId = ClientID;
                            ClientSessionObj.Active = true;
                            ClientSessionObj.CreationDate = DateTime.Now;
                            ClientSessionObj.EndDate = DateTime.Now.AddDays(1);
                            ClientSessionObj.ModifiedBy = "System";

                            _Context.ClientSessions.Add(ClientSessionObj);
                            var Res = await _Context.SaveChangesAsync();
                            if (Res > 0)
                            {
                                long ClientSessionID = ClientSessionObj.Id;
                                response.Result = true;
                                response.Data = HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientSessionID.ToString(), key));
                                response.ClientID = ClientID;
                                response.ClientIDEnc = Encrypt_Decrypt.Encrypt(ClientID.ToString(), key);
                                response.ClientName = ClientName;
                                response.ContactPersonName = ContactPersonName;
                                response.ClientMobile = ClientMobile;
                                response.ContactPersonMobile = ContactPersonMobile;
                                response.MaintenanceID = MaintenanceID;
                                if (ClientHasLogo && CheckClientEmailDB != null)
                                {
                                    if (CheckClientEmailDB.Logo != null)
                                    {

                                        response.ClientImageURL = baseURL + CheckClientEmailDB.Logo; // "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientID.ToString(), key)) + "&type=client&CompName=" + CompName;
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.Result = false;
                            Models.Common.Error error = new Models.Common.Error();
                            error.ErrorCode = "Err-P11";
                            error.ErrorMSG = "Invalid UserName or password";
                            response.Errors.Add(error);
                        }
                    }
                    else
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P11";
                        error.ErrorMSG = "Invalid UserName or password";
                        response.Errors.Add(error);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }

        [HttpPost("Logout")]
        public BaseResponseWithId Logout(LogoutRequest logoutRequest)
        {
            /////////////// request/////////////////////
            //            {
            //                "SessionID":"G7N7wJhBXdc+M1XQsI2wgQ==",
            //          }



            BaseResponseWithId response = new BaseResponseWithId();
            response.Result = true;
            response.Errors = new List<Models.Common.Error>();

            try
            {

                //IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                // HeaderDictionary headers =(HeaderDictionary) HttpContext.Request.Headers;
                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;

                if (response.Result)
                {
                    //check sent data
                    if (logoutRequest == null)
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P12";
                        error.ErrorMSG = "please insert a valid data.";
                        response.Errors.Add(error);
                        return response;
                    }
                    if (string.IsNullOrEmpty(logoutRequest.SessionID) || logoutRequest.SessionID.Trim() == "")
                    {

                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P13";
                        error.ErrorMSG = "please write your session ID";
                        response.Errors.Add(error);
                    }

                    string UserToken = HttpUtility.UrlDecode(logoutRequest.SessionID);
                    long UserSessionID = 0;
                    if (string.IsNullOrEmpty(UserToken) || !long.TryParse(Encrypt_Decrypt.Decrypt(UserToken, key), out UserSessionID))
                    {
                        response.Result = false;
                        Models.Common.Error error = new Models.Common.Error();
                        error.ErrorCode = "Err-P14";
                        error.ErrorMSG = "Invalid Session ID.";
                        response.Errors.Add(error);
                    }
                    //var dsd = HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt("110", key));
                    if (response.Result)
                    {
                        // var CheckUserSession = _Context.proc_UserSessionLoadByPrimaryKey(UserSessionID).FirstOrDefault();
                        SqlParameter ID = new SqlParameter("ID", System.Data.SqlDbType.BigInt);

                        ID.Value = UserSessionID;
                        object[] param = new object[] { ID };
                        //List<SqlParameter> param = new List<SqlParameter>();
                        //param.Add(ID);


                        var CheckUserSession = _Context.Database.SqlQueryRaw<proc_UserSessionLoadByPrimaryKey_Result>("Exec proc_UserSessionLoadByPrimaryKey @ID ", param).AsEnumerable().FirstOrDefault();

                        //var CheckUserSession = _Context.Database.ExecuteSqlRaw
                        //   ("Exec proc_UserSessionLoadByPrimaryKey @ID ", param);

                        if (CheckUserSession != null)
                        {
                            if (CheckUserSession.Active == true)
                            {
                                var DiID = new SqlParameter("ID", System.Data.SqlDbType.BigInt);
                                DiID.Value = UserSessionID;
                                var activeParameter = new SqlParameter("Active", System.Data.SqlDbType.Bit);
                                activeParameter.Value = 0;

                                var modifiedByParameter = new SqlParameter("ModifiedBy", System.Data.SqlDbType.NVarChar);
                                modifiedByParameter.Value = "System";
                                param = new object[] { DiID, activeParameter, modifiedByParameter };


                                // _Context.proc_UserSessionUpdate_DiActivate(UserSessionID, false, "System");
                                _Context.Database.SqlQueryRaw<int>("EXEC proc_UserSessionUpdate_DiActivate @ID , @Active , @ModifiedBy", param).AsEnumerable().FirstOrDefault();
                                response.Result = true;
                                return response;
                            }
                            else
                            {
                                response.Result = false;
                                Models.Common.Error error = new Models.Common.Error();
                                error.ErrorCode = "Err-P15";
                                error.ErrorMSG = "User is already logouted.";
                                response.Errors.Add(error);
                            }
                        }
                        else
                        {
                            response.Result = false;
                            Models.Common.Error error = new Models.Common.Error();
                            error.ErrorCode = "Err-P14";
                            error.ErrorMSG = "Invalid Session ID.";
                            response.Errors.Add(error);
                        }
                    }
                }
                return response;


            }
            catch (Exception ex)
            {
                response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }

        [HttpGet("GetUserData")]    //service added
        public async Task<ActionResult<LoginResponse>> GetUserData([FromHeader]string UserToken)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            LoginResponse response = new LoginResponse();
            response.Result = true;
            response.Errors = new List<Models.Common.Error>();

            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                response.Errors = validation.errors;
                response.Result = validation.result;

                #region old code without service
                //if (response.Result)
                //{
                //    //var CheckUserDB = _Context.proc_UserLoadAll().Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim()).FirstOrDefault();
                //    var CheckUserDB = await _Context.Users.Where(x => x.Id == validation.userID).Include(x => x.JobTitle).Include(x => x.Department).Include(x => x.Branch).FirstOrDefaultAsync();


                //    //var CheckUserDB = await _Context.V_UserInfo.Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == PassEncrypted).FirstOrDefaultAsync();

                //    if (CheckUserDB != null)
                //    {
                //        if (CheckUserDB.Active)
                //        {
                //            //////////////////////

                //            if (CheckUserDB.PhotoUrl != null)
                //            {
                //                // user.UserImage = DBUser.Photo;
                //                //var baseURL = OperationContext.Current.Host.BaseAddresses[0].Authority;

                //                response.UserImageURL = baseURL + CheckUserDB.PhotoUrl;
                //                //baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key)) + "&type=photo&CompName=" + Request.Headers["CompanyName"].ToString().ToLower();
                //            }
                //            //long UserSessionID = 0;
                //            //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
                //            //if (CheckSessionOpen == null)
                //            //{
                //            //    var UserSessionObj = new UserSession(); // DB
                //            //    UserSessionObj.UserID = CheckUserDB.ID;
                //            //    UserSessionObj.Active = true;
                //            //    UserSessionObj.CreationDate = DateTime.Now;
                //            //    UserSessionObj.EndDate = DateTime.Now.AddDays(1);
                //            //    UserSessionObj.ModifiedBy = "System";
                //            //    _Context.UserSessions.Add(UserSessionObj);

                //            //      await _Context.SaveChangesAsync();
                //            //    UserSessionID = (long)UserSessionObj.ID;
                //            //}
                //            //else
                //            //{
                //            //    UserSessionID = CheckSessionOpen.ID;
                //            //}
                //            //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.CreationDate).FirstOrDefaultAsync();
                //            //if (CheckSessionOpen != null)
                //            //{

                //            response.EmplyeeId = _Context.HrUsers.Where(x => x.UserId == CheckUserDB.Id).Select(x => x.Id).FirstOrDefault();
                //            response.Result = true;
                //            response.Data = Request.Headers["UserToken"]; // HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckSessionOpen.ID.ToString(), key));
                //            response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key);
                //            response.UserIDNO = CheckUserDB.Id;
                //            response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                //            response.Jobtitle = CheckUserDB?.JobTitle?.Name; // CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                //            response.DepartmentName = CheckUserDB.Department?.Name; // != null ? CheckUserDB.UserDepartmentName : "";
                //            response.BranchName = CheckUserDB.Branch?.Name; // CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                //            response.BranchID = CheckUserDB.BranchId;
                //            var MainCompanyProfile = await _Context.Clients.Where(a => a.OwnerCoProfile == true).FirstOrDefaultAsync();
                //            var MainCompanyProfileAddress = MainCompanyProfile?.ClientAddresses?.FirstOrDefault();
                //            if (MainCompanyProfileAddress != null)
                //            {
                //                response.CountryId = MainCompanyProfileAddress.CountryId;
                //                response.CountryName = MainCompanyProfileAddress.Country?.Name;
                //            }

                //            //  for Working Hour Teacking and Task
                //            var WorkingHourTrackingForHrUserList = _Context.WorkingHourseTrackings
                //                .Include(a => a.Task)
                //                .Where(a => a.HrUserId == response.EmplyeeId  )
                //                .OrderByDescending(a => a.Id).ToList();

                //            if (WorkingHourTrackingForHrUserList.Count() > 0)
                //            {

                //                //  HR Check In Task Working Hours
                //                var TaskWorkingHours = WorkingHourTrackingForHrUserList
                //                    .Where(a =>  a.TaskId != null && (a.CheckOutTime == null && a.CheckInTime != null))
                //                    .OrderByDescending(a => a.Id).FirstOrDefault();
                //                if (TaskWorkingHours != null)
                //                {
                //                    response.OpenTaskCheckIn = new GetOpenWorkingHoursForAllTasksDto()
                //                    {
                //                        Id = TaskWorkingHours.Id,
                //                        ProgressRate = TaskWorkingHours.ProgressRate,
                //                        CheckIn = (TimeOnly)TaskWorkingHours.CheckInTime,
                //                        Date = TaskWorkingHours.Date.ToString("yyyy-MM-dd"),
                //                        TaskId = TaskWorkingHours.TaskId,
                //                        TaskName = TaskWorkingHours.Task?.Name
                //                    }
                //                    ;
                //                }


                //                // working Hours
                //                var workingHours = WorkingHourTrackingForHrUserList
                //                        .Where(a =>  a.TaskId == null && (a.CheckOutTime == null && a.CheckInTime != null) )
                //                        .OrderByDescending(a => a.Id).FirstOrDefault();
                //                if (workingHours != null)
                //                {
                //                    response.OpenAttendanceCheckIn = new GetOpenWorkingHoursForAllTasksDto()
                //                    {
                //                        Id = workingHours.Id,
                //                        CheckIn = (TimeOnly)workingHours.CheckInTime,
                //                        Date = workingHours.Date.ToString("yyyy-MM-dd"),
                //                    }
                //                    ;
                //                }
                //                // working Hours check in and check out
                //                var LastWorkingHour = WorkingHourTrackingForHrUserList
                //                        .Where(a => a.TaskId == null && (a.CheckInTime != null || a.CheckOutTime != null) )
                //                        .OrderByDescending(a => a.Id).FirstOrDefault();
                //                if (LastWorkingHour != null)
                //                {
                //                    response.LastWorkingHourCheckIn = new LastWorkingHourDto()
                //                    {
                //                        Date = LastWorkingHour.Date.ToString("yyyy-MM-dd"),
                //                        CheckIn = LastWorkingHour.CheckInTime,
                //                        CheckOut = LastWorkingHour.CheckOutTime
                //                    }
                //                    ;
                //                }
                //            }


                //            var LocalCurrency = await _Context.Currencies.Where(x => x.IsLocal == true).FirstOrDefaultAsync();
                //            if (LocalCurrency != null)
                //            {
                //                response.LocalCurrencyId = LocalCurrency.Id;
                //                response.LocalCurrencyName = LocalCurrency.Name;
                //            }

                //            // Not From UserID => To User Id
                //            var NotificationCount = _Context.Notifications.Where(x => (x.FromUserId == CheckUserDB.Id || x.FromUserId == null) && x.New == true).Count();
                //            response.NotificationCount = NotificationCount;

                //            //var UserName = Common.GetUserName(CheckUserDB.Id, _Context);


                //            var TaskCountFromTaskCount = _Context.Tasks.Where(x => x.TaskDetails.Where(y => y.Status == "Open").Any() &&
                //            (x.CreatedBy == CheckUserDB.Id || x.TaskPermissions.Where(p => p.UserGroupId == CheckUserDB.Id).Any()
                //            )).Count();

                //            response.TaskCount = TaskCountFromTaskCount;

                //            //response.Jobtitle = CheckUserDB.JobTitleID != null ? Common.GetJobTitleName((int)CheckUserDB.JobTitleID) : "";
                //            //response.DepartmentName = CheckUserDB.DepartmentID != null ? Common.GetDepartmentName((int)CheckUserDB.DepartmentID) : "";
                //            //response.BranchName = CheckUserDB.BranchID != null ? Common.GetBranchName((int)CheckUserDB.BranchID) : "";

                //            // Fill User role List 
                //            var RoleList = new List<Roles>();
                //            var RoleListDB = await _Context.VUserRoles.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
                //            foreach (var UserRoleOBJ in RoleListDB)
                //            {
                //                RoleList.Add(new Roles
                //                {
                //                    RoleID = UserRoleOBJ.RoleId,
                //                    RoleName = UserRoleOBJ.RoleName
                //                });
                //            }



                //            // Fill User Group List 
                //            var GroupList = new List<GroupRoles>();
                //            var GroupListDB = await _Context.VGroupUserBranches.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
                //            foreach (var UserGroupOBJ in GroupListDB)
                //            {
                //                GroupList.Add(new GroupRoles
                //                {
                //                    GroupID = (long)UserGroupOBJ.GroupId,
                //                    GroupName = UserGroupOBJ.GroupName
                //                });
                //            }
                //            //var GroupList = new List<GroupRoles>();
                //            //var GroupListDB = _Context.proc_Group_UserLoadAll().Where(x => x.UserID == CheckUserDB.ID).ToList();
                //            //foreach (var UserGroupOBJ in GroupListDB)
                //            //{
                //            //        GroupList.Add(new GroupRoles
                //            //        {
                //            //            GroupID = (long)UserGroupOBJ.GroupID,
                //            //            GroupName = Common.GetGroupName(UserGroupOBJ.GroupID)
                //            //        });
                //            //}
                //            var SpecialityList = new List<SelectDDL>();
                //            SpecialityList = await _Context.CompanySpecialties.Select(x => new SelectDDL
                //            {
                //                ID = x.SpecialityId,
                //                Name = x.SpecialityName
                //            }).ToListAsync();

                //            response.SpecialityList = SpecialityList;
                //            response.RoleList = RoleList;
                //            response.GroupList = GroupList;
                //            //}




                //            // from supplier Is Owner
                //            var ClientInfo = _Context.Clients.Where(x => x.OwnerCoProfile == true).FirstOrDefault();
                //            if (ClientInfo != null)
                //            {
                //                if (ClientInfo.LogoUrl != null)
                //                {
                //                    response.ClientId = ClientInfo.Id;
                //                    response.CompanyImg = baseURL + ClientInfo.LogoUrl; //Common.GetUserPhoto(ClientInfo.Id, _Context);
                //                    /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientInfo.Id.ToString(), key)) + "&type=client&CompName=" + login.CompanyName.ToString().ToLower();*/
                //                }
                //                response.CompanyInfo = ClientInfo.Name;
                //                //response.CompanyImg = baseURL + ClientInfo.Logo;
                //            }
                //            return response;

                //        }
                //        else
                //        {
                //            response.Result = false;
                //            Models.Common.Error error = new Models.Common.Error();
                //            error.ErrorCode = "Err-P9";
                //            error.ErrorMSG = "This Email is not active ";
                //            response.Errors.Add(error);

                //        }

                //    }
                //    else
                //    {
                //        response.Result = false;
                //        Models.Common.Error error = new Models.Common.Error();
                //        error.ErrorCode = "Err-P11";
                //        error.ErrorMSG = "Invalid User";
                //        response.Errors.Add(error);

                //    }
                //    //if (CheckUserDB != null)
                //    //{
                //    //    if (CheckUserDB.Active)
                //    //    {
                //    //        if (CheckUserDB.Photo != null)
                //    //        {
                //    //            response.UserImageURL = baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.ID.ToString(), key)) + "&type=photo&CompName=" + headers["CompanyName"].ToString().ToLower();
                //    //        }

                //    //        response.Result = true;
                //    //        response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.ID.ToString(), key);
                //    //        response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                //    //        response.Jobtitle = CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                //    //        response.DepartmentName = CheckUserDB.UserDepartmentName != null ? CheckUserDB.UserDepartmentName : "";
                //    //        response.BranchName = CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                //    //        response.Data = headers["UserToken"];

                //    //        // Fill User role List 
                //    //        var RoleList = new List<Roles>();
                //    //        var RoleListDB = _Context.V_UserRole.Where(x => x.UserID == CheckUserDB.ID).ToList();
                //    //        foreach (var UserRoleOBJ in RoleListDB)
                //    //        {
                //    //            RoleList.Add(new Roles
                //    //            {
                //    //                RoleID = UserRoleOBJ.RoleID,
                //    //                RoleName = UserRoleOBJ.RoleName //Common.GetRoleName(UserRoleOBJ.RoleID)
                //    //            });
                //    //        }

                //    //        // Fill User Group List 
                //    //        var GroupList = new List<GroupRoles>();
                //    //        var GroupListDB = _Context.V_GroupUser_Branch.Where(x => x.UserID == CheckUserDB.ID).ToList();
                //    //        foreach (var UserGroupOBJ in GroupListDB)
                //    //        {
                //    //            GroupList.Add(new GroupRoles
                //    //            {
                //    //                GroupID = (long)UserGroupOBJ.GroupID,
                //    //                GroupName = UserGroupOBJ.GroupName
                //    //            });
                //    //        }

                //    //        response.RoleList = RoleList;
                //    //        response.GroupList = GroupList;
                //    //        return response;

                //    //    }
                //    //    else
                //    //    {
                //    //        response.Result = false;
                //    //        Error error = new Error();
                //    //        error.ErrorCode = "Err-P9";
                //    //        error.ErrorMSG = "This Email was not active ";
                //    //        response.Errors.Add(error);

                //    //    }

                //    //}

                //}
                #endregion

                if (response.Result)
                {
                    response = await _userService.GetUserData(validation.userID, UserToken);
                }
                return response;

            }
            catch (Exception ex)
            {
                response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                response.Errors.Add(error);

                return response;
            }

        }

/*
        [HttpGet("GetUserList")]
        public async Task<ActionResult<UserDDLResponse>> GetUserList()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            UserDDLResponse Response = new UserDDLResponse();
            Response.Result = true;
            Response.Errors = new List<Models.Common.Error>();
            try
            {

                HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
                Response.Errors = validation.errors;
                Response.Result = validation.result;

                int BranchId = 0;
                if (!string.IsNullOrEmpty(Request.Headers["BranchId"]) && int.TryParse(Request.Headers["BranchId"], out BranchId))
                {
                    int.TryParse(Request.Headers["BranchId"], out BranchId);
                }
                int RoleId = 0;
                if (!string.IsNullOrEmpty(Request.Headers["RoleId"]) && int.TryParse(Request.Headers["RoleId"], out RoleId))
                {
                    int.TryParse(Request.Headers["RoleId"], out RoleId);
                }
                long GroupId = 0;
                if (!string.IsNullOrEmpty(Request.Headers["GroupId"]) && long.TryParse(Request.Headers["GroupId"], out GroupId))
                {
                    long.TryParse(Request.Headers["GroupId"], out GroupId);
                }

                var DDLList = new List<UserDDL>();
                if (Response.Result)
                {
                    List<long> UsersIds = new List<long>();

                    var ListDBQuery = _Context.Users.Where(x => x.Active == true).AsQueryable();
                    if (BranchId != 0)
                    {
                        ListDBQuery = ListDBQuery.Where(a => a.BranchId == BranchId);
                    }
                    if (RoleId != 0)
                    {
                        var RoleUsers = await _Context.UserRoles.Where(a => a.RoleId == RoleId).Select(a => a.UserId).ToListAsync();
                        foreach (var roleUser in RoleUsers)
                        {
                            UsersIds.Add((long)roleUser);
                        }
                    }
                    if (GroupId != 0)
                    {
                        var GroupUsers = await _Context.GroupUsers.Where(a => a.GroupId == GroupId).Select(a => a.UserId).ToListAsync();
                        foreach (var grpUser in GroupUsers)
                        {
                            UsersIds.Add((long)grpUser);
                        }
                    }

                    if (UsersIds.Count() > 0)
                    {
                        ListDBQuery = ListDBQuery.Where(a => UsersIds.Contains(a.Id)).Distinct();
                    }

                    var ListDB = ListDBQuery.ToList();

                    if (ListDB.Count > 0)
                    {
                        foreach (var user in ListDB)
                        {
                            var DLLObj = new UserDDL();
                            DLLObj.ID = user.Id;
                            DLLObj.Email = user.Email.Trim(); ;
                            DLLObj.BranchId = user.BranchId;
                            DLLObj.Department = user.Department?.Name; // != null ? Common.GetDepartmentName((int)user.DepartmentID) : "";
                            DLLObj.JobTitleName = user.JobTitle?.Name; // != null ? Common.GetJobTitleName((int)user.JobTitleID) : "";
                            DLLObj.BranchName = user.Branch?.Name; // != null ? Common.GetBranchName((int)user.BranchID) : "";
                            DLLObj.Name = user.FirstName.Trim() + " " + user.LastName.Trim();

                            if (user.Photo != null)
                            {
                                DLLObj.Image = baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(user.Id.ToString(), key)) + "&type=photo&CompName=" + Request.Headers["CompanyName"].ToString().ToLower();
                            }

                            // Fill User role List 
                            var RoleList = new List<Roles>();
                            var RoleListDB = await _Context.VUserRoles.Where(x => x.UserId == user.Id).ToListAsync();
                            foreach (var UserRoleOBJ in RoleListDB)
                            {
                                RoleList.Add(new Roles
                                {
                                    RoleID = UserRoleOBJ.RoleId,
                                    RoleName = UserRoleOBJ.RoleName // Common.GetRoleName(UserRoleOBJ.RoleID)
                                });
                            }

                            // Fill User Group List 
                            var GroupList = new List<GroupRoles>();
                            var GroupListDB = await _Context.GroupUsers.Where(x => x.UserId == user.Id && x.Active == true).ToListAsync();
                            foreach (var UserGroupOBJ in GroupListDB)
                            {
                                GroupList.Add(new GroupRoles
                                {
                                    GroupID = UserGroupOBJ.GroupId,
                                    GroupName = Common.GetGroupName(UserGroupOBJ.GroupId, _Context)
                                });
                            }
                            DLLObj.RoleList = RoleList;
                            DLLObj.GroupList = GroupList;
                            DDLList.Add(DLLObj);
                        }
                    }
                }
                Response.DDLList = DDLList.Distinct().ToList();
                return Response;

            }
            catch (Exception ex)
            {
                Response.Result = false;
                Models.Common.Error error = new Models.Common.Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }*/

        [HttpGet("GetUserList")]
        public UserDDLResponse GetUserList([FromHeader]int BranchId, [FromHeader]int RoleId, [FromHeader]long GroupId, [FromHeader] string JobTitleId, [FromHeader]bool NotActiveUser, [FromHeader]bool WithTeam)
        {
            var response = new UserDDLResponse();
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
                    response = _userService.GetUserList(BranchId, RoleId, GroupId, JobTitleId,NotActiveUser, WithTeam);
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

        //---------------------------------------------project Users----------------------------------------------------------

        [HttpGet("GetUserWithJobTitleDDL")]
        public async Task<BaseResponseWithData<List<UserWithJobTitleDDL>>> GetUserWithJobTitleDDL([FromHeader] string UserName, [FromHeader] int? BranchID, [FromHeader] long projectID)
        {
            var response = new BaseResponseWithData<List<UserWithJobTitleDDL>>();
            response.Result = true;
            response.Errors = new List<Error>();

            #region user Auth
            HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            response.Errors = validation.errors;
            response.Result = validation.result;
            #endregion

            //if(UserName == null)
            //{
            //    response.Result = false;
            //    Error err = new Error();
            //    err.ErrorCode = "E-1";
            //    err.errorMSG = "the UserName Is Required";
            //    response.Errors.Add(err);
            //    return response;
            //}

            try
            {
                if (response.Result)
                {
                    response = await _userService.GetUserWithJobTitleDDL(UserName, BranchID, projectID);
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

        [HttpGet("GetUserTargetDistribution")]
        public async Task<GetUserTargetDistributionResponse> GetUserTargetDistribution([FromHeader] int BranchId, [FromHeader] long Year)
        {
            var response = new GetUserTargetDistributionResponse();
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
                    response = await _userService.GetUserTargetDistribution(BranchId, Year);
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


        //----------------------------------------------Support---------------------------------------------------


        [HttpGet("GetDateTimeEGPZone")]
        public BaseResponseWithMessage<string> GetDateTimeEGPZone()
        {
            BaseResponseWithMessage<string> response = new BaseResponseWithMessage<string>();
            response.Result = true;
            response.Errors = new List<Error>();
            try
            {
                // Get the timezone information for Egypt
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                // Get the current datetime in Egypt
                DateTime egyptDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                // Alternatively, you can specify a custom format
                string formattedDateTime = egyptDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                response.Message = formattedDateTime;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + ex.Message;
                response.Errors.Add(err);
            }
            return response;
        }


        //---------------------------------------------Teams----------------------
      

        //public string EncodeId(int id)
        //{
        //    byte[] bytes = BitConverter.GetBytes(id);
        //    return Convert.ToBase64String(bytes);
        //}

        //public int DecodeId(string encodedId)
        //{
        //    byte[] bytes = Convert.FromBase64String(encodedId);
        //    return BitConverter.ToInt32(bytes, 0);
        //}


        
    }
}
