using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;

using NewGaras.Infrastructure.DTO.User;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models.Mail;
using NewGaras.Infrastructure.Models.User;
using NewGarasAPI.Helper;

using NewGarasAPI.Models.User;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewGaras.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;
        private GarasTestContext _Context;
        private readonly string key;
        private readonly IMailService _mailService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment host, IMailService mailSettingsOptions, GarasTestContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _host = host;
            _Context = context;
            key = "SalesGarasPass";
            _mailService = mailSettingsOptions;
        }

        public async Task<BaseResponseWithData<List<UserWithJobTitleDDL>>> GetUserWithJobTitleDDL(string UserName,int? BranchID,long? projectId)
        {
            BaseResponseWithData<List<UserWithJobTitleDDL>> response = new BaseResponseWithData<List<UserWithJobTitleDDL>>();
            response.Result = true;
            response.Errors = new List<Error>();

            Expression<Func<User, bool>> criteria = (a => true);

            //criteria = a =>
            //(
            //(!string.IsNullOrEmpty(UserName) ?
            //((a.FirstName + a.MiddleName + a.LastName).Contains(UserName.Replace(" ", ""))) : true) &&
            //(BranchID != null ? a.BranchId == BranchID : true)
            //);

            if ((!string.IsNullOrEmpty(UserName) || BranchID != null ) && (projectId == null || projectId == 0))
            {
                criteria = a =>
                (
                (!string.IsNullOrEmpty(UserName) ?
                ((a.FirstName + a.MiddleName + a.LastName).Contains(UserName.Replace(" ", ""))) : true) &&
                (BranchID != null ? a.BranchId == BranchID : true) 
                );

            }

            if(projectId != null && projectId != 0)
            {
                var projectAssignUserList = _unitOfWork.ProjectAssignUsers.FindAll((a => a.ProjectId == projectId));
                var userIDs = projectAssignUserList.Select(a => a.UserId).Distinct().ToList();
                //var UserOfproject = _unitOfWork.Users.FindAll(a => userIDs.Contains(a.Id), new[] { "JobTitle" });

                criteria = a =>
                (
                (!string.IsNullOrEmpty(UserName) ?
                ((a.FirstName + a.MiddleName + a.LastName).Contains(UserName.Replace(" ", ""))) : true) &&
                (BranchID != null ? a.BranchId == BranchID : true) &&
                (projectId != null ? userIDs.Contains(a.Id):true)
                );
            }

            try
            {
                //var ListOfUsers =await _unitOfWork.Users.FindAllAsync((a => (a.FirstName + a.MiddleName + a.LastName).Contains(UserName.Replace(" ", "")) && a.BranchId == BranchID), new[] { "JobTitle" } );
                var ListOfUsers =await _unitOfWork.Users.FindAllAsync(criteria, new[] { "JobTitle" } );
                var l1 = ListOfUsers.OrderBy(x=>x.FirstName).ThenBy(x=>x.MiddleName).ThenBy(x=>x.LastName).ToList();
                
                var UserWithJobTitleList = new List<UserWithJobTitleDDL>();
                foreach (var user in l1)
                {
                    var UserWithJobTitle = new UserWithJobTitleDDL()
                    {
                        UserId = user.Id,
                        FullName = user.FirstName + " " + (user.MiddleName ?? "") + " " + user.LastName,
                        ImgPath = user.PhotoUrl != null ? Globals.baseURL + user.PhotoUrl : null,
                        JobTitleName = user.JobTitle != null ? user.JobTitle.Name : null,
                    };
                    UserWithJobTitleList.Add(UserWithJobTitle);
                }
                response.Data = UserWithJobTitleList;
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

        public async Task<GetUserTargetDistributionResponse> GetUserTargetDistribution([FromHeader] int BranchId, [FromHeader] long Year)
        {
            GetUserTargetDistributionResponse response = new GetUserTargetDistributionResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            var SalesMenDistribtionList = new List<UserTargetDistributionData>();

            try
            {
                
                if (response.Result)
                {

                    if (response.Result)
                    {


                        var GetSalesPersonsIDs = (await _unitOfWork.VUserBranchGroups.FindAllAsync(a => a.BranchId == BranchId && a.GroupName == "SalesMen")).Select(a => a.UserId).ToList();

                        var users = _unitOfWork.Users.FindAll(a => GetSalesPersonsIDs.Contains(a.Id)).ToList();

                        var GetSalesBranchUserTargetDB = (await _unitOfWork.VSalesBranchUserTargetTargetYears.FindAllAsync(x => GetSalesPersonsIDs.Contains(x.UserId) && x.Year == Year)).ToList();

                        foreach (var SalesPersonID in GetSalesPersonsIDs)
                        {
                            var GetUserBranchGroupResponse = new UserTargetDistributionData();

                            GetUserBranchGroupResponse.ID = SalesPersonID;

                            GetUserBranchGroupResponse.Name = users.Where(a=>a.Id==GetUserBranchGroupResponse.ID).Select(a=>a.FirstName+" "+a.LastName).FirstOrDefault();

                            GetUserBranchGroupResponse.BranchID = BranchId;

                            GetUserBranchGroupResponse.Amount = GetSalesBranchUserTargetDB.Where(x => x.UserId == GetUserBranchGroupResponse.ID).Select(x => x.Amount).FirstOrDefault();

                            SalesMenDistribtionList.Add(GetUserBranchGroupResponse);

                        }



                    }

                }
                response.UserTargetDistributionList = SalesMenDistribtionList;
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

        public UserDDLResponse GetUserList(int BranchId, int RoleId, long GroupId, string JobTitleId ,bool NotActiveUser = false, bool WithTeam = false)
        {
            UserDDLResponse Response = new UserDDLResponse();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                var DDLList = new List<UserDDL>();
                if (Response.Result)
                {
                    List<long> UsersIds = new List<long>();

                    var ListDBQuery = _unitOfWork.Users.FindAll(a => true, includes: new[] { "Department", "Branch", "JobTitle" }).AsQueryable();
                    if(NotActiveUser != true)
                    {
                        ListDBQuery = ListDBQuery.Where(x => x.Active == true ).AsQueryable();
                    }
                    if (BranchId != 0)
                    {
                        ListDBQuery = ListDBQuery.Where(a => a.BranchId == BranchId);
                    }
                    if (!string.IsNullOrEmpty(JobTitleId))
                    {
                        var jobTitleIDsList = JobTitleId.Split(',')
                                                    .Select(s => int.Parse(s)).ToList();


                        ListDBQuery = ListDBQuery.Where(a => jobTitleIDsList.Contains(a.JobTitleId??0));
                    }
                    if (RoleId != 0)
                    {
                        var RoleUsers = _unitOfWork.UserRoles.FindAllAsync(a => a.RoleId == RoleId).Result.Select(a => a.UserId).ToList();
                        foreach (var roleUser in RoleUsers)
                        {
                            if (roleUser != null)
                            {
                                UsersIds.Add((long)roleUser);
                            }
                        }
                    }
                    if (GroupId != 0)
                    {
                        var GroupUsers = _unitOfWork.GroupUsers.FindAllAsync(a => a.GroupId == GroupId).Result.Select(a => a.UserId).ToList();
                        foreach (var grpUser in GroupUsers)
                        {
                            if (grpUser != null)
                            {
                                UsersIds.Add((long)grpUser);
                            }
                        }
                    }

                    if (UsersIds.Count() > 0)
                    {
                        ListDBQuery = ListDBQuery.Where(a => UsersIds.Contains(a.Id)).Distinct();
                    }
                    var ListDB = ListDBQuery.ToList();

                    var HrUserList = new List<HrUser>();
                    if (WithTeam)
                    {
                        var ListUserIdsList = ListDB.Select(x => x.Id).ToList();
                        HrUserList = _unitOfWork.HrUsers.FindAll(x => ListUserIdsList.Contains(x.UserId ?? 0), new[] { "Team", "Department", "Branch", "JobTitle" }).ToList();
                    }
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
                            DLLObj.Name = user.FirstName.Trim() + " " + user.MiddleName + " " + user.LastName.Trim();
                            if (HrUserList.Count() > 0)
                            {
                                var TeamObj = HrUserList.Where(x => x.UserId == user.Id).Select(x => x.Team).FirstOrDefault();
                                if (TeamObj != null)
                                {
                                    DLLObj.TeamId = TeamObj.Id;
                                    DLLObj.TeamName = TeamObj.Name;
                                }
                            }
                            if (user.PhotoUrl != null)
                            {
                                DLLObj.Image = Globals.baseURL + user.PhotoUrl;
                            }

                            // Fill User role List 
                            var RoleList = new List<Roles>();
                            var RoleListDB = _unitOfWork.VUserRoles.FindAllAsync(x => x.UserId == user.Id).Result.ToList();
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
                            var GroupListDB = _unitOfWork.GroupUsers.FindAllAsync(x => x.UserId == user.Id && x.Active == true).Result.ToList();
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
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
                Response.Errors.Add(error);
                return Response;
            }
        }


        //public async Task<ActionResult<LoginResponse>> Login(UserLogin login)

        //{

        //    //DateTime Trxdate =DateTime.Parse("2023-05-22 00:00:00.000");
        //    //DateTime date = DateTime.Now;
        //    //TimeSpan time = new TimeSpan(0, DateTime.Now.Hour, 0, 0);
        //    //var test = (Trxdate).Add(time).AddHours(-2).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

        //    /////////////// request/////////////////////
        //    //            {
        //    //                "Username":"michael_milad",
        //    //"Password":"P@ssw0rd",
        //    //"ApplicationID":"G7N7wJhBXdc+M1XQsI2wgQ=="
        //    //          }

        //    ////////////////response/////////////////////
        //    //            {
        //    //                "ErrorCode": [
        //    //                   "Err9"
        //    //   ],
        //    //   "ErrorMSG": [
        //    //      "please write your user name and password"
        //    //   ],
        //    //   "Result": false,
        //    //   "UserID": null,
        //    //   "UserImage": null,
        //    //   "UserName": null
        //    //}
        //    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        //    string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");


        //    LoginResponse response = new LoginResponse();
        //    response.Result = true;
        //    response.Errors = new List<Error>();

        //    try
        //    {
        //        //check sent data
        //        if (login == null)
        //        {
        //            response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err-P6";
        //            error.ErrorMSG = "please write your Email and password";
        //            response.Errors.Add(error);
        //            return response;
        //        }
        //        if (string.IsNullOrEmpty(login.Email) || login.Email.Trim() == "")
        //        {

        //            response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err-P7";
        //            error.ErrorMSG = "please write your Email";
        //            response.Errors.Add(error);
        //        }
        //        if (login.ExternalLoginFrom?.ToLower() != "office365")  // Currently only temporarily
        //        {
        //            if (string.IsNullOrEmpty(login.Password) || login.Password.Trim() == "")
        //            {
        //                response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err-P8";
        //                error.ErrorMSG = "please write your password";
        //                response.Errors.Add(error);
        //            }
        //        }
        //        #region For Shared Server
        //        if (string.IsNullOrEmpty(login.CompanyName))
        //        {

        //            response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err-P7";
        //            error.ErrorMSG = "please write your Company";
        //            response.Errors.Add(error);
        //            //if (TXT_CompanyName.Text.ToLower() == "marinaplt" || TXT_CompanyName.Text.ToLower() == "proauto" || TXT_CompanyName.Text.ToLower() == "piaroma")
        //        }
        //        else if (login.CompanyName.ToLower() != "marinaplt" &&
        //                 login.CompanyName.ToLower() != "proauto" &&
        //                 login.CompanyName.ToLower() != "piaroma" &&
        //                 login.CompanyName.ToLower() != "elsalam" &&
        //                 login.CompanyName.ToLower() != "garastest" &&
        //                 login.CompanyName.ToLower() != "elwaseem" &&
        //                 login.CompanyName.ToLower() != "marinapltq" &&
        //                 login.CompanyName.ToLower() != "vetanoia" &&
        //                 login.CompanyName.ToLower() != "ramsissteel" &&
        //                 login.CompanyName.ToLower() != "periti" &&
        //                 login.CompanyName.ToLower() != "ortho" &&
        //                 login.CompanyName.ToLower() != "libmark" &&
        //                 login.CompanyName.ToLower() != "coctail" &&
        //                 login.CompanyName.ToLower() != "eldib" &&
        //                 login.CompanyName.ToLower() != "stmark" &&
        //                 login.CompanyName.ToLower() != "st.george" &&
        //                 login.CompanyName.ToLower() != "shi" &&
        //                 login.CompanyName.ToLower() != "shc" &&
        //                 login.CompanyName.ToLower() != "royaltent")
        //        {
        //            response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err-P7";
        //            error.ErrorMSG = "Invalid Company Name";
        //            response.Errors.Add(error);
        //        }
        //        #endregion
        //        if (response.Result)
        //        {

        //            #region For Shared Server

        //            // Check Entity DB 
        //            string CompName = login.CompanyName.ToLower();
        //            //UpdateContext(CompName);

        //            _Context.Database.SetConnectionString(_helper.GetConnectonString(CompName));
        //            #endregion

        //            //var CheckUserDB = _Context.proc_UserLoadAll().Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim()).FirstOrDefault();

        //            var CheckUserDB = new NewGaras.Infrastructure.Entities.User();

        //            Expression<Func<NewGaras.Infrastructure.Entities.User, bool>> Criteria = (x => true);


        //            if (login.ExternalLoginFrom?.ToLower() != "office365")
        //            {
        //                string PassEncrypted = Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim();
        //                Criteria = (x => x.Email.Trim() == login.Email.Trim() && x.Password.ToLower().Trim() == PassEncrypted);
        //            }
        //            else
        //            {
        //                Criteria = (x => x.Email.Trim() == login.Email.Trim());
        //            }

        //            CheckUserDB = await _Context.Users.Where(Criteria).Include(x => x.JobTitle).Include(x => x.Department).Include(x => x.Branch).FirstOrDefaultAsync();
        //            var test = _Context.Database.GetConnectionString();
        //            if (CheckUserDB == null && login.ExternalLoginFrom?.ToLower() != "office365")
        //            {
        //                response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err-P11";
        //                error.ErrorMSG = "Invalid Email or Password";
        //                response.Errors.Add(error);
        //                return response;
        //            }
        //            if (CheckUserDB != null)
        //            {
        //                if (CheckUserDB.Active)
        //                {
        //                    //////////////////////

        //                    if (CheckUserDB.PhotoUrl != null)
        //                    {
        //                        // user.UserImage = DBUser.Photo;
        //                        //var baseURL = OperationContext.Current.Host.BaseAddresses[0].Authority;
        //                        response.UserImageURL = baseURL + CheckUserDB.PhotoUrl; // Common.GetUserPhoto(CheckUserDB.Id, _Context);
        //                        /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key)) + "&type=photo&CompName=" + CompName;*/
        //                    }
        //                    response.EmplyeeId = _Context.HrUsers.Where(x => x.UserId == CheckUserDB.Id).Select(x => x.Id).FirstOrDefault();
        //                    long UserSessionID = 0;
        //                    //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
        //                    //if (CheckSessionOpen == null)
        //                    //{
        //                    //    var UserSessionObj = new UserSession(); // DB
        //                    //    UserSessionObj.UserID = CheckUserDB.ID;
        //                    //    UserSessionObj.Active = true;
        //                    //    UserSessionObj.CreationDate = DateTime.Now;
        //                    //    UserSessionObj.EndDate = DateTime.Now.AddDays(1);
        //                    //    UserSessionObj.ModifiedBy = "System";
        //                    //    _Context.UserSessions.Add(UserSessionObj);

        //                    //      await _Context.SaveChangesAsync();
        //                    //    UserSessionID = (long)UserSessionObj.ID;
        //                    //}
        //                    //else
        //                    //{
        //                    //    UserSessionID = CheckSessionOpen.ID;
        //                    //}


        //                    //var UserSessionInsertion = _Context.proc_UserSessionInsert(IDUserSession,
        //                    //                                                              CheckUserDB.Id,
        //                    //                                                              true,
        //                    //                                                              DateTime.Now,
        //                    //                                                              DateTime.Now.AddDays(1),
        //                    //                                                              "System");

        //                    //SqlParameter test = new SqlParameter
        //                    //{
        //                    //    Direction = System.Data.ParameterDirection.Output,
        //                    //    Value = 1,

        //                    //};

        //                    SqlParameter IDUserSession = new SqlParameter("ID", System.Data.SqlDbType.BigInt);
        //                    IDUserSession.Direction = System.Data.ParameterDirection.Output;
        //                    var userIDParameter = new SqlParameter("UserID", System.Data.SqlDbType.BigInt);
        //                    userIDParameter.Value = CheckUserDB.Id;
        //                    var activeParameter = new SqlParameter("Active", System.Data.SqlDbType.Bit);
        //                    activeParameter.Value = 1;
        //                    var creationDateParameter = new SqlParameter("CreationDate", System.Data.SqlDbType.DateTime);
        //                    creationDateParameter.Value = DateTime.Now;
        //                    var endDateParameter = new SqlParameter("EndDate", System.Data.SqlDbType.DateTime);
        //                    endDateParameter.Value = DateTime.Now.AddDays(1);
        //                    var modifiedByParameter = new SqlParameter("ModifiedBy", System.Data.SqlDbType.NVarChar);
        //                    modifiedByParameter.Value = "System";

        //                    object[] param = new object[] { IDUserSession, userIDParameter , activeParameter , creationDateParameter
        //                     ,endDateParameter,modifiedByParameter};
        //                    //List<SqlParameter> param = new List<SqlParameter>();
        //                    //param.Add(IDUserSession);
        //                    //param.Add(userIDParameter); 
        //                    //param.Add(activeParameter);
        //                    //param.Add(creationDateParameter);
        //                    //param.Add(endDateParameter);
        //                    //param.Add(modifiedByParameter);

        //                    var UserSessionInsertion = _Context.Database.SqlQueryRaw<int>("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param).AsEnumerable().FirstOrDefault();

        //                    //var UserSessionInsertion = _Context.Database.ExecuteSqlRaw
        //                    //   ("Exec proc_UserSessionInsert @ID OUTPUT, @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", param);


        //                    //var UserSessionInsertion = _Context.Database.ExecuteSqlRaw
        //                    //    ("Exec proc_UserSessionInsert @ID , @UserID, @Active ,@CreationDate, @EndDate , @ModifiedBy", IDUserSession,
        //                    //    userIDParameter,activeParameter, creationDateParameter, endDateParameter, modifiedByParameter);


        //                    UserSessionID = (long)IDUserSession.Value;
        //                    if (UserSessionID > 0)
        //                    {
        //                        /* Mark Shawky */
        //                        /* 2023-1-4 */
        //                        /* Create Daily Report If Not Exist For SalesMan User */
        //                        List<string> groups = new List<string>();
        //                        groups.Add("SalesMen");

        //                        var IsUserInGrp = await _Context.VGroupUsers.Where(a => groups.Contains(a.Name) && a.UserId == CheckUserDB.Id && a.Active == true && a.UserActive == true).ToListAsync();
        //                        if (IsUserInGrp != null && IsUserInGrp.Count > 0)
        //                        {

        //                            List<string> WeekendList = new List<string>();
        //                            var Weekends = await _Context.WeekEnds.ToListAsync();
        //                            if (Weekends != null && Weekends.Count > 0)
        //                            {
        //                                foreach (var weekend in Weekends)
        //                                {
        //                                    WeekendList.Add(weekend.Day);
        //                                }
        //                            }

        //                            bool hasReport = false;
        //                            DateTime lastReportDate = DateTime.Now.AddDays(-1).Date;
        //                            DateTime MaxReportDate = DateTime.Now.AddDays(1).Date;
        //                            var OldReport = await _Context.DailyReports.Where(a => a.UserId == CheckUserDB.Id && a.ReprotDate < MaxReportDate).OrderByDescending(a => a.ReprotDate).FirstOrDefaultAsync();
        //                            if (OldReport != null)
        //                            {
        //                                hasReport = true;
        //                                lastReportDate = OldReport.ReprotDate.Date;
        //                            }

        //                            bool createreport = false;
        //                            if (hasReport)
        //                            {
        //                                if (DateTime.Compare(lastReportDate.Date, DateTime.Now.Date) != 0)
        //                                {
        //                                    createreport = true;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                createreport = true;
        //                            }

        //                            if (createreport)
        //                            {
        //                                while (DateTime.Compare(lastReportDate.Date, DateTime.Now.Date) != 0)
        //                                {
        //                                    lastReportDate = lastReportDate.AddDays(1);
        //                                    if (!WeekendList.Contains(lastReportDate.DayOfWeek.ToString()))
        //                                    {
        //                                        DailyReport newReport = new DailyReport();
        //                                        newReport.CreationDate = DateTime.Now;
        //                                        newReport.ModifiedBy = 1;
        //                                        newReport.ModifiedDate = DateTime.Now;
        //                                        newReport.ReprotDate = lastReportDate;
        //                                        newReport.Reviewed = false;
        //                                        newReport.Status = "Not Filled";
        //                                        newReport.UserId = CheckUserDB.Id;
        //                                        _Context.DailyReports.Add(newReport);
        //                                        await _Context.SaveChangesAsync();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        ///* End */
        //                        var MainCompanyProfile = await _Context.Clients.Where(a => a.OwnerCoProfile == true).FirstOrDefaultAsync();
        //                        var MainCompanyProfileAddress = MainCompanyProfile?.ClientAddresses?.FirstOrDefault();
        //                        if (MainCompanyProfileAddress != null)
        //                        {
        //                            response.CountryId = MainCompanyProfileAddress.CountryId;
        //                            response.CountryName = MainCompanyProfileAddress.Country?.Name;
        //                        }
        //                        response.Result = true;
        //                        response.UserIDNO = CheckUserDB.Id;
        //                        response.Data = HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(UserSessionID.ToString(), key));
        //                        response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key);
        //                        response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
        //                        response.Jobtitle = CheckUserDB?.JobTitle?.Name; // CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
        //                        response.DepartmentName = CheckUserDB.Department?.Name; // != null ? CheckUserDB.UserDepartmentName : "";
        //                        response.BranchName = CheckUserDB.Branch?.Name; // CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
        //                        response.BranchID = CheckUserDB.BranchId;

        //                        //response.Jobtitle = CheckUserDB.JobTitleID != null ? Common.GetJobTitleName((int)CheckUserDB.JobTitleID) : "";
        //                        //response.DepartmentName = CheckUserDB.DepartmentID != null ? Common.GetDepartmentName((int)CheckUserDB.DepartmentID) : "";
        //                        //response.BranchName = CheckUserDB.BranchID != null ? Common.GetBranchName((int)CheckUserDB.BranchID) : "";

        //                        // Fill User role List 
        //                        var RoleList = new List<Roles>();
        //                        var RoleListDB = await _Context.VUserRoles.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
        //                        foreach (var UserRoleOBJ in RoleListDB)
        //                        {
        //                            RoleList.Add(new Roles
        //                            {
        //                                RoleID = UserRoleOBJ.RoleId,
        //                                RoleName = UserRoleOBJ.RoleName
        //                            });
        //                        }

        //                        // Fill User Group List 
        //                        var GroupList = new List<GroupRoles>();
        //                        var GroupListDB = await _Context.VGroupUserBranches.Where(x => x.UserId == CheckUserDB.Id).ToListAsync();
        //                        foreach (var UserGroupOBJ in GroupListDB)
        //                        {
        //                            GroupList.Add(new GroupRoles
        //                            {
        //                                GroupID = UserGroupOBJ.GroupId,
        //                                GroupName = UserGroupOBJ.GroupName
        //                            });
        //                        }

        //                        var SpecialityList = new List<SelectDDL>();
        //                        SpecialityList = await _Context.CompanySpecialties.Select(x => new SelectDDL
        //                        {
        //                            ID = x.SpecialityId,
        //                            Name = x.SpecialityName
        //                        }).ToListAsync();
        //                        //var GroupList = new List<GroupRoles>();
        //                        //var GroupListDB = _Context.proc_Group_UserLoadAll().Where(x => x.UserID == CheckUserDB.ID).ToList();
        //                        //foreach (var UserGroupOBJ in GroupListDB)
        //                        //{
        //                        //        GroupList.Add(new GroupRoles
        //                        //        {
        //                        //            GroupID = (long)UserGroupOBJ.GroupID,
        //                        //            GroupName = Common.GetGroupName(UserGroupOBJ.GroupID)
        //                        //        });
        //                        //}
        //                        //
        //                        var NotificationCount = _Context.Notifications.Where(x => (x.FromUserId == CheckUserDB.Id || x.FromUserId == null) && x.New == true).Count();
        //                        response.NotificationCount = NotificationCount;


        //                        var TaskCountFromTaskCount = _Context.Tasks.Where(x => x.TaskDetails.Where(y => y.Status == "Open").Any() &&
        //                        (x.CreatedBy == CheckUserDB.Id || x.TaskPermissions.Where(p => p.UserGroupId == CheckUserDB.Id).Any()
        //                        )).Count();

        //                        response.TaskCount = TaskCountFromTaskCount;


        //                        response.SpecialityList = SpecialityList;
        //                        response.RoleList = RoleList;
        //                        response.GroupList = GroupList;





        //                        //  for Working Hour Teacking and Task
        //                        var WorkingHourTrackingForHrUserList = _Context.WorkingHourseTrackings
        //                            .Include(a => a.Task)
        //                            .Where(a => a.HrUserId == response.EmplyeeId)
        //                            .OrderByDescending(a => a.Id).ToList();

        //                        if (WorkingHourTrackingForHrUserList.Count() > 0)
        //                        {

        //                            //  HR Check In Task Working Hours
        //                            var TaskWorkingHours = WorkingHourTrackingForHrUserList
        //                                .Where(a => a.TaskId != null && (a.CheckOutTime == null && a.CheckInTime != null))
        //                                .OrderByDescending(a => a.Id).FirstOrDefault();
        //                            if (TaskWorkingHours != null)
        //                            {
        //                                response.OpenTaskCheckIn = new GetOpenWorkingHoursForAllTasksDto()
        //                                {
        //                                    Id = TaskWorkingHours.Id,
        //                                    ProgressRate = TaskWorkingHours.ProgressRate,
        //                                    CheckIn = (TimeOnly)TaskWorkingHours.CheckInTime,
        //                                    Date = TaskWorkingHours.Date.ToString("yyyy-MM-dd"),
        //                                    TaskId = TaskWorkingHours.TaskId,
        //                                    TaskName = TaskWorkingHours.Task?.Name
        //                                }
        //                                ;
        //                            }


        //                            // working Hours
        //                            var workingHours = WorkingHourTrackingForHrUserList
        //                                    .Where(a => a.TaskId == null && (a.CheckOutTime == null && a.CheckInTime != null))
        //                                    .OrderByDescending(a => a.Id).FirstOrDefault();
        //                            if (workingHours != null)
        //                            {
        //                                response.OpenAttendanceCheckIn = new GetOpenWorkingHoursForAllTasksDto()
        //                                {
        //                                    Id = workingHours.Id,
        //                                    CheckIn = (TimeOnly)workingHours.CheckInTime,
        //                                    Date = workingHours.Date.ToString("yyyy-MM-dd"),
        //                                }
        //                                ;
        //                            }
        //                            // working Hours check in and check out
        //                            var LastWorkingHour = WorkingHourTrackingForHrUserList
        //                                    .Where(a => a.TaskId == null)
        //                                    .OrderByDescending(a => a.Id).FirstOrDefault();
        //                            if (LastWorkingHour != null)
        //                            {
        //                                response.LastWorkingHourCheckIn = new LastWorkingHourDto()
        //                                {
        //                                    Date = LastWorkingHour.Date.ToString("yyyy-MM-dd"),
        //                                    CheckIn = LastWorkingHour.CheckInTime,
        //                                    CheckOut = LastWorkingHour.CheckOutTime
        //                                }
        //                                ;
        //                            }
        //                        }



        //                        //                        //  HR Check In Task Working Hours
        //                        //                        var TaskWorkingHours = _Context.WorkingHourseTrackings
        //                        //                            .Include(a => a.Task)
        //                        //                            .Where(a => a.HrUserId == response.EmplyeeId && a.TaskId != null && (a.CheckOutTime == null && a.CheckInTime != null) && a.Date.Date == DateTime.Now.Date)
        //                        //                            .OrderBy(a => a.Id).LastOrDefault();
        //                        //                        if (TaskWorkingHours != null)
        //                        //                        {
        //                        //                            response.OpenTaskCheckIn = new GetOpenWorkingHoursForAllTasksDto()
        //                        //                            {
        //                        //                                Id = TaskWorkingHours.Id,
        //                        //                                ProgressRate = TaskWorkingHours.ProgressRate,
        //                        //                                CheckIn = (TimeOnly)TaskWorkingHours.CheckInTime,
        //                        //                                Date = TaskWorkingHours.Date.ToString("yyyy-MM-dd"),
        //                        //                                TaskId = TaskWorkingHours.TaskId,
        //                        //                                TaskName = TaskWorkingHours.Task?.Name
        //                        //                            }
        //                        //                            ;
        //                        //                        }


        //                        //                        // working Hours
        //                        //                        var workingHours = _Context.WorkingHourseTrackings
        //                        //.Where(a => a.HrUserId == response.EmplyeeId && a.TaskId == null && (a.CheckOutTime == null && a.CheckInTime != null) && a.Date.Date == DateTime.Now.Date)
        //                        //.OrderBy(a => a.Id).LastOrDefault();
        //                        //                        if (workingHours != null)
        //                        //                        {
        //                        //                            response.OpenAttendanceCheckIn = new GetOpenWorkingHoursForAllTasksDto()
        //                        //                            {
        //                        //                                Id = workingHours.Id,
        //                        //                                CheckIn = (TimeOnly)workingHours.CheckInTime,
        //                        //                                Date = workingHours.Date.ToString("yyyy-MM-dd"),
        //                        //                            }
        //                        //                            ;
        //                        //                        }




        //                        var LocalCurrency = await _Context.Currencies.Where(x => x.IsLocal == true).FirstOrDefaultAsync();
        //                        if (LocalCurrency != null)
        //                        {
        //                            response.LocalCurrencyId = LocalCurrency.Id;
        //                            response.LocalCurrencyName = LocalCurrency.Name;
        //                        }


        //                        // from supplier Is Owner
        //                        var ClientInfo = _Context.Clients.Where(x => x.OwnerCoProfile == true).FirstOrDefault();
        //                        if (ClientInfo != null)
        //                        {
        //                            if (ClientInfo.LogoUrl != null)
        //                            {
        //                                response.ClientId = ClientInfo.Id;
        //                                response.CompanyImg = baseURL + "/" + ClientInfo.LogoUrl; //Common.GetUserPhoto(ClientInfo.Id, _Context);
        //                                /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientInfo.Id.ToString(), key)) + "&type=client&CompName=" + login.CompanyName.ToString().ToLower();*/
        //                            }
        //                            response.CompanyInfo = ClientInfo.Name;
        //                            //response.CompanyImg = baseURL + ClientInfo.Logo;
        //                        }
        //                        return response;
        //                    }

        //                }
        //                else
        //                {
        //                    response.Result = false;
        //                    Error error = new Error();
        //                    error.ErrorCode = "Err-P9";
        //                    error.ErrorMSG = "This Email is not active ";
        //                    response.Errors.Add(error);

        //                }

        //            }
        //            else
        //            {
        //                response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err-P11";
        //                error.ErrorMSG = "Invalid Email or password";
        //                response.Errors.Add(error);

        //            }
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Result = false;
        //        Error error = new Error();
        //        error.ErrorCode = "Err10";
        //        error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
        //        response.Errors.Add(error);

        //        return response;
        //    }

        //}
                     
        public async Task<LoginResponse> GetUserData(long userID, string UserToken)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            string baseURL = MyConfig.GetValue<string>("AppSettings:baseURL");
            LoginResponse response = new LoginResponse();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {

                if (response.Result)
                {
                    //var CheckUserDB = _Context.proc_UserLoadAll().Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == Encrypt_Decrypt.Encrypt(login.Password.Trim(), key).ToLower().Trim()).FirstOrDefault();
                    var CheckUserDB = await _unitOfWork.Users.FindAsync(x => x.Id == userID, new[] { "JobTitle", "Department", "Branch" });


                    //var CheckUserDB = await _Context.V_UserInfo.Where(x => (x.Email.Trim() == login.Email.Trim()) && x.Password.ToLower().Trim() == PassEncrypted).FirstOrDefaultAsync();

                    if (CheckUserDB != null)
                    {
                        if (CheckUserDB.Active)
                        {
                            //////////////////////

                            if (CheckUserDB.PhotoUrl != null)
                            {
                                // user.UserImage = DBUser.Photo;
                                //var baseURL = OperationContext.Current.Host.BaseAddresses[0].Authority;

                                response.UserImageURL = baseURL + CheckUserDB.PhotoUrl;
                                //baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key)) + "&type=photo&CompName=" + Request.Headers["CompanyName"].ToString().ToLower();
                            }
                            //long UserSessionID = 0;
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
                            //var CheckSessionOpen = await _Context.UserSessions.Where(x => x.Active == true && x.UserID == CheckUserDB.ID && x.EndDate > DateTime.Now).OrderByDescending(x => x.CreationDate).FirstOrDefaultAsync();
                            //if (CheckSessionOpen != null)
                            //{

                            response.EmplyeeId = _unitOfWork.HrUsers.FindAll(x => x.UserId == CheckUserDB.Id).Select(x => x.Id).FirstOrDefault();
                            response.Result = true;
                            response.Data = UserToken; // HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckSessionOpen.ID.ToString(), key));
                            response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.Id.ToString(), key);
                            response.UserIDNO = CheckUserDB.Id;
                            response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                            response.Jobtitle = CheckUserDB?.JobTitle?.Name; // CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                            response.DepartmentName = CheckUserDB.Department?.Name; // != null ? CheckUserDB.UserDepartmentName : "";
                            response.BranchName = CheckUserDB.Branch?.Name; // CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                            response.BranchID = CheckUserDB.BranchId;
                            var MainCompanyProfile = await _unitOfWork.Clients.FindAsync(a => a.OwnerCoProfile == true);
                            var MainCompanyProfileAddress = MainCompanyProfile?.ClientAddresses?.FirstOrDefault();
                            if (MainCompanyProfileAddress != null)
                            {
                                response.CountryId = MainCompanyProfileAddress.CountryId;
                                response.CountryName = MainCompanyProfileAddress.Country?.Name;
                            }

                            //  for Working Hour Teacking and Task
                            var WorkingHourTrackingForHrUserList = _unitOfWork.WorkingHoursTrackings
                                .FindAll(a => a.HrUserId == response.EmplyeeId, new[] { "Task" })
                                .OrderByDescending(a => a.Id).ToList();



                            var LocalCurrency = await _unitOfWork.Currencies.FindAsync(x => x.IsLocal == true);
                            if (LocalCurrency != null)
                            {
                                response.LocalCurrencyId = LocalCurrency.Id;
                                response.LocalCurrencyName = LocalCurrency.Name;
                            }

                            // Not From UserID => To User Id
                            var NotificationCount = _unitOfWork.Notifications.Count(x => (x.FromUserId == CheckUserDB.Id || x.FromUserId == null) && x.New == true);
                            response.NotificationCount = NotificationCount;

                            //var UserName = Common.GetUserName(CheckUserDB.Id, _Context);


                            var TaskCountFromTaskCount = _unitOfWork.Tasks.FindAll(x => x.TaskDetails.Where(y => y.Status == "Open").Any() &&
                            (x.CreatedBy == CheckUserDB.Id || x.TaskPermissions.Where(p => p.UserGroupId == CheckUserDB.Id).Any()
                            )).Count();

                            response.TaskCount = TaskCountFromTaskCount;

                            //response.Jobtitle = CheckUserDB.JobTitleID != null ? Common.GetJobTitleName((int)CheckUserDB.JobTitleID) : "";
                            //response.DepartmentName = CheckUserDB.DepartmentID != null ? Common.GetDepartmentName((int)CheckUserDB.DepartmentID) : "";
                            //response.BranchName = CheckUserDB.BranchID != null ? Common.GetBranchName((int)CheckUserDB.BranchID) : "";

                            // Fill User role List 
                            var RoleList = new List<Roles>();
                            var RoleListDB = await _unitOfWork.VUserRoles.FindAllAsync(x => x.UserId == CheckUserDB.Id);
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
                            var GroupListDB = await _unitOfWork.VGroupUserBranchs.FindAllAsync(x => x.UserId == CheckUserDB.Id);
                            foreach (var UserGroupOBJ in GroupListDB)
                            {
                                GroupList.Add(new GroupRoles
                                {
                                    GroupID = (long)UserGroupOBJ.GroupId,
                                    GroupName = UserGroupOBJ.GroupName
                                });
                            }
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
                            var SpecialityList = new List<SelectDDL>();
                            SpecialityList =  _unitOfWork.CompanySpecialties.FindAll(a => true).Select(x => new SelectDDL
                            {
                                ID = x.SpecialityId,
                                Name = x.SpecialityName
                            }).ToList();

                            response.SpecialityList = SpecialityList;
                            response.RoleList = RoleList;
                            response.GroupList = GroupList;
                            //}
                            if (response.EmplyeeId != null)
                            {

                            var ContractDetails = _unitOfWork.Contracts.FindAll(x => x.HrUserId == response.EmplyeeId && x.IsCurrent == true).FirstOrDefault();
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
                                    response.CompanyImg = baseURL + ClientInfo.LogoUrl; //Common.GetUserPhoto(ClientInfo.Id, _Context);
                                    /*"/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(ClientInfo.Id.ToString(), key)) + "&type=client&CompName=" + login.CompanyName.ToString().ToLower();*/
                                }
                                response.CompanyInfo = ClientInfo.Name;
                                //response.CompanyImg = baseURL + ClientInfo.Logo;
                            }
                            return response;

                        }
                        else
                        {
                            response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err-P9";
                            error.ErrorMSG = "This Email is not active ";
                            response.Errors.Add(error);

                        }

                    }
                    else
                    {
                        response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err-P11";
                        error.ErrorMSG = "Invalid User";
                        response.Errors.Add(error);

                    }
                    //if (CheckUserDB != null)
                    //{
                    //    if (CheckUserDB.Active)
                    //    {
                    //        if (CheckUserDB.Photo != null)
                    //        {
                    //            response.UserImageURL = baseURL + "/ShowImage.ashx?ImageID=" + HttpUtility.UrlEncode(Encrypt_Decrypt.Encrypt(CheckUserDB.ID.ToString(), key)) + "&type=photo&CompName=" + headers["CompanyName"].ToString().ToLower();
                    //        }

                    //        response.Result = true;
                    //        response.UserID = Encrypt_Decrypt.Encrypt(CheckUserDB.ID.ToString(), key);
                    //        response.UserName = CheckUserDB.FirstName + " " + CheckUserDB.LastName;
                    //        response.Jobtitle = CheckUserDB.UserJobTitle != null ? CheckUserDB.UserJobTitle : "";
                    //        response.DepartmentName = CheckUserDB.UserDepartmentName != null ? CheckUserDB.UserDepartmentName : "";
                    //        response.BranchName = CheckUserDB.UserBranchName != null ? CheckUserDB.UserBranchName : "";
                    //        response.Data = headers["UserToken"];

                    //        // Fill User role List 
                    //        var RoleList = new List<Roles>();
                    //        var RoleListDB = _Context.V_UserRole.Where(x => x.UserID == CheckUserDB.ID).ToList();
                    //        foreach (var UserRoleOBJ in RoleListDB)
                    //        {
                    //            RoleList.Add(new Roles
                    //            {
                    //                RoleID = UserRoleOBJ.RoleID,
                    //                RoleName = UserRoleOBJ.RoleName //Common.GetRoleName(UserRoleOBJ.RoleID)
                    //            });
                    //        }

                    //        // Fill User Group List 
                    //        var GroupList = new List<GroupRoles>();
                    //        var GroupListDB = _Context.V_GroupUser_Branch.Where(x => x.UserID == CheckUserDB.ID).ToList();
                    //        foreach (var UserGroupOBJ in GroupListDB)
                    //        {
                    //            GroupList.Add(new GroupRoles
                    //            {
                    //                GroupID = (long)UserGroupOBJ.GroupID,
                    //                GroupName = UserGroupOBJ.GroupName
                    //            });
                    //        }

                    //        response.RoleList = RoleList;
                    //        response.GroupList = GroupList;
                    //        return response;

                    //    }
                    //    else
                    //    {
                    //        response.Result = false;
                    //        Error error = new Error();
                    //        error.ErrorCode = "Err-P9";
                    //        error.ErrorMSG = "This Email was not active ";
                    //        response.Errors.Add(error);

                    //    }

                    //}

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
