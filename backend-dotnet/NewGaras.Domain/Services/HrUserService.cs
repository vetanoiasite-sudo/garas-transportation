using AutoMapper;
using Azure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.DTO.HrUser;
using NewGaras.Infrastructure.DTO.VacationType;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.HrUser;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using NewGarasAPI.Helper;
using NewGarasAPI.Models.User;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = NewGarasAPI.Models.Common.Error;


namespace NewGaras.Domain.Services
{
    public class HrUserService : IHrUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private UnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;
        private readonly string key;
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
        public HrUserService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment host)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _host = host;
            key = "SalesGarasPass";
        }

        public async Task<BaseResponseWithId<long>> CreateHrUser(HrUserDto NewHrUser, long UserId, string CompanyName)
        {
            var response = new BaseResponseWithId<long>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            try
            {
                #region validation 
                string errorMessage = "";
                DateTime DoB = DateTime.Now;
                if (NewHrUser.DateOfBirth != null)
                {

                    if (!DateTime.TryParse(NewHrUser.DateOfBirth, out DoB))
                    {
                        response.Result = false;
                        //Error err = new Error();
                        //err.ErrorCode = "E-1";
                        errorMessage = errorMessage + "a valid Date of Birth - ";
                        //response.Errors.Add(err);
                        //return response;
                    }

                }
                if (NewHrUser.FirstName == null || NewHrUser.FirstName == "")
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    errorMessage = errorMessage + "FirstName - ";
                    //response.Errors.Add(err);
                    //return response;
                }
                if (NewHrUser.ARLastName == null || NewHrUser.ARLastName == "")
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    errorMessage = errorMessage + "ArlastName - ";
                    //response.Errors.Add(err);
                    //return response;
                }
                if (NewHrUser.LastName == null || NewHrUser.LastName == "")
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    errorMessage = errorMessage + "lastName - ";
                    //response.Errors.Add(err);
                    //return response;
                }
                string MiddleName = "";
                if (!string.IsNullOrWhiteSpace(NewHrUser.MiddleName))
                {
                    MiddleName = NewHrUser.MiddleName;
                }

                if (NewHrUser.Email == null || NewHrUser.Email == "")
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    errorMessage = errorMessage + "Email - ";
                    //response.Errors.Add(err);
                    //return response;
                }
                if (string.IsNullOrWhiteSpace(NewHrUser.Mobile))
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    errorMessage = errorMessage + "Mobile ";
                    //response.Errors.Add(err);
                    //return response;
                }
                //if (NewHrData.Gender == null || NewHrData.Gender == "")
                //{
                //    response.Result = false;
                //    Error err = new Error();
                //    err.ErrorCode = "E-1";
                //    err.errorMSG = "please, Enter a valid Gender :";
                //    response.Errors.Add(err);
                //    return response;
                //}
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = errorMessage.Insert(0, "Please add( ");
                    string finalErrorMessage = errorMessage + $") before Submission";
                    Error error = new Error();
                    error.ErrorMSG = finalErrorMessage;
                    response.Errors.Add(error);
                }
                #endregion

                var allHrUsers = await _unitOfWork.HrUsers.GetAllAsync();
                var allHrUsersFullName = allHrUsers.Select(a => a.FirstName.ToLower() + a.MiddleName.ToLower() + a.LastName.ToLower()).ToList();

                var newUserName = "";
                if (!string.IsNullOrWhiteSpace(NewHrUser.FirstName) && !string.IsNullOrWhiteSpace(NewHrUser.LastName))
                {
                    newUserName = NewHrUser.FirstName.Replace(" ", "") + MiddleName.Replace(" ", "") + NewHrUser.LastName.Replace(" ", "");
                }

                #region not repeating
                var notRepatingMessage = "";
                if (allHrUsersFullName.Contains(newUserName.ToLower()))
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    notRepatingMessage = notRepatingMessage + " This FullName is already Exists please, Choose another one -";
                    //response.Errors.Add(err);
                    //return response;
                }
                var allHrUsersEmails = allHrUsers.Select(a => a.Email);
                if (allHrUsersEmails.Contains(NewHrUser.Email) || allHrUsersEmails.Contains(NewHrUser.Email.ToLower()))
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    notRepatingMessage = notRepatingMessage + " This Email is already Exists please, Enter a valid Email -";
                    //response.Errors.Add(err);
                    //return response;
                }
                var allHrUsersLandLines = allHrUsers.Where(a => a.LandLine != null).Select(a => a.LandLine);
                if (allHrUsersLandLines.Contains(NewHrUser.LandLine))
                {
                    response.Result = false;
                    //Error err = new Error();
                    //err.ErrorCode = "E-1";
                    notRepatingMessage = notRepatingMessage + " This Home is already Exists please, Enter a valid Home -";
                    //response.Errors.Add(err);
                    //return response;
                }
                if (NewHrUser.Mobile != "0")
                {
                    var allHrUsersMobileNumbers = allHrUsers.Select(a => a.Mobile);
                    if (allHrUsersMobileNumbers.Contains(NewHrUser.Mobile))
                    {
                        response.Result = false;
                        //Error err = new Error();
                        //err.ErrorCode = "E-1";
                        notRepatingMessage = notRepatingMessage + " This Mobile is already Exists please, Enter a valid Mobile ";
                        //response.Errors.Add(err);
                        //return response;
                    }
                }

                if (!string.IsNullOrWhiteSpace(notRepatingMessage))
                {
                    Error error = new Error();
                    error.ErrorMSG = notRepatingMessage;
                    response.Errors.Add(error);
                }

                if (!string.IsNullOrWhiteSpace(errorMessage) || !string.IsNullOrWhiteSpace(notRepatingMessage)) return response;

                #endregion

                #region check in DB
                if (NewHrUser.BranchID != null)
                {
                    var branch = _unitOfWork.Branches.FindAll(a => a.Id == NewHrUser.BranchID).FirstOrDefault();
                    if (branch == null)
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "No Branch with this ID :";
                        response.Errors.Add(err);
                        return response;
                    }
                }
                if (NewHrUser.JobTitleID != null)
                {
                    var branch = _unitOfWork.JobTitles.FindAll(a => a.Id == NewHrUser.JobTitleID).FirstOrDefault();
                    if (branch == null)
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "No JobTilte with this ID :";
                        response.Errors.Add(err);
                        return response;
                    }
                }
                if (NewHrUser.DepartmentID != null)
                {
                    var branch = _unitOfWork.Departments.FindAll(a => a.Id == NewHrUser.DepartmentID).FirstOrDefault();
                    if (branch == null)
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "No Department with this ID :";
                        response.Errors.Add(err);
                        return response;
                    }
                }
                if (NewHrUser.TeamId != null)
                {
                    var branch = _unitOfWork.Teams.FindAll(a => a.Id == NewHrUser.TeamId).FirstOrDefault();
                    if (branch == null)
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "No Team with this ID :";
                        response.Errors.Add(err);
                        return response;
                    }
                }
                #endregion


                IFormFile ImgInMemory = null;
                if (NewHrUser.Photo != null)
                {
                    ImgInMemory = NewHrUser.Photo;
                }
                var user = _mapper.Map<HrUser>(NewHrUser);
                user.Email = user.Email.ToLower();
                if (NewHrUser.DateOfBirth != null) user.DateOfBirth = DoB;
                //--------------Trim() spaces from full name-------------------------
                user.FirstName = user.FirstName.Trim();
                user.MiddleName = MiddleName.Trim();
                user.LastName = user.LastName.Trim();
                //-------------------------------------------------------------------
                user.ImgPath = null; //Common.SaveFileIFF(virtualPath, file, FileName, fileExtension, _host);
                user.CreationDate = DateTime.Now;
                user.ModifiedById = UserId;
                user.CreatedById = UserId;
                user.MaritalStatusId = NewHrUser.MaritalStatusId;
                user.Modified = DateTime.Now;
                user.IsDeleted = false;
                //user.OldId = 0;
                var HrUser = await _unitOfWork.HrUsers.AddAsync(user);
                _unitOfWork.Complete();
                response.ID = HrUser.Id;
                long lastUserId = HrUser.Id;

                if (ImgInMemory != null)
                {
                    var fileExtension = ImgInMemory.FileName.Split('.').Last();
                    var virtualPath = $"Attachments\\{CompanyName}\\HrUser\\{HrUser.Id}\\";
                    var FileName = System.IO.Path.GetFileNameWithoutExtension(ImgInMemory.FileName.Trim().Replace(" ", ""));
                    HrUser.ImgPath = Common.SaveFileIFF(virtualPath, ImgInMemory, FileName, fileExtension, _host);
                }
                _unitOfWork.Complete();

                if (NewHrUser.TeamId != null)
                {

                    var newTeamUser = new UserTeam();
                    newTeamUser.TeamId = NewHrUser.TeamId ?? 0;
                    newTeamUser.HrUserId = HrUser.Id;
                    newTeamUser.CreatedBy = UserId;
                    newTeamUser.CreatedDate = DateTime.Now;
                    newTeamUser.ModifiedBy = UserId;
                    newTeamUser.ModifiedDate = DateTime.Now;

                    var teamUser = await _unitOfWork.UserTeams.AddAsync(newTeamUser);
                    _unitOfWork.Complete();

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

        public async Task<BaseResponseWithDataAndHeader<List<HrUserCardDto>>> GetAll(int CurrentPage, int NumberOfItemsPerPage, string? searchKey,
            bool? active, int? DepId, int? jobTilteId, int? BranchId, bool? isUser, string? Email, string? mobile, bool? isDeleted, bool? ActiveUser,
            int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus)
        {
            var response = new BaseResponseWithDataAndHeader<List<HrUserCardDto>>();
            response.Result = true;
            response.Errors = new List<Error>();
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string BaseURL = MyConfig.GetValue<string>("AppSettings:baseURL");

            ////string BaseURL = "https://garascore.garassolutions.com\\";
            //string BaseURL = "https://byapi.garassolutions.com\\";



            try
            {
                if (response.Result)
                {
                    //var data = await _unitOfWork.HrUsers.FindAllAsync((a => a.Active == true), new[] { "JobTitle" });
                    var data = _unitOfWork.HrUsers.FindAllQueryable(a => true, new[] { "JobTitle", "User" }) ;

                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        data = data.Where(a => (a.FirstName + a.MiddleName + a.LastName).Contains(searchKey.Replace(" ", "")) || (a.Email).Contains(searchKey) || (a.Mobile).Contains(searchKey));
                    }
                    if(active != null)
                    {
                        data = data.Where(a => a.Active == active).AsQueryable();
                    }
                    if (DepId != null)
                    {
                        data = data.Where(a => a.DepartmentId == DepId).AsQueryable();
                    }
                    if (jobTilteId != null)
                    {
                        data = data.Where(a => a.JobTitleId == jobTilteId).AsQueryable();
                    }
                    if (BranchId != null)
                    {
                        data = data.Where(a => a.BranchId == BranchId).AsQueryable();
                    }
                    if (isUser != null)
                    {
                        data = data.Where(a => a.IsUser == isUser).AsQueryable();
                    }
                    if (isDeleted == true)
                    {
                        data = data.Where(a => a.IsDeleted == true).AsQueryable();
                    }
                    if (Email != null)
                    {
                        data = data.Where(a => a.Email == Email).AsQueryable();
                    }

                    if (DateSerach != null && (  TransportionlineId > 0 || SupplierId > 0 || supplierContactPersonId > 0 || !string.IsNullOrEmpty(serialBus)) )
                    {
                        var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.Active == true, new[] { "TransportationLine", "SupplierContactPerson", "Supplier", "TransportationLine.TransportationVehicle.VehicleType", "Supervisor" });
                        var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);
                        var transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => (x.CheckIn.HasValue && x.CheckIn.Value.Date == DateSerach.Value.Date) ||
                                                           (x.CheckOut.HasValue && x.CheckOut.Value.Date == DateSerach.Value.Date) && x.Type == "Person").ToList();
                       
                        // Apply specific filters based on conditions
                        if (TransportionlineId > 0)
                        {
                            VehicleRoute = VehicleRoute.Where(x => x.TransportationLineId == TransportionlineId);
                        }
                        if (SupplierId > 0)
                        {
                            VehicleRoute = VehicleRoute.Where(x => x.SupplierId == SupplierId);
                        }
                        if (supplierContactPersonId > 0)
                        {
                            VehicleRoute = VehicleRoute.Where(x => x.SupplierContactPersonId == supplierContactPersonId);
                        }
                        if (!string.IsNullOrEmpty(serialBus))
                        {
                            VehicleRoute = VehicleRoute.Where(x => x.Active && x.Serial == serialBus);
                        }

                        // Collect all matching route IDs
                        var transportationVehicleRouteIds = VehicleRoute.Select(t => t.Id).ToList();

                        // Attendance users (all employees)
                        var allAttendanceUserIds = transprotationUserAttedance
                            .Where(x => x.Type == "Person")
                            .Select(y => long.Parse(y.Serial))
                            .ToList();

                        // Users for these routes
                        var routeHrUserIds = transportationVehicleRouteEmployee
                            .Where(x => transportationVehicleRouteIds.Contains(x.TransportationVehicleRouteId))
                            .Select(y => (long)y.HrUserId)
                            .ToList();

                        // Apply final filter on HrUserDb
                        data = data.Where(x => routeHrUserIds.Contains(x.Id));

                    }

                      

                    else
                    {
                        data = data.Where(a => a.IsDeleted != true).AsQueryable();
                    }

                    data = data.OrderBy(a => (a.FirstName + " " + (a.LastName != null ? (a.LastName + " ") : "") + a.LastName));

                    var hrUserData = PagedList<HrUser>.Create(data, CurrentPage, NumberOfItemsPerPage);
                    response.Data = hrUserData.Select(x => new HrUserCardDto
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        JobTitle = x.JobTitle?.Name,
                        IsUser = x.IsUser,
                        ImgPath = x.ImgPath != null ? BaseURL + x.ImgPath : null
                    }).ToList();
                    PaginationHeader paginationHeader = new PaginationHeader();
                    paginationHeader.CurrentPage = CurrentPage;
                    paginationHeader.ItemsPerPage = NumberOfItemsPerPage;
                    paginationHeader.TotalPages = hrUserData.TotalPages;
                    paginationHeader.TotalItems = hrUserData.TotalCount;

                    response.PaginationHeader = paginationHeader;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorCode = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        public BaseResponseWithDataAndHeader<HrUserListDDL> GetHrUserListDDl(int CurrentPage, int NumberOfItemsPerPage, string? searchKey, long? DoctorSpecialtyId)
        {
            var response = new BaseResponseWithDataAndHeader<HrUserListDDL>();
            response.Result = true;
            response.Errors = new List<Error>();
            try
            {
                var userIDsOfTeam = new List<long>();
                if(DoctorSpecialtyId != null)
                {
                    var userTeamDB = _unitOfWork.UserTeams.FindAll(a => a.TeamId == DoctorSpecialtyId);
                    var usersIDs = userTeamDB.Select(a => a.UserId).ToList();

                    foreach (var id in usersIDs)
                    {
                        userIDsOfTeam.Add(id??0);
                    }
                }

                var AllHrUsers = _unitOfWork.HrUsers.FindAllQueryable(a => true);

                if (searchKey != null)
                {
                    searchKey = HttpUtility.UrlDecode(searchKey);
                    AllHrUsers = AllHrUsers.Where(a => (a.FirstName + a.LastName).Contains(searchKey)).AsQueryable();
                }
                if(DoctorSpecialtyId != null)
                {
                    AllHrUsers = AllHrUsers.Where(a => userIDsOfTeam.Contains(a.UserId??0)).AsQueryable();
                }
                var finalList = PagedList<HrUser>.Create(AllHrUsers, CurrentPage, NumberOfItemsPerPage);
                var ModelList = _mapper.Map<List<HrUserListDDLModel>>(finalList.ToList());
                response.Data = new HrUserListDDL()
                {
                    HrUserLists = ModelList
                };
                response.PaginationHeader = new PaginationHeader()
                {
                    CurrentPage = finalList.CurrentPage,
                    TotalPages = finalList.TotalPages,
                    TotalItems = finalList.TotalCount,
                    ItemsPerPage = finalList.PageSize
                };
                return response;
            }
            catch(Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorCode = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        public async Task<BaseResponseWithData<GetHrUserDto>> GetHrUser(long HrUserId, long systemUserId)
        {
            var response = new BaseResponseWithData<GetHrUserDto>();
            response.Result = true;
            response.Errors = new List<Error>();

            //#region user Auth
            //HearderVaidatorOutput validation = _helper.ValidateHeader(Request.Headers, ref _Context);
            //response.Errors = validation.errors;
            //response.Result = validation.result;
            //#endregion


            try
            {
                if (response.Result)
                {
                    if (HrUserId != 0)
                    {
                        //using (GarasTestContext _context = new GarasTestContext())
                        //    Helper _helper = new Helper();
                        //string conn=_helper.GetConnectonString("garastest");
                        //_context.Database.SetConnectionString(conn);
                        //unitOfWork = new UnitOfWork(_context);

                        var UserDtoData = await _unitOfWork.HrUsers.FindAsync((HU => HU.Id == HrUserId), new[] { "User", "Department", "JobTitle", "Branch", "Team", "MaritalStatus" });
                        if (UserDtoData == null)
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This HR User Id is not found ";
                            response.Errors.Add(err);
                            return response;
                        }
                        response.Data = _mapper.Map<GetHrUserDto>(UserDtoData);
                    }
                    if (systemUserId != 0)
                    {
                        var UserDtoData = await _unitOfWork.HrUsers.FindAsync((HU => HU.UserId == systemUserId), new[] { "User", "Department", "JobTitle", "Branch", "Team", "MaritalStatus" });
                        if (UserDtoData == null)
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This HR User Id is not found ";
                            response.Errors.Add(err);
                            return response;
                        }
                        response.Data = _mapper.Map<GetHrUserDto>(UserDtoData);
                    }

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

        public async Task<BaseResponseWithData<UserEmployeeResponse>> AddHrEmployeeToUserAsync(AddHrEmployeeToUserDTO InData, long userId, string key)
        {
            var response = new BaseResponseWithData<UserEmployeeResponse>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            try
            {
                if (response.Result)
                {
                    var allUsers = await _unitOfWork.Users.GetAllAsync();
                    var allUsersEmails = allUsers.Select(x => x.Email);
                    var HrUser = await _unitOfWork.HrUsers.FindAsync((HU => HU.Id == InData.HrUserId), new[] { "Department", "JobTitle", "Branch" });
                    if (HrUser != null)
                    {
                        if (HrUser.IsUser == true && HrUser.UserId != null)
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "This HR Employee is already a User";
                            response.Errors.Add(err);
                            return response;
                        }
                        NewGaras.Infrastructure.Entities.User usr = new NewGaras.Infrastructure.Entities.User();

                        if (InData.Email == null || InData.Email == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "Please, enter a valid Email";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (allUsersEmails.Contains(InData.Email) || allUsersEmails.Contains(InData.Email.ToLower()))
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "This Email is already Exists ,Please enter a valid Email";
                            response.Errors.Add(err);
                            return response;
                        }
                        usr.Email = InData.Email.ToLower();

                        if (InData.Password == null || InData.Password == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "Please, enter a valid password";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (InData.ConfirmPass == null || InData.ConfirmPass == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "Please, enter a valid Confirm password";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (InData.Password == InData.ConfirmPass)
                        {
                            usr.Password = Encrypt_Decrypt.Encrypt(InData.Password, key);
                        }
                        else
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-20";
                            err.errorMSG = "The password and confirm password did not match!";
                            response.Errors.Add(err);
                            return response;
                        }

                        #region add Data to user table
                        usr.FirstName = HrUser.FirstName;
                        usr.LastName = HrUser.LastName;
                        usr.MiddleName = HrUser.MiddleName;
                        usr.Mobile = HrUser.Mobile;
                        usr.Active = HrUser.Active;
                        usr.CreationDate = DateTime.Now;
                        usr.CreatedBy = userId;
                        usr.ModifiedBy = userId;
                        usr.Modified = DateTime.Now;
                        usr.Gender = HrUser.Gender;
                        usr.BranchId = HrUser.BranchId;
                        usr.DepartmentId = HrUser.DepartmentId;
                        usr.JobTitleId = HrUser.JobTitleId;
                        usr.PhotoUrl = HrUser.ImgPath;
                        #endregion

                        var User = _unitOfWork.Users.Add(usr);
                        _unitOfWork.Complete();

                        HrUser.IsUser = true;
                        HrUser.UserId = User.Id;


                        _unitOfWork.Complete();
                        UserEmployeeResponse UserEmployeeResponse = new UserEmployeeResponse();
                        UserEmployeeResponse.Id = User.Id;
                        UserEmployeeResponse.UserName = usr.FirstName + " " + usr.LastName;
                        UserEmployeeResponse.UserEmail = HrUser.Email;
                        response.Data = UserEmployeeResponse;

                        //Common.CreateNotification(validation.userID, "Welcome , " + HrUser.FirstName, "Thank you for joining our system", "#", true, usr.Id, 0, _Context);

                    }
                    else
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "The ID of HrUser is Not Valid , no HrUser with this Id";
                        response.Errors.Add(err);
                        return response;
                    }
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

        public async Task<BaseResponseWithData<UserDataResponse>> EditHrEmployee(EditHrEmployeeDto NewHrData, long userId, string CompanyName, string key)
        {

            var response = new BaseResponseWithData<UserDataResponse>()
            {
                Result = true,
                Errors = new List<Error>()
            };
            UserDataResponse UserDataResponse = new UserDataResponse();


            var allHrUsers = await _unitOfWork.HrUsers.GetAllAsync(); // to be Edited

            var HrUser = await _unitOfWork.HrUsers.FindAsync((HU => HU.Id == NewHrData.HrUserId), new[] { "User" });

            #region validation 
            DateTime DoB = DateTime.Now;
            if (NewHrData.DateOfBirth != null)
            {
                if (!DateTime.TryParse(NewHrData.DateOfBirth, out DoB))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "please, Enter a valid Date of Birth :";
                    response.Errors.Add(err);
                    return response;
                }
                if (DoB.Year >= DateTime.Now.Year)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "please, Enter a valid Date :";
                    response.Errors.Add(err);
                    return response;
                }
            }
            if (NewHrData.FirstName == null || NewHrData.FirstName == "")
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "please, Enter a valid FrstName :";
                response.Errors.Add(err);
                return response;
            }
            if (NewHrData.ARLastName == null || NewHrData.ARLastName == "")
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "please, Enter a valid ArlastName :";
                response.Errors.Add(err);
                return response;
            }
            if (NewHrData.LastName == null || NewHrData.LastName == "")
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "please, Enter a valid lastName :";
                response.Errors.Add(err);
                return response;
            }
            string MiddleName = "";
            if (!string.IsNullOrWhiteSpace(NewHrData.MiddleName))
            {
                MiddleName = NewHrData.MiddleName;
            }
            //if (NewHrData.Mobile == null || NewHrData.Mobile == "")
            //{
            //    response.Result = false;
            //    Error err = new Error();
            //    err.ErrorCode = "E-1";
            //    err.errorMSG = "please, Enter a valid Mobile number :";
            //    response.Errors.Add(err);
            //    return response;
            //}
            if (NewHrData.Email == null || NewHrData.Email == "")
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "please, Enter a valid Email:";
                response.Errors.Add(err);
                return response;
            }
            if (string.IsNullOrWhiteSpace(NewHrData.Mobile))
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "please, Enter a valid Mobile:";
                response.Errors.Add(err);
                return response;
            }
            //if (NewHrData.Gender == null || NewHrData.Gender == "")
            //{
            //    response.Result = false;
            //    Error err = new Error();
            //    err.ErrorCode = "E-1";
            //    err.errorMSG = "please, Enter a valid Gender :";
            //    response.Errors.Add(err);
            //    return response;
            //}
            if (NewHrData.IsUser == true)
            {
                if (string.IsNullOrWhiteSpace(NewHrData.systemEmail))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "please, Enter a valid user Email :";
                    response.Errors.Add(err);
                    return response;
                }
                if (string.IsNullOrWhiteSpace(NewHrData.password) && HrUser.UserId is null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "please, Enter a valid password :";
                    response.Errors.Add(err);
                    return response;
                }
                if (string.IsNullOrWhiteSpace(NewHrData.confirmPassword) && HrUser.UserId is null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "please, Enter a valid Confirm password :";
                    response.Errors.Add(err);
                    return response;
                }
                if (NewHrData.password != NewHrData.confirmPassword)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "The password and confirm password did not match:";
                    response.Errors.Add(err);
                    return response;
                }
            }

            #endregion

            #region check in DB
            if (NewHrData.BranchID != null)
            {
                var branch = _unitOfWork.Branches.FindAll(a => a.Id == NewHrData.BranchID).FirstOrDefault();
                if (branch == null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "No Branch with this ID :";
                    response.Errors.Add(err);
                    return response;
                }
            }
            if (NewHrData.JobTitleID != null)
            {
                var branch = _unitOfWork.JobTitles.FindAll(a => a.Id == NewHrData.JobTitleID).FirstOrDefault();
                if (branch == null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "No JobTilte with this ID :";
                    response.Errors.Add(err);
                    return response;
                }
            }
            if (NewHrData.DepartmentID != null)
            {
                var branch = _unitOfWork.Departments.FindAll(a => a.Id == NewHrData.DepartmentID).FirstOrDefault();
                if (branch == null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "No Department with this ID :";
                    response.Errors.Add(err);
                    return response;
                }
            }
            if (NewHrData.TeamId != null)
            {
                var branch = _unitOfWork.Teams.FindAll(a => a.Id == NewHrData.TeamId).FirstOrDefault();
                if (branch == null)
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "No Team with this ID :";
                    response.Errors.Add(err);
                    return response;
                }
            }
            #endregion

            var allHrUsersFullName = allHrUsers.Select(a => a.FirstName.ToLower() + a.MiddleName?.ToLower() ?? "" + a.LastName.ToLower()).ToList();

            var newUserName = NewHrData.FirstName.Replace(" ", "").ToLower() + MiddleName.Replace(" ", "").ToLower() + NewHrData.LastName.Replace(" ", "").ToLower();

            #region not repeating
            var FulLNameHrUserFromDB = HrUser.FirstName.Replace(" ", "") + HrUser.MiddleName.Replace(" ", "") + HrUser.LastName.Replace(" ", "");
            if (FulLNameHrUserFromDB.ToLower() != newUserName)
            {
                if (allHrUsersFullName.Contains(newUserName.ToLower()))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "This FullName is already Exists please, Enter a valid FullName :";
                    response.Errors.Add(err);
                    return response;
                }
            }

            if (HrUser.Email != NewHrData.Email)
            {
                var allHrUsersEmails = allHrUsers.Select(a => a.Email);
                if (allHrUsersEmails.Contains(NewHrData.Email) || allHrUsersEmails.Contains(NewHrData.Email.ToLower()))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "This Email is already Exists please, Enter a valid Email :";
                    response.Errors.Add(err);
                    return response;
                }
            }

            if (HrUser.LandLine != NewHrData.LandLine && NewHrData.LandLine != null)
            {
                var allHrUsersLandLines = allHrUsers.Where(a => a.LandLine != null).Select(a => a.LandLine);
                if (allHrUsersLandLines.Contains(NewHrData.LandLine))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "This Home is already Exists please, Enter a valid Home Number :";
                    response.Errors.Add(err);
                    return response;
                }
            }

            if (HrUser.Mobile != NewHrData.Mobile && NewHrData.Mobile != "0")
            {
                var allHrUsersMobileNumbers = allHrUsers.Select(a => a.Mobile);
                if (allHrUsersMobileNumbers.Contains(NewHrData.Mobile))
                {
                    response.Result = false;
                    Error err = new Error();
                    err.ErrorCode = "E-1";
                    err.errorMSG = "This Mobile is already Exists please, Enter a valid Mobile :";
                    response.Errors.Add(err);
                    return response;
                }
            }

            #endregion


            try
            {
                if (response.Result)
                {

                    IFormFile ImgInMemory = null;
                    var savedpath = "";
                    if (NewHrData.Photo != null)
                    {
                        ImgInMemory = NewHrData.Photo;
                    }

                    if (HrUser != null)
                    {
                        #region validation 
                        if (NewHrData.FirstName == null || NewHrData.FirstName == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "please, Enter a valid FrstName :";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (NewHrData.ARLastName == null || NewHrData.ARLastName == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "please, Enter a valid ArlastName :";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (NewHrData.LastName == null || NewHrData.LastName == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "please, Enter a valid lastName :";
                            response.Errors.Add(err);
                            return response;
                        }

                        //if (NewHrData.Mobile == null || NewHrData.Mobile == "")
                        //{
                        //    response.Result = false;
                        //    Error err = new Error();
                        //    err.ErrorCode = "E-1";
                        //    err.errorMSG = "please, Enter a valid Mobile number :";
                        //    response.Errors.Add(err);
                        //    return response;
                        //}
                        if (NewHrData.Email == null || NewHrData.Email == "")
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "please, Enter a valid Email:";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (string.IsNullOrWhiteSpace(NewHrData.Mobile))
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "please, Enter a valid Mobile:";
                            response.Errors.Add(err);
                            return response;
                        }
                        //if (NewHrData.Gender == null || NewHrData.Gender == "")
                        //{
                        //    response.Result = false;
                        //    Error err = new Error();
                        //    err.ErrorCode = "E-1";
                        //    err.errorMSG = "please, Enter a valid Gender :";
                        //    response.Errors.Add(err);
                        //    return response;
                        //}
                        if (NewHrData.IsUser == true)
                        {
                            if (string.IsNullOrWhiteSpace(NewHrData.systemEmail))
                            {
                                response.Result = false;
                                Error err = new Error();
                                err.ErrorCode = "E-1";
                                err.errorMSG = "please, Enter a valid user Email :";
                                response.Errors.Add(err);
                                return response;
                            }
                            var userOldData = _unitOfWork.Users.Find((a => a.Email == NewHrData.systemEmail));
                            if (userOldData != null && userOldData.Id != HrUser.UserId)
                            {
                                response.Result = false;
                                Error err = new Error();
                                err.ErrorCode = "E-1";
                                err.errorMSG = "The System Email is already Exists, please Enter a valid system Email:";
                                response.Errors.Add(err);
                                return response;
                            }
                            if (string.IsNullOrWhiteSpace(NewHrData.password) && HrUser.UserId is null)
                            {
                                response.Result = false;
                                Error err = new Error();
                                err.ErrorCode = "E-1";
                                err.errorMSG = "please, Enter a valid password :";
                                response.Errors.Add(err);
                                return response;
                            }
                            if (string.IsNullOrWhiteSpace(NewHrData.confirmPassword) && HrUser.UserId is null)
                            {
                                response.Result = false;
                                Error err = new Error();
                                err.ErrorCode = "E-1";
                                err.errorMSG = "please, Enter a valid Confirm password :";
                                response.Errors.Add(err);
                                return response;
                            }
                        }

                        #endregion

                        DateTime ForCheckOnly = new DateTime(1900, 01, 01);

                        HrUser.FirstName = NewHrData.FirstName;
                        if (NewHrData.Active != null)
                        {
                            HrUser.Active = NewHrData.Active ?? false;
                        }
                        HrUser.ModifiedById = userId;
                        HrUser.Modified = DateTime.Now;
                        HrUser.ArlastName = NewHrData.ARLastName;
                        HrUser.MaritalStatusId = NewHrData.MaritalStatusId;
                        HrUser.LastName = NewHrData.LastName;
                        HrUser.MiddleName = MiddleName;
                        //HrUser.IsUser = NewHrData
                        HrUser.Mobile = NewHrData.Mobile;
                        HrUser.Email = NewHrData.Email.ToLower();
                        HrUser.Gender = NewHrData.Gender;
                        //------------------------------CanBeNull-------------------
                        HrUser.ArfirstName = NewHrData.ARFirstName;
                        HrUser.ArmiddleName = NewHrData.ARMiddleName;
                        HrUser.BranchId = NewHrData.BranchID;
                        HrUser.DepartmentId = NewHrData.DepartmentID;
                        HrUser.TeamId = NewHrData.TeamId;
                        HrUser.JobTitleId = NewHrData.JobTitleID;
                        HrUser.IsUser = NewHrData.IsUser;
                        HrUser.LandLine = NewHrData.LandLine;
                        HrUser.IsDeleted = NewHrData.IsDeleted;
                        HrUser.Latitude =NewHrData.Latitude;
                        HrUser.Longtitud=NewHrData.Longtitud;
                        if (NewHrData.IsDeleted == true)
                        {
                            HrUser.Active = false;
                            if (HrUser.User != null)
                            {
                                HrUser.User.Active = false;
                            }
                        }
                        if (HrUser.DateOfBirth != DoB && DoB > ForCheckOnly)
                        {
                            if (NewHrData.DateOfBirth != null) HrUser.DateOfBirth = DoB;
                        }
                        if (NewHrData.Photo != null)
                        {
                            if (HrUser.ImgPath != null && HrUser.ImgPath != "")
                            {
                                string FilePath = Path.Combine(_host.WebRootPath, HrUser.ImgPath);
                                if (System.IO.File.Exists(FilePath))
                                {
                                    System.IO.File.Delete(FilePath);
                                }
                            }
                            //System.IO.File.Delete(HrUser.ImgPath);
                            var fileExtension = NewHrData.Photo.FileName.Split('.').Last();
                            var virtualPath = $"Attachments\\{CompanyName}\\HrUser\\{HrUser.Id}\\";
                            var FileName = System.IO.Path.GetFileNameWithoutExtension(NewHrData.Photo.FileName.Trim().Replace(" ", ""));
                            HrUser.ImgPath = Common.SaveFileIFF(virtualPath, NewHrData.Photo, FileName, fileExtension, _host);
                        }
                        if (NewHrData.TeamId != null)
                        {
                            var alreadyAtTisTeam = _unitOfWork.UserTeams.FindAll(a => a.HrUserId == HrUser.Id).FirstOrDefault();

                            if (HrUser.TeamId != null)
                            {
                                if (alreadyAtTisTeam == null)
                                {
                                    var newTeamUser = new UserTeam();
                                    newTeamUser.TeamId = NewHrData.TeamId ?? 0;
                                    newTeamUser.HrUserId = HrUser.Id;
                                    newTeamUser.CreatedBy = userId;
                                    newTeamUser.CreatedDate = DateTime.Now;
                                    newTeamUser.ModifiedBy = userId;
                                    newTeamUser.ModifiedDate = DateTime.Now;

                                    var teamUser = await _unitOfWork.UserTeams.AddAsync(newTeamUser);
                                }
                                else
                                {
                                    _unitOfWork.UserTeams.Delete(alreadyAtTisTeam);

                                    var newTeamUser = new UserTeam();
                                    newTeamUser.TeamId = NewHrData.TeamId ?? 0;
                                    newTeamUser.HrUserId = HrUser.Id;
                                    newTeamUser.CreatedBy = userId;
                                    newTeamUser.CreatedDate = DateTime.Now;
                                    newTeamUser.ModifiedBy = userId;
                                    newTeamUser.ModifiedDate = DateTime.Now;

                                    var teamUser = await _unitOfWork.UserTeams.AddAsync(newTeamUser);
                                }
                                _unitOfWork.Complete();
                            }
                        }

                        if (HrUser.User != null)
                        {

                            if (!string.IsNullOrWhiteSpace(NewHrData.systemEmail))
                            {

                                HrUser.User.Email = NewHrData.systemEmail;
                            }
                            if (!string.IsNullOrWhiteSpace(NewHrData.password) && !string.IsNullOrWhiteSpace(NewHrData.confirmPassword))
                            {

                                if (NewHrData.password == NewHrData.confirmPassword)
                                {
                                    HrUser.User.Password = Encrypt_Decrypt.Encrypt(NewHrData.password, key); ;
                                }

                            }
                            if (NewHrData.Active != null)
                            {
                                HrUser.User.Active = NewHrData.Active ?? false;
                                if (!HrUser.IsUser)
                                {
                                    HrUser.User.Active = false;
                                }
                            }
                            if (NewHrData.Photo != null)
                            {
                                HrUser.User.PhotoUrl = HrUser.ImgPath;
                            }
                            if (!string.IsNullOrWhiteSpace(NewHrData.FirstName))
                            {
                                HrUser.User.FirstName = NewHrData.FirstName;
                            }
                            if (!string.IsNullOrWhiteSpace(NewHrData.MiddleName))
                            {
                                HrUser.User.MiddleName = NewHrData.MiddleName;
                            }
                            if (!string.IsNullOrWhiteSpace(NewHrData.LastName))
                            {
                                HrUser.User.LastName = NewHrData.LastName;
                            }
                            if (!string.IsNullOrWhiteSpace(NewHrData.Mobile))
                            {
                                HrUser.User.Mobile = NewHrData.Mobile;
                            }

                            if (!string.IsNullOrWhiteSpace(NewHrData.Gender))
                            {
                                HrUser.User.Gender = NewHrData.Gender;
                            }

                            HrUser.User.BranchId = NewHrData.BranchID;
                            HrUser.User.DepartmentId = NewHrData.DepartmentID;
                            HrUser.User.JobTitleId = NewHrData.JobTitleID;
                            HrUser.User.ModifiedBy = userId;
                            HrUser.User.Modified = DateTime.Now;

                            if (NewHrData.IsDeleted == true)
                            {
                                HrUser.User.Active = false;
                            }
                        }
                        else
                        {
                            if (NewHrData.IsUser == true)
                            {
                                if (NewHrData.systemEmail != null || NewHrData.systemEmail != "" || NewHrData.password != null || NewHrData.password != "")
                                {
                                    NewGaras.Infrastructure.Entities.User usr = new NewGaras.Infrastructure.Entities.User();

                                    #region add Data to user table
                                    usr.Password = Encrypt_Decrypt.Encrypt(NewHrData.password, key);
                                    usr.Email = NewHrData.systemEmail;
                                    usr.FirstName = HrUser.FirstName;
                                    usr.LastName = HrUser.LastName;
                                    usr.MiddleName = HrUser.MiddleName;
                                    usr.Mobile = HrUser.Mobile;
                                    usr.Active = HrUser.Active;
                                    usr.CreationDate = DateTime.Now;
                                    usr.CreatedBy = userId;
                                    usr.ModifiedBy = userId;
                                    usr.Modified = DateTime.Now;
                                    usr.Gender = HrUser.Gender;
                                    usr.BranchId = HrUser.BranchId;
                                    usr.DepartmentId = HrUser.DepartmentId;
                                    usr.JobTitleId = HrUser.JobTitleId;
                                    usr.PhotoUrl = HrUser.ImgPath;
                                    #endregion

                                    var User = _unitOfWork.Users.Add(usr);
                                    _unitOfWork.Complete();

                                    HrUser.UserId = User.Id;

                                    //Common.CreateNotification(validation.userID, "Welcome , " + HrUser.FirstName, "Thank you for joining our system", "#", true, usr.Id, 0, _Context);

                                    UserDataResponse.UserSystemId = User.Id;
                                    UserDataResponse.UserSystemName = HrUser.FirstName;
                                    UserDataResponse.UserSystemEmail = NewHrData.systemEmail;
                                }
                            }
                        }
                        _unitOfWork.Complete();

                    }
                }
                UserDataResponse.HRUserId = NewHrData.HrUserId;
                response.Data = UserDataResponse;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException?.Message : ex.Message);
                response.Errors.Add(err);
                return response;
            }
        }

        public BaseResponseWithData<List<GetHrTeamUsersDto>> GetHrTeamUsers(long TeamId)
        {
            var response = new BaseResponseWithData<List<GetHrTeamUsersDto>>();
            response.Result = true;
            response.Errors = new List<Error>();
            var users = _unitOfWork.HrUsers.FindAll(a => a.TeamId == TeamId);
            var teamUsers = _mapper.Map<List<GetHrTeamUsersDto>>(users);
            response.Data = teamUsers;
            return response;
        }

        public async Task<BaseResponseWithData<List<HrUserJobTitleDto>>> GetAllUsersWithJobTitle(int? JobTitleId = null)
        {
            var response = new BaseResponseWithData<List<HrUserJobTitleDto>>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                if (response.Result)
                {
                    var HrUsersWithJobTitleId = await _unitOfWork.HrUsers.FindAllAsync(a => a.JobTitleId != JobTitleId);
                    response.Data = HrUsersWithJobTitleId.Select(x => new HrUserJobTitleDto
                    {
                        Id = x.Id,
                        JobTitleId = x.JobTitleId

                    }).ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorCode = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        public async Task<BaseResponseWithData<List<HrUsersWithJobTitleNameImage>>> GetHrUsersWithJobTitleNameImage(int JobTitleId)
        {
            var response = new BaseResponseWithData<List<HrUsersWithJobTitleNameImage>>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                var HrUsersWithJobTitleId = await _unitOfWork.HrUsers.FindAllAsync(a => a.JobTitleId == JobTitleId);
                response.Data = HrUsersWithJobTitleId.Select(x => new HrUsersWithJobTitleNameImage
                {
                    Name = x.FirstName + " " + x.LastName,
                    ImgPath = x.ImgPath

                }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error err = new Error();
                err.ErrorCode = "E-1";
                err.errorCode = "Exception :" + ex.Message;
                response.Errors.Add(err);
                return response;
            }
        }

        public async Task<BaseResponseWithId<long>> RetriveDeletedUser(long id)
        {
            var response = new BaseResponseWithId<long>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            try
            {
                if (response.Result)
                {
                    var allUsers = await _unitOfWork.HrUsers.GetAllAsync();
                    //var InData = allUsers.Where(a => a.Id == id).FirstOrDefault();
                    var allUsersEmails = allUsers.Select(x => x.Email);
                    var allHrUsersFullName = allUsers.Where(a => a.Id != id).Select(a => a.FirstName.ToLower() + a.MiddleName.ToLower() + a.LastName.ToLower()).ToList();

                    var user = await _unitOfWork.HrUsers.FindAsync((HU => HU.Id == id), new[] { "Department", "JobTitle", "Branch" });
                    var newUserName = user.FirstName.Replace(" ", "") + user.MiddleName.Replace(" ", "") + user.LastName.Replace(" ", "");

                    if (user != null)
                    {
                        if (user.IsDeleted == false || user.IsDeleted == null)
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This User is already Not deleted";
                            response.Errors.Add(err);
                            return response;
                        }
                        #region not repeating
                        if (allHrUsersFullName.Contains(newUserName.ToLower()))
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This FullName is already Exists .";
                            response.Errors.Add(err);
                            return response;
                        }
                        var allHrUsersEmails = allUsers.Where(a => a.Id != id).Select(a => a.Email);
                        if (allHrUsersEmails.Contains(user.Email) || allHrUsersEmails.Contains(user.Email.ToLower()))
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This Email is already Exists.";
                            response.Errors.Add(err);
                            return response;
                        }
                        var allHrUsersLandLines = allUsers.Where(a => a.LandLine != null && a.Id != id).Select(a => a.LandLine);
                        if (allHrUsersLandLines.Contains(user.LandLine))
                        {
                            response.Result = false;
                            Error err = new Error();
                            err.ErrorCode = "E-1";
                            err.errorMSG = "This Home is already Exists.";
                            response.Errors.Add(err);
                            return response;
                        }
                        if (user.Mobile != "0")
                        {
                            var allHrUsersMobileNumbers = allUsers.Where(a => a.Id != id).Select(a => a.Mobile);
                            if (allHrUsersMobileNumbers.Contains(user.Mobile))
                            {
                                response.Result = false;
                                Error err = new Error();
                                err.ErrorCode = "E-1";
                                err.errorMSG = "This Mobile is already Exists .";
                                response.Errors.Add(err);
                                return response;
                            }

                        }
                        #endregion
                        user.IsDeleted = false;
                        _unitOfWork.Complete();


                        //Common.CreateNotification(validation.userID, "Welcome , " + HrUser.FirstName, "Thank you for joining our system", "#", true, usr.Id, 0, _Context);

                    }
                    else
                    {
                        response.Result = false;
                        Error err = new Error();
                        err.ErrorCode = "E-1";
                        err.errorMSG = "The ID of HrUser is Not Valid , no HrUser with this Id";
                        response.Errors.Add(err);
                        return response;
                    }
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

        public BaseResponseWithData<GetAbsenceHistoryModel> GetAbsenceHistoryForUser(GetAbsenceHistoryRequest request)
        {
            BaseResponseWithData<GetAbsenceHistoryModel> Response = new BaseResponseWithData<GetAbsenceHistoryModel>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            Response.Data = new GetAbsenceHistoryModel();

            if (request.HrUserId == 0)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err101";
                error.ErrorMSG = "HrUser Id Is Required";
                Response.Errors.Add(error);
                return Response;
            }
            var HrUser = _unitOfWork.HrUsers.GetById(request.HrUserId);
            if (HrUser == null)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err102";
                error.ErrorMSG = "Hr User is not found";
                Response.Errors.Add(error);
                return Response;
            }
            if (request.AbsenceTypeId == 0)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err103";
                error.ErrorMSG = "Absence Type Id Is Required";
                Response.Errors.Add(error);
                return Response;
            }
            var Absence = _unitOfWork.ContractLeaveSetting.GetById(request.AbsenceTypeId);
            if (Absence == null)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err104";
                error.ErrorMSG = "Absence Type is not found";
                Response.Errors.Add(error);
                return Response;
            }
            var Attendances = _unitOfWork.Attendances.FindAll(a => a.AbsenceTypeId == request.AbsenceTypeId && a.HrUserId == request.HrUserId && a.IsApprovedAbsence == true, includes: new[] { "AbsenceType", "HrUser", "ApprovedByUser" }).ToList();

            var Past = Attendances.Where(a => DateOnly.FromDateTime(DateTime.Now) > a.AttendanceDate).Select(a => new AbsenceHistory { Id = a.Id, Date = a.AttendanceDate, HrUserId = (long)a.HrUserId, AbsenceCause = a.AbsenceCause, AbsenceName = a.AbsenceType.HolidayName, HrUserName = a.HrUser.FirstName + " " + a.HrUser.LastName, ApprovedAbsenceById = (long)a.ApprovedByUserId, ApprovedAbsenceName = a.ApprovedByUser.FirstName + " " + a.ApprovedByUser.LastName, ApprovedCause = a.AbsenceRejectCause, ApprovedDate = a.ModificationDate }).ToList();

            var planned = Attendances.Where(a => DateOnly.FromDateTime(DateTime.Now) <= a.AttendanceDate).Select(a => new AbsenceHistory { Id = a.Id, Date = a.AttendanceDate, HrUserId = (long)a.HrUserId, AbsenceCause = a.AbsenceCause, AbsenceName = a.AbsenceType.HolidayName, HrUserName = a.HrUser.FirstName + " " + a.HrUser.LastName, ApprovedAbsenceById = (long)a.ApprovedByUserId, ApprovedAbsenceName = a.ApprovedByUser.FirstName + " " + a.ApprovedByUser.LastName, ApprovedCause = a.AbsenceRejectCause, ApprovedDate = a.ModificationDate }).ToList();

            Response.Data.PastAbsence = Past;
            Response.Data.PlannedAbsencee = planned;

            return Response;
        }


        public BaseResponseWithData<string> GetUsersReportExcell(bool? Active, int? DeptID, long? teamID, string CompName, bool? IsUser)
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();

            try
            {
                //var inventoryItemsViewData = _unitOfWork.VInventoryStoreItemMovements.FindAll(a => a.date)
                var HrUsersQueryable = _unitOfWork.HrUsers.FindAllQueryable(x => true, new[] { "Department", "Branch", "User", "MilitaryStatus", "MaritalStatus", "JobTitle", "Team" });

                var currentDate = DateTime.Now;
                if (Active != null)
                {
                    HrUsersQueryable = HrUsersQueryable.Where(x => x.Active == Active);
                }
                if (DeptID != null)
                {
                    HrUsersQueryable = HrUsersQueryable.Where(x => x.DepartmentId == DeptID);
                }
                if (teamID != null)
                {
                    HrUsersQueryable = HrUsersQueryable.Where(x => x.TeamId == teamID);
                }

                if (IsUser != null)
                {
                    HrUsersQueryable = HrUsersQueryable.Where(x => x.IsUser == IsUser);
                }


                var HrUsersList = HrUsersQueryable.ToList();

                var nationalities = _unitOfWork.Nationalities.GetAll();

                ExcelPackage excel = new ExcelPackage();
                var sheet = excel.Workbook.Worksheets.Add($"Users Report");

                for (int col = 1; col <= 6; col++) sheet.Column(col).Width = 25;
                sheet.DefaultRowHeight = 15;
                sheet.Cells[1, 1].Value = "FirstName";
                sheet.Cells[1, 2].Value = "MiddleName";
                sheet.Cells[1, 3].Value = "LastName";
                sheet.Cells[1, 4].Value = "Active";
                sheet.Cells[1, 5].Value = "CreationDate";
                sheet.Cells[1, 6].Value = "CreatedBy";
                sheet.Cells[1, 7].Value = "Branch Name";
                sheet.Cells[1, 8].Value = "Department Name";
                sheet.Cells[1, 9].Value = "JobTitle";
                sheet.Cells[1, 10].Value = "DateOfBirth";
                sheet.Cells[1, 11].Value = "LandLine";
                sheet.Cells[1, 12].Value = "NationalityId";
                sheet.Cells[1, 13].Value = "MaritalStatus";
                sheet.Cells[1, 14].Value = "MilitaryStatus";
                sheet.Cells[1, 15].Value = "Team Name";
                sheet.Cells[1, 16].Value = "IsUser";
                sheet.Cells[1, 17].Value = "Mobile";
                sheet.Cells[1, 18].Value = "Email";
                sheet.Cells[1, 19].Value = "Gender";
                //sheet.Cells[1, 20].Value = "IsDeleted";
                sheet.Cells[1, 20].Value = "Active 'User'";
                sheet.Cells[1, 21].Value = "User CreationDate";


                sheet.Row(1).Height = 20;
                sheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(1).Style.Font.Bold = true;
                sheet.Cells[1, 1, 1, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[1, 1, 1, 21].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                sheet.Cells[1, 1, 1, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 1, 1, 21].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                var rowCount = 2;
                foreach (var HrUser in HrUsersList)
                {
                    sheet.Cells[rowCount, 1].Value = HrUser.FirstName;
                    sheet.Cells[rowCount, 2].Value = HrUser.MiddleName != null ? HrUser.MiddleName : "";
                    sheet.Cells[rowCount, 3].Value = HrUser.LastName;
                    sheet.Cells[rowCount, 4].Value = HrUser.Active;
                    sheet.Cells[rowCount, 5].Value = HrUser.CreationDate.ToString("yyyy-MM-dd");
                    sheet.Cells[rowCount, 6].Value = HrUser.CreatedBy.FirstName + " " + HrUser.CreatedBy.LastName;
                    sheet.Cells[rowCount, 7].Value = HrUser.Branch != null ? HrUser.Branch.Name : "";
                    sheet.Cells[rowCount, 8].Value = HrUser.Department != null ? HrUser.Department.Name : "";
                    sheet.Cells[rowCount, 9].Value = HrUser.JobTitle != null ? HrUser.JobTitle.Name : "";
                    sheet.Cells[rowCount, 10].Value = HrUser.DateOfBirth != null ? ((DateTime)HrUser.DateOfBirth).ToString("yyyy-MM-dd") : "";
                    sheet.Cells[rowCount, 11].Value = HrUser.LandLine;
                    sheet.Cells[rowCount, 12].Value = nationalities.Where(a => a.Id == HrUser.NationalityId).FirstOrDefault() != null ? nationalities.Where(a => a.Id == HrUser.NationalityId).FirstOrDefault().Nationality1 : "";
                    sheet.Cells[rowCount, 13].Value = HrUser.MaritalStatus != null ? HrUser.MaritalStatus.Name : "";
                    sheet.Cells[rowCount, 14].Value = HrUser.MilitaryStatus != null ? HrUser.MilitaryStatus.Name : "";
                    sheet.Cells[rowCount, 15].Value = HrUser.Team != null ? HrUser.Team.Name : "";
                    sheet.Cells[rowCount, 16].Value = HrUser.IsUser;
                    sheet.Cells[rowCount, 17].Value = HrUser.Mobile;
                    sheet.Cells[rowCount, 18].Value = HrUser.Email;
                    sheet.Cells[rowCount, 19].Value = HrUser.Gender;
                    //sheet.Cells[rowCount, 20].Value = HrUser.IsDeleted;
                    sheet.Cells[rowCount, 20].Value = HrUser.User != null ? HrUser.User.Active : "";
                    sheet.Cells[rowCount, 21].Value = HrUser.User != null ? HrUser.User.CreationDate.ToString("yyyy-MM-dd") : "";


                    sheet.Cells[rowCount, 1, rowCount, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[rowCount, 1, rowCount, 21].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowCount++;
                }

                for (int i = 1; i < 23; i++)
                {
                    sheet.Column(i).AutoFit();
                }
                //----------------------------file saving------------------------------
                var path = $"Attachments\\{CompName}\\UsersReport";
                var savedPath = Path.Combine(_host.WebRootPath, path);
                if (File.Exists(savedPath))
                    File.Delete(savedPath);

                // Create excel file on physical disk  
                Directory.CreateDirectory(savedPath);
                //FileStream objFileStrm = File.Create(savedPath);
                //objFileStrm.Close();
                var date = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                var excelPath = savedPath + $"\\UsersReport_{date}.xlsx";
                excel.SaveAs(excelPath);
                // Write content to excel file  
                //File.WriteAllBytes(savedPath, excel.GetAsByteArray());
                //Close Excel package 
                excel.Dispose();
                var fullPath = Globals.baseURL + "\\" + path + $"\\UsersReport_{date}.xlsx";

                response.Data = fullPath;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                response.Errors.Add(error);
                return response;
            }
        }






       


      

        public ActionResult<SelectDDLResponse> GetAbsenceTypeList()
        {
            SelectDDLResponse Response = new SelectDDLResponse();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                var List = new List<SelectDDL>();

                if (Response.Result)
                {

                    var ListDB = _unitOfWork.ContractLeaveSetting.GetAll();
                    foreach (var item in ListDB)
                    {
                        var ItemCategoryrObj = new SelectDDL();
                        ItemCategoryrObj.ID = item.Id;
                        ItemCategoryrObj.Name = item.HolidayName;
                        List.Add(ItemCategoryrObj);
                    }


                    Response.DDLList = List;
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

        


      
        //public ActionResult<BaseResponseWithID> AddAttendanceData(AddEmployeesAttendanceRequest request, long userID)   // Not Used
        //{
        //    BaseResponseWithID Response = new BaseResponseWithID();
        //    Response.Result = true;
        //    Response.Errors = new List<Error>();
        //    try
        //    {
        //        if (Response.Result)
        //        {

        //            var ErrorsList = new List<Error>();

        //            var SuccessInsertCount = 0;
        //            var SuccessUpdateCount = 0;
        //            var FailedInsertCount = 0;
        //            var FailedUpdateCount = 0;
        //            var TotalRows = 0;
        //            if (request.AttendanceData != null && request.AttendanceData.Any())
        //            {
        //                var IDSEmployeeList = request.AttendanceData.Select(x => x.EmployeeId).ToList();
        //                var UserEmployeeList = _unitOfWork.Users.FindAll(x => IDSEmployeeList.Contains(x.Id)).ToList();
        //                var IDSDepartmentList = request.AttendanceData.Select(x => x.DepartmentId).ToList();
        //                var DepartmentList = _unitOfWork.Departments.FindAll(x => IDSDepartmentList.Contains(x.Id)).ToList();
        //                var Counter = 1;
        //                foreach (var item in request.AttendanceData)

        //                {
        //                    var EmployeeErrorsList = new List<Error>();

        //                    var AttendanceRecord = new AddAttendanceData();

        //                    var NoOfWorkingHours = 0;
        //                    var NoOfWorkingMins = 0;
        //                    var DelayHours = 0;
        //                    var DelayMins = 0;
        //                    var OverTimeHours = 0;
        //                    var OverTimeMins = 0;
        //                    var AbsenceTypeId = 0;

        //                    if (item.EmployeeId != 0)
        //                    {
        //                        // Check User is exist
        //                        if (UserEmployeeList.Where(x => x.Id == item.EmployeeId).FirstOrDefault() == null)
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "Employee ID is not exist! for Counter : " + Counter;
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }
        //                        AttendanceRecord.EmployeeId = item.EmployeeId;
        //                    }
        //                    else
        //                    {
        //                        var Error = new Error();
        //                        Error.ErrorMSG = "Employee ID is required! for Counter : " + Counter;
        //                        Error.ErrorCode = Counter.ToString();
        //                        EmployeeErrorsList.Add(Error);
        //                    }
        //                    if (item.DepartmentId != 0)
        //                    {
        //                        if (DepartmentList.Where(x => x.Id == item.DepartmentId).FirstOrDefault() == null)
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "Department ID is not exist! for Counter : " + Counter;
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }
        //                        AttendanceRecord.DepartmentId = item.DepartmentId;
        //                    }
        //                    else
        //                    {
        //                        var Error = new Error();
        //                        Error.ErrorMSG = "Employee ID is required! for Counter : " + Counter;
        //                        Error.ErrorCode = Counter.ToString();
        //                        EmployeeErrorsList.Add(Error);
        //                    }

        //                    DateTime AttendanceDateConverted = DateTime.Now;
        //                    if (!DateTime.TryParse(item.AttendanceDateSTR, out AttendanceDateConverted))
        //                    {
        //                        var Error = new Error();
        //                        Error.ErrorMSG = "Invalid Attendance Date for Counter : " + Counter;
        //                        Error.ErrorCode = Counter.ToString();
        //                        EmployeeErrorsList.Add(Error);
        //                    }

        //                    if (!string.IsNullOrWhiteSpace(item.AttendanceDateSTR))
        //                    {
        //                        try
        //                        {

        //                            DateTime DayDate = AttendanceDateConverted.Date;


        //                            if (AttendanceDateConverted.Date <= DateTime.Now.Date)
        //                            {
        //                                //error when IsCompleteEmplyeePayslip is NULL (need to be edited according to logic)
        //                                var IsCompleteEmplyeePayslip = _unitOfWork.AttendancePaySlips.Find(a => a.EmployeeUserId == AttendanceRecord.EmployeeId && a.PaySlipDate.Year == DayDate.Year && a.PaySlipDate.Month == DayDate.Month).IsCompleted;

        //                                if (!IsCompleteEmplyeePayslip)
        //                                {
        //                                    AttendanceRecord.AttendanceDate = AttendanceDateConverted;
        //                                }
        //                                else
        //                                {
        //                                    var Error = new Error();
        //                                    Error.ErrorMSG = "You Can't take Attendance for This Employee Because his Payslip for this Month is Completed!!";
        //                                    Error.ErrorCode = Counter.ToString();
        //                                    EmployeeErrorsList.Add(Error);
        //                                }

        //                            }
        //                            else
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "You Can't take Attendance for Upcoming Date!!";
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "Worng Date format!";
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var Error = new Error();
        //                        Error.ErrorMSG = "Attendance Date is required! for Counter : " + Counter;
        //                        Error.ErrorCode = Counter.ToString();
        //                        EmployeeErrorsList.Add(Error);
        //                    }

        //                    if (item.AbsenceTypeId != null)
        //                        AttendanceRecord.AbsenceTypeId = item.AbsenceTypeId; //AbsenceTypeId;

        //                    if (AttendanceRecord.AbsenceTypeId == null)
        //                    {
        //                        if (item.CheckInHour != null)
        //                        {
        //                            var CheckInHour = item.CheckInHour;
        //                            if (CheckInHour < 0 || CheckInHour > 24)
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "CheckIn Hour must be between 0 and 24! for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                            else
        //                            {
        //                                AttendanceRecord.CheckInHour = CheckInHour;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "CheckIn Hour is required! for Counter : " + Counter;
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }

        //                        if (item.CheckInMin != null)
        //                        {
        //                            var CheckInMin = item.CheckInMin;
        //                            if (CheckInMin < 0 || CheckInMin > 59)
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "CheckIn Min must be between 0 and 59! for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                            else
        //                            {
        //                                AttendanceRecord.CheckInMin = CheckInMin;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "CheckIn Min is required! for Counter : " + Counter;
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }

        //                        if (item.CheckOutHour != null)
        //                        {
        //                            var CheckOutHour = item.CheckOutHour;
        //                            if (CheckOutHour < 0 || CheckOutHour > 24)
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "CheckOut Hour must be between 0 and 24! for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                            if (CheckOutHour < AttendanceRecord.CheckInHour)
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "Check Out Hour must be Greater than Check In Hour (In 24 Hours Format)! for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                            else
        //                            {
        //                                AttendanceRecord.CheckOutHour = CheckOutHour;
        //                            }
        //                        }
        //                        //else
        //                        //{
        //                        //    var Error = new Error();
        //                        //    Error.ErrorMSG = "CheckOut Hour is required!";
        //                        //    Error.ErrorCode = Counter.ToString();
        //                        //    EmployeeErrorsList.Add(Error);
        //                        //}

        //                        if (item.CheckOutMin != null)
        //                        {
        //                            var CheckOutMin = item.CheckOutMin;
        //                            if (CheckOutMin < 0 || CheckOutMin > 59)
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "CheckOut Min must be between 0 and 59! for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                            }
        //                            else
        //                            {
        //                                AttendanceRecord.CheckOutMin = CheckOutMin;
        //                            }
        //                        }
        //                        //else
        //                        //{
        //                        //    var Error = new Error();
        //                        //    Error.ErrorMSG = "CheckOut Min is required!";
        //                        //    Error.ErrorCode = Counter.ToString();
        //                        //    EmployeeErrorsList.Add(Error);
        //                        //}
        //                        if (AttendanceRecord.CheckInHour != null && AttendanceRecord.CheckInMin != null && AttendanceRecord.CheckOutMin != null && AttendanceRecord.CheckOutHour != null)
        //                        {
        //                            var CheckInTime = new TimeSpan(AttendanceRecord.CheckInHour ?? 0, AttendanceRecord.CheckInMin ?? 0, 0);
        //                            var CheckOutTime = new TimeSpan(AttendanceRecord.CheckOutHour ?? 0, AttendanceRecord.CheckOutMin ?? 0, 0);
        //                            var WorkingTimePeriod = CheckOutTime - CheckInTime;
        //                            NoOfWorkingHours = WorkingTimePeriod.Hours;
        //                            NoOfWorkingMins = WorkingTimePeriod.Minutes;

        //                            var DayOfWeek = AttendanceRecord.AttendanceDate.DayOfWeek.ToString();
        //                            var DayIdDb = _unitOfWork.WeekDays.Find(a => a.Day == DayOfWeek && a.BranchId == ).Id;
        //                            if (DayIdDb != 0)
        //                            {
        //                                var WorkingDay = _unitOfWork.WorkingHours.Find(a => a.Day == DayIdDb);
        //                                if (WorkingDay != null)
        //                                {
        //                                    var IntervalFromTime = new TimeSpan(WorkingDay.IntervalFromHour, WorkingDay.IntervalFromMin, 0);
        //                                    var IntervalToTime = new TimeSpan(WorkingDay.IntervalToHour, WorkingDay.IntervalToMin, 0);

        //                                    if (CheckInTime > IntervalFromTime)
        //                                    {
        //                                        var DelayTime = CheckInTime - IntervalFromTime;
        //                                        DelayHours = DelayTime.Hours;
        //                                        DelayMins = DelayTime.Minutes;
        //                                    }
        //                                    if (CheckOutTime > IntervalToTime)
        //                                    {
        //                                        var OverTime = CheckOutTime - IntervalToTime;
        //                                        OverTimeHours = OverTime.Hours;
        //                                        OverTimeMins = OverTime.Minutes;
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        AttendanceRecord.AbsenceTypeId = null;
        //                    }
        //                    else
        //                    {
        //                        AttendanceRecord.CheckInHour = 0;
        //                        AttendanceRecord.CheckInMin = 0;
        //                        AttendanceRecord.CheckOutHour = 0;
        //                        AttendanceRecord.CheckOutMin = 0;

        //                        AbsenceTypeId = (int)item.AbsenceTypeId;

        //                        int? ExistAbsenceTypeId = _unitOfWork.Attendances.Find(a => a.EmployeeId == AttendanceRecord.EmployeeId && a.AttendanceDate == DateOnly.FromDateTime(AttendanceRecord.AttendanceDate.Date)).AbsenceTypeId;


        //                        if (!UpdateEmployeAbsence(AttendanceRecord.EmployeeId, AbsenceTypeId, ExistAbsenceTypeId))
        //                        {
        //                            var Error = new Error();
        //                            Error.ErrorMSG = "There is no Absence balance For This Employee! for Counter : " + Counter;
        //                            Error.ErrorCode = Counter.ToString();
        //                            EmployeeErrorsList.Add(Error);
        //                        }
        //                        else
        //                        {
        //                            AttendanceRecord.AbsenceTypeId = AbsenceTypeId;
        //                        }
        //                    }

        //                    if (EmployeeErrorsList.Count() > 0)
        //                    {
        //                        FailedInsertCount++;
        //                    }
        //                    else
        //                    {
        //                        var ExistEmpAttendance = _unitOfWork.Attendances.Find(a => a.EmployeeId == AttendanceRecord.EmployeeId && a.AttendanceDate == DateOnly.FromDateTime(AttendanceRecord.AttendanceDate.Date));

        //                        if (ExistEmpAttendance == null)
        //                        {
        //                            var attendance = new Attendance()
        //                            {
        //                                EmployeeId = AttendanceRecord.EmployeeId,
        //                                DepartmentId = item.DepartmentId,
        //                                TeamId = null,
        //                                AttendanceDate = DateOnly.FromDateTime(AttendanceRecord.AttendanceDate.Date),
        //                                CheckInHour = AttendanceRecord.CheckInHour,
        //                                CheckInMin = AttendanceRecord.CheckInMin,
        //                                CheckOutHour = AttendanceRecord.CheckOutHour,
        //                                CheckOutMin = AttendanceRecord.CheckOutMin,
        //                                NoHours = NoOfWorkingHours,
        //                                NoMin = NoOfWorkingMins,
        //                                DelayHours = DelayHours,
        //                                DelayMin = DelayMins,
        //                                OverTimeHour = OverTimeHours,
        //                                OverTimeMin = OverTimeMins,
        //                                AbsenceTypeId = AttendanceRecord.AbsenceTypeId,
        //                                IsApprovedAbsence = AttendanceRecord.AbsenceTypeId != null ? true : false,
        //                                ApprovedByUserId = null,
        //                                CreatedBy = userID,
        //                                CreationDate = DateOnly.FromDateTime(DateTime.Now),
        //                                ModifiedBy = userID,
        //                                ModificationDate = DateOnly.FromDateTime(DateTime.Now),
        //                                Active = true
        //                            };
        //                            /*ObjectParameter AttendanceId = new ObjectParameter("ID", typeof(long));
        //                            var AttendanceInsertion = _Context.proc_AttendanceInsert(AttendanceId,
        //                                AttendanceRecord.EmployeeId,
        //                                item.DepartmentId,
        //                                null,
        //                                AttendanceRecord.AttendanceDate,
        //                                AttendanceRecord.CheckInHour,
        //                                AttendanceRecord.CheckInMin,
        //                                AttendanceRecord.CheckOutHour,
        //                                AttendanceRecord.CheckOutMin,
        //                                NoOfWorkingHours,
        //                                NoOfWorkingMins,
        //                                DelayHours,
        //                                DelayMins,
        //                                OverTimeHours,
        //                                OverTimeMins,
        //                                AttendanceRecord.AbsenceTypeId,
        //                                AttendanceRecord.AbsenceTypeId != null ? true : false,
        //                                null,
        //                                validation.userID, //ExistEmpAttendance.CreatedBy,
        //                                DateTime.Now,
        //                                validation.userID,
        //                                DateTime.Now,
        //                                true
        //                            );*/
        //                            var AttendanceInsertion = _unitOfWork.Attendances.Add(attendance);
        //                            _unitOfWork.Complete();
        //                            if (AttendanceInsertion != null)
        //                            {
        //                                UpdateEmployeeAttendencePayslip(attendance.Id, null, userID);
        //                                SuccessInsertCount++;
        //                            }
        //                            else
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "Error in insertion this record for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                                FailedInsertCount++;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var OldRecord = new BeforeUpdatRecordEmployeeHours()
        //                            {
        //                                AbsenceTypeId = ExistEmpAttendance.AbsenceTypeId,
        //                                CheckInHour = ExistEmpAttendance.CheckInHour ?? 0,
        //                                CheckInMin = ExistEmpAttendance.CheckInMin ?? 0,
        //                                CheckOutHour = ExistEmpAttendance.CheckOutHour ?? 0,
        //                                CheckOutMin = ExistEmpAttendance.CheckOutMin ?? 0,
        //                                DelayHours = ExistEmpAttendance.DelayHours ?? 0,
        //                                DeleyMin = ExistEmpAttendance.DelayMin ?? 0,
        //                                OverTimeHours = ExistEmpAttendance.OverTimeHour ?? 0,
        //                                OverTimeMin = ExistEmpAttendance.OverTimeMin ?? 0
        //                            };
        //                            ExistEmpAttendance = _unitOfWork.Attendances.GetById(ExistEmpAttendance.Id);
        //                            if (ExistEmpAttendance != null)
        //                            {
        //                                ExistEmpAttendance.CheckInHour = AttendanceRecord.CheckInHour;
        //                                ExistEmpAttendance.CheckInMin = AttendanceRecord.CheckInMin;
        //                                ExistEmpAttendance.CheckOutHour = AttendanceRecord.CheckOutHour;
        //                                ExistEmpAttendance.CheckOutMin = AttendanceRecord.CheckOutMin;
        //                                ExistEmpAttendance.NoHours = NoOfWorkingHours;
        //                                ExistEmpAttendance.NoMin = NoOfWorkingMins;
        //                                ExistEmpAttendance.AbsenceTypeId = AttendanceRecord.AbsenceTypeId;
        //                                ExistEmpAttendance.IsApprovedAbsence = true;
        //                                ExistEmpAttendance.ApprovedByUserId = null;
        //                                ExistEmpAttendance.ModificationDate = DateOnly.FromDateTime(DateTime.Now);
        //                                ExistEmpAttendance.ModifiedBy = userID;
        //                                ExistEmpAttendance.Active = true;


        //                            }
        //                            var AttendanceUpdate = _unitOfWork.Complete();
        //                            /*var AttendanceUpdate = _Context.proc_AttendanceUpdate(ExistEmpAttendance.Id,
        //                                                                                    ExistEmpAttendance.EmployeeId,
        //                                                                                    ExistEmpAttendance.DepartmentId,
        //                                                                                    null,
        //                                                                                    ExistEmpAttendance.AttendanceDate,
        //                                                                                    AttendanceRecord.CheckInHour,
        //                                                                                    AttendanceRecord.CheckInMin,
        //                                                                                    AttendanceRecord.CheckOutHour,
        //                                                                                    AttendanceRecord.CheckOutMin,
        //                                                                                    NoOfWorkingHours,
        //                                                                                    NoOfWorkingMins,
        //                                                                                    DelayHours,
        //                                                                                    DelayMins,
        //                                                                                    OverTimeHours,
        //                                                                                    OverTimeMins,
        //                                                                                    AttendanceRecord.AbsenceTypeId,
        //                                                                                    true,
        //                                                                                    null,
        //                                                                                    ExistEmpAttendance.CreatedBy,
        //                                                                                    DateTime.Now,
        //                                                                                    validation.userID,
        //                                                                                    DateTime.Now,
        //                                                                                    true
        //                                                                                );*/

        //                            if (AttendanceUpdate > 0)
        //                            {
        //                                UpdateEmployeeAttendencePayslip(ExistEmpAttendance.Id, OldRecord, userID);
        //                                SuccessUpdateCount++;
        //                            }
        //                            else
        //                            {
        //                                var Error = new Error();
        //                                Error.ErrorMSG = "Error in Update this record for Counter : " + Counter;
        //                                Error.ErrorCode = Counter.ToString();
        //                                EmployeeErrorsList.Add(Error);
        //                                FailedUpdateCount++;
        //                            }
        //                        }
        //                    }
        //                    ErrorsList.AddRange(EmployeeErrorsList);
        //                    Counter++;
        //                }



        //            }
        //            else
        //            {
        //                var Error = new Error();
        //                Error.ErrorMSG = "Null Request, Failed to upload Employees Attendance";
        //                ErrorsList.Add(Error);
        //            }

        //            StringBuilder sb = new StringBuilder();
        //            sb.AppendLine("Success Inserted Rows: " + SuccessInsertCount);
        //            sb.AppendLine("Failed Inserted Rows: " + FailedInsertCount);
        //            sb.AppendLine("Success Updated Rows: " + SuccessUpdateCount);
        //            sb.AppendLine("Failed Updated Rows: " + FailedUpdateCount);
        //            sb.AppendLine("Total Rows: " + TotalRows + "\n");

        //            if (ErrorsList.Count() > 0)
        //            {
        //                var RowNumber = int.Parse(ErrorsList.Select(a => a.ErrorCode).FirstOrDefault());
        //                foreach (Error e in ErrorsList)
        //                {
        //                    if (e.ErrorCode != RowNumber.ToString())
        //                    {
        //                        RowNumber++;
        //                        sb.AppendLine("\n");
        //                    }
        //                    sb.AppendLine("Number of row: " + e.ErrorCode + ", " + "Error message: " + e.ErrorMSG);
        //                    Response.Errors.Add(e);
        //                }
        //            }
        //        }

        //        return Response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Result = false;
        //        Error error = new Error();
        //        error.ErrorCode = "Err10";
        //        error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message; ;
        //        Response.Errors.Add(error);
        //        return Response;
        //    }
        //}


        public bool UpdateEmployeAbsence(long EmployeeID, int AbsenceTypeID, int? ExistAbsenceType)
        {
            bool Sucessed = false;
            if (EmployeeID != 0 && AbsenceTypeID != 0)
            {
                if (ExistAbsenceType == null) // Insert First Time
                {
                    Sucessed = DeductEmployeAbsence(EmployeeID, AbsenceTypeID, true);
                }
                else // update 
                {
                    if (ExistAbsenceType != 0) // Change from absence  To another  Absence 
                    {
                        // check if have balance on AbsenceTypeID and decrease balance .... then increase balance from old absnece again
                        bool CeckHaveBalanceOnNewAbsnce = DeductEmployeAbsence(EmployeeID, AbsenceTypeID, true);
                        if (CeckHaveBalanceOnNewAbsnce)
                        {
                            Sucessed = DeductEmployeAbsence(EmployeeID, (int)ExistAbsenceType, false);
                        }
                    }
                    else if (ExistAbsenceType == 0)
                    {
                        // employee take this day attend then update to absence .... check if have balance
                        Sucessed = DeductEmployeAbsence(EmployeeID, AbsenceTypeID, true);
                    }
                }


            }

            return Sucessed;
        }


        public bool DeductEmployeAbsence(long EmployeeID, int AbsenceTypeID, bool IsDeduct)
        {
            bool Sucessed = false;
            if (EmployeeID != 0 && AbsenceTypeID != 0)
            {
                var EmployeeContractLeaveDB = _unitOfWork.ContractLeaveEmployees.Find(x => x.UserId == EmployeeID && x.ContractLeaveSettingId == AbsenceTypeID && x.LeaveAllowed == "Yes");
                if (EmployeeContractLeaveDB != null)
                {
                    if (IsDeduct)
                    {
                        if (EmployeeContractLeaveDB.Remain > 0)
                        {
                            EmployeeContractLeaveDB.Used = EmployeeContractLeaveDB.Used + 1;
                            EmployeeContractLeaveDB.Remain = EmployeeContractLeaveDB.Remain - 1;
                            _unitOfWork.Complete();
                            Sucessed = true;
                        }
                    }
                    else
                    {
                        if (EmployeeContractLeaveDB.Used > 0)
                        {
                            EmployeeContractLeaveDB.Used = EmployeeContractLeaveDB.Used - 1;
                            EmployeeContractLeaveDB.Remain = EmployeeContractLeaveDB.Remain + 1;
                            _unitOfWork.Complete();
                            Sucessed = true;
                        }
                    }

                }
            }

            return Sucessed;
        }




        public bool CheckISAllowOverTimeAutomatic(long EmployeeID)
        {
            bool ISAllowOverTimeAutomatic = false;
            var ContractDetailOBJDB = _unitOfWork.Contracts.Find(x => x.UserId == EmployeeID && x.IsAllowOverTime && x.IsCurrent && x.Isautomatic);

            if (ContractDetailOBJDB != null)
            {
                ISAllowOverTimeAutomatic = true;
            }

            return ISAllowOverTimeAutomatic;
        }


        public decimal CalculdateOffDayOverTime(int? CheckHourIn, int? CheckHourOut, int? CheckMinuteIn, int? CheckMinuteOut)
        {
            decimal OverTimeHour = 0;
            CheckHourIn = CheckHourIn != null ? (int)CheckHourIn : 0;
            CheckHourOut = CheckHourOut != null ? (int)CheckHourOut : 0;
            CheckMinuteIn = CheckMinuteIn != null ? (int)CheckMinuteIn : 0;
            CheckMinuteOut = CheckMinuteOut != null ? (int)CheckMinuteOut : 0;

            decimal CheckInTime = ((decimal)CheckHourIn * 60 + (decimal)CheckMinuteIn) / 60;
            decimal CheckOutTime = ((decimal)CheckHourOut * 60 + (decimal)CheckMinuteOut) / 60;
            if (CheckOutTime > CheckInTime)
            {
                OverTimeHour = CheckOutTime - CheckInTime;
            }

            return OverTimeHour;
        }

        public int CalcWorkingDaysPerMonth(DateTime AttendanceDate)
        {
            int WorkingDays = 0;
            int NOOfDaysPerMonth = DateTime.DaysInMonth(AttendanceDate.Year, AttendanceDate.Month);
            for (int Counter = 1; Counter <= NOOfDaysPerMonth; Counter++)
            {
                DateTime PayslipDatePerDay = new DateTime(AttendanceDate.Year, AttendanceDate.Month, AttendanceDate.Day);
                if (!CheckISAttendanceDateOFFDayOrWeekEnd(PayslipDatePerDay))
                {
                    WorkingDays++;
                }
            }
            return WorkingDays;
        }

        public bool CheckISAttendanceDateOFFDayOrWeekEnd(DateTime AttendanceDate)
        {
            bool ISAttendanceDateOFFDayOrWeekEnd = false;
            var OffDay = _unitOfWork.OffDays.Find(x => x.Day.Year == AttendanceDate.Year && x.Day.Month == AttendanceDate.Month
                                                                  && x.Day.Day == AttendanceDate.Day && x.Active == true && x.AllowWorking == true);

            var WeekEnd = _unitOfWork.WeekEnds.Find(x => x.Day.ToLower() == AttendanceDate.DayOfWeek.ToString().ToLower());

            if (OffDay != null || WeekEnd != null)
            {
                ISAttendanceDateOFFDayOrWeekEnd = true;
            }

            return ISAttendanceDateOFFDayOrWeekEnd;
        }




        public async Task<BaseResponseWithId<long>> CreateHrUserWorker(AddHrUserWorker Worker, long UserId) 
        {
            var response = new BaseResponseWithId<long>()
            {
                Result = true,
                Errors = new List<Error>()
            };

            try
            {
                if (response.Result)
                {
                    #region validation
                    if (string.IsNullOrEmpty(Worker.FirstName))
                    {
                        response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Please, enter first name";
                        response.Errors.Add(error);
                    }
                    if (string.IsNullOrEmpty(Worker.LastName))
                    {
                        response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err25";
                        error.ErrorMSG = "Please, enter last name";
                        response.Errors.Add(error);
                    }
                    #endregion

                    var newHrUser = new HrUser()
                    {
                        FirstName = Worker.FirstName,
                        LastName = Worker.LastName,
                        MiddleName = string.IsNullOrEmpty(Worker.MiddleName) == false ? Worker.MiddleName : string.Empty,
                        Mobile = "0",
                        Email = "@",
                        ArlastName = Worker.LastName,
                        Active = true,
                        CreatedById = UserId,
                        CreationDate = DateTime.Now,
                        ModifiedById = UserId,
                        Modified = DateTime.Now,
                        IsUser = false
                    };

                    var newHruser =  _unitOfWork.HrUsers.Add(newHrUser);
                    _unitOfWork.Complete();

                    response.ID = newHrUser.Id;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                response.Errors.Add(error);
                return response;
            }
        }

       
    }
}
