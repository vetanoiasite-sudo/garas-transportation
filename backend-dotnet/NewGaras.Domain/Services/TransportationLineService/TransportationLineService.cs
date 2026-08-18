using AutoMapper;
using Azure;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NewGaras.Domain.DTO.HrUser;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.Mail;
using NewGaras.Infrastructure.Models.TransportationLine;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using DataTable = System.Data.DataTable;
using Error = NewGarasAPI.Models.Common.Error;
using Extension = NewGaras.Domain.Helper.Extension;
using Path = System.IO.Path;


namespace NewGaras.Domain.Services.TransportationLineService
{
    public class TransportationLineService : ITransportationLineService
    {
        private readonly IWebHostEnvironment _host;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHrUserService HrUserService;
        private GarasTestContext _Context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private HearderVaidatorOutput validation;

        public TransportationLineService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment host, GarasTestContext context,
                                         Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, INotificationService notificationService, IMailService mailService, IHrUserService hrUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _host = host;
            _Context = context;
            _environment = environment;
            _notificationService = notificationService;
            _mailService = mailService;
            HrUserService = hrUserService;
        }

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


        public BaseResponseWithData<List<VBusProgram>> Bus_Program()
        {
            BaseResponseWithData<List<VBusProgram>> Response = new BaseResponseWithData<List<VBusProgram>>();
            // Response.Data = new TransportationRouteVM();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {


                var tst = _Context.VBusPrograms.FirstOrDefault();
                var test = _unitOfWork.VBusProgram.FindAllQueryable(x => x.EmployeeNumber > 0).FirstOrDefault();
                //   Response.Data = _unitOfWork.v_Bus_Programs.GetAsQueryable().ToList();
                Response.Data = _Context.VBusPrograms.Take(1).ToList();

                Response.Result = true;
                return Response;

            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException.Message + " " + ex.InnerException.InnerException.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }

        public BaseResponse AddDeduction(TransportationVehicleRouteDeduction dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.TransportationVehicleRouteId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter TransportationVehicleRouteId";
                    Response.Errors.Add(error);
                }
                var RouteDeduction = _unitOfWork.TransportationVehicleRouteDeductions.Add(new TransportationVehicleRouteDeduction
                {
                    SupplierId = dto.SupplierId,
                    TransportationVehicleRouteId = dto.TransportationVehicleRouteId,
                    Serial = dto.Serial,
                    DateOfDeduction = dto.DateOfDeduction,
                    DeductPerRound = dto.DeductPerRound,
                    Cause = dto.Cause,
                    CreationDate = DateTime.Now,
                    CreationBy = validation.userID,
                    TypeOfDedct = dto.TypeOfDedct
                });

                var routeAccount = _unitOfWork.TransportationVehicleRouteAccounts.FindAll
                                                                                           (x => x.TransportationVehicleRouteId == dto.TransportationVehicleRouteId &&
                                                                                                x.DateOfMonth.Month == dto.DateOfDeduction.Month).FirstOrDefault();


                if (routeAccount == null)
                {
                    _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                    {
                        TransportationVehicleRouteId = dto.TransportationVehicleRouteId,
                        SupplierId = dto.SupplierId ?? 0,
                        Serial = dto.Serial,
                        DeductPerRounds = dto.DeductPerRound,
                        CreationBy = validation.userID,
                        CreationDate = DateTime.Now,
                        ModifiedBy = validation.userID,
                        ModifiedDate = DateTime.Now,
                        DateOfMonth = dto.DateOfDeduction
                    });
                }
                else
                {
                    routeAccount.DeductPerRounds = routeAccount.DeductPerRounds == null ? dto.DeductPerRound : routeAccount.DeductPerRounds + dto.DeductPerRound;
                    _unitOfWork.TransportationVehicleRouteAccounts.Update(routeAccount);
                }

                _unitOfWork.Complete();
                Response.Result = true;
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

        // Direction
        public BaseResponse AddTransportationDirection(TransportationDirectionDto dto, long TransportationVehicleRouteId)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (TransportationVehicleRouteId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }

                var AllTransportationDirection = new List<TransportationVehicleRouteDirection>();
                foreach (var item in dto.Data)
                {

                    var transportationDirection = new TransportationVehicleRouteDirection
                    {
                        TransportationVehicleRouteId = TransportationVehicleRouteId,
                        RouteDirection = item.RouteDirection,
                        Active = item.Active,
                        Description = item.Description,
                        CreationBy = validation.userID,
                        CreationDate = DateTime.Now,
                        Latitude = item.Latitude,
                        Longtitud = item.Longtitud

                    };
                    AllTransportationDirection.Add(transportationDirection);


                }
                _unitOfWork.TransportationVehicleRouteDirections.AddRange(AllTransportationDirection);


                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse AddTransportationEmployee(TransportationEmployeeDto dto, long TransportationVehicleRouteId)

        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (TransportationVehicleRouteId <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }

                var AllTransportationEmployee = new List<TransportationVehicleRouteEmployee>();
                var RouteEmployeesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0, new[] { "HrUser", "TransportationVehicleRoute" });
                var routeDb = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == TransportationVehicleRouteId).FirstOrDefault();

                foreach (var item in dto.Data)
                {
                    var checkEmployeeFound = RouteEmployeesDB.Where(x => x.HrUserId == item.hrUserId && x.TransportationVehicleRouteId == TransportationVehicleRouteId).FirstOrDefault();
                    if (checkEmployeeFound != null)
                    {

                        if (checkEmployeeFound.ToDate != null)
                        {
                            checkEmployeeFound.ToDate = item.ToDate;
                            checkEmployeeFound.Active = item.Active;
                            _unitOfWork.TransportationVehicleRouteEmployees.Update(checkEmployeeFound);

                        }
                        else
                        {
                            Error error = new Error();
                            error.ErrorCode = "Err10";
                            error.ErrorMSG = "Employee: " + checkEmployeeFound.HrUser.Email + "  route : " + routeDb.NameOfRoute + "مسجل بالفعل";
                            Response.Errors.Add(error);
                        }

                    }

                    else
                    {
                        if (RouteEmployeesDB.Where(x => x.HrUserId == item.hrUserId).Count() > 0)
                        {


                            foreach (var route in RouteEmployeesDB.Where(x => x.HrUserId == item.hrUserId))
                            {



                                if (
                                        (route.TransportationVehicleRoute.FromoDate.HasValue &&
                                         routeDb.FromoDate.HasValue &&
                                         route.TransportationVehicleRoute.FromoDate == routeDb.FromoDate)

                                        ||

                                        (route.TransportationVehicleRoute.ToDate.HasValue &&
                                         routeDb.ToDate.HasValue &&
                                         route.TransportationVehicleRoute.ToDate == routeDb.ToDate)
                                       )


                                {
                                    Error error = new Error();
                                    error.ErrorCode = "Err10";
                                    error.ErrorMSG = "Employee: " + route.HrUser.Email + "  route : " + routeDb.NameOfRoute + "يوجد  لديه خط في في نفس ميعادالحضور او الانصراف";
                                    Response.Errors.Add(error);
                                    break;
                                }
                                else
                                {
                                    var transportationEmployee = new TransportationVehicleRouteEmployee
                                    {
                                        TransportationVehicleRouteId = TransportationVehicleRouteId,
                                        HrUserId = item.hrUserId,
                                        Active = item.Active,
                                        CreationBy = validation.userID,
                                        CreationDate = DateTime.Now,
                                        TransportationVehicleRouteDirectionId = item.TransportationVehicleRouteDirectionId,
                                        FromDate = DateTime.Now,
                                        ToDate = item.ToDate,
                                        DurationLatitude = item.DurationLatitude,
                                        DurationLongtitud = item.DurationLongtitud,
                                        Period = item.Period,
                                    };
                                    AllTransportationEmployee.Add(transportationEmployee);
                                }

                            }
                        }
                        else
                        {
                            var transportationEmployee = new TransportationVehicleRouteEmployee
                            {
                                TransportationVehicleRouteId = TransportationVehicleRouteId,
                                HrUserId = item.hrUserId,
                                Active = item.Active,
                                CreationBy = validation.userID,
                                CreationDate = DateTime.Now,
                                TransportationVehicleRouteDirectionId = item.TransportationVehicleRouteDirectionId,
                                FromDate = DateTime.Now,
                                ToDate = item.ToDate,
                                DurationLatitude = item.DurationLatitude,
                                DurationLongtitud = item.DurationLongtitud,
                                Period = item.Period,
                            };
                            AllTransportationEmployee.Add(transportationEmployee);
                        }


                    }

                }
                if (AllTransportationEmployee.Count > 0)
                {
                    _unitOfWork.TransportationVehicleRouteEmployees.AddRange(AllTransportationEmployee);
                }


                _unitOfWork.Complete();
                Response.Result = true;
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


        public BaseResponse AddRoutesForEmployee(AddRoutesForHrUserDto dto, long HrUserId)

        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (HrUserId <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter HrUser";
                    Response.Errors.Add(error);
                }

                var AllTransportationEmployee = new List<TransportationVehicleRouteEmployee>();
                var RouteEmployeesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);

                foreach (var item in dto.Data)
                {
                    var checkEmployeeFound = RouteEmployeesDB.Where(x => x.HrUserId == HrUserId && x.TransportationVehicleRouteId == item.RouteId).FirstOrDefault();
                    if (checkEmployeeFound != null)
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err10";
                        error.ErrorMSG = $" HrUserId = {HrUserId} and  TransportationVehicleRouteId  = {item.RouteId} adding before ...remove it and Update";
                        Response.Errors.Add(error);

                        //if (checkEmployeeFound.ToDate == null)
                        //{
                        //    checkEmployeeFound.ToDate = DateTime.Now;
                        //    checkEmployeeFound.Active = false;
                        //}
                        //_unitOfWork.TransportationVehicleRouteEmployees.Update(checkEmployeeFound);

                    }

                    var transportationEmployee = new TransportationVehicleRouteEmployee
                    {
                        TransportationVehicleRouteId = item.RouteId,
                        HrUserId = HrUserId,
                        Active = item.Active,
                        CreationBy = validation.userID,
                        CreationDate = DateTime.Now,
                        TransportationVehicleRouteDirectionId = item.TransportationVehicleRouteDirectionId,
                        FromDate = DateTime.Now,
                        ToDate = item.ToDate,
                        DurationLatitude = item.DurationLatitude,
                        DurationLongtitud = item.DurationLongtitud,
                        Period = item.Period,
                    };
                    AllTransportationEmployee.Add(transportationEmployee);


                }
                _unitOfWork.TransportationVehicleRouteEmployees.AddRange(AllTransportationEmployee);


                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponseWithData<TransportationRouteDetailsVM> getTransportationRouteDetails(long RouteId, DateTime FromDate, DateTime ToDate)
        {
            BaseResponseWithData<TransportationRouteDetailsVM> Response = new BaseResponseWithData<TransportationRouteDetailsVM>();
            Response.Data = new TransportationRouteDetailsVM();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;
                var supplierDb = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
                var supplierContactPersionDb = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0).AsQueryable();
                var DirectionDb = _unitOfWork.TransportationVehicleRouteDirections.FindAllQueryable(x => x.Id > 0);

                var christianID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "C").Select(c => c.Id).FirstOrDefault();
                var MuslimID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "M").Select(c => c.Id).FirstOrDefault();
                var RoutesEmployesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                var RouteDB = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id == RouteId && x.Active == true, new[] { "TransportationVehicle" }).FirstOrDefault();
                var usersInRoute = _unitOfWork.TransportationVehicleRouteEmployees.Count(c => c.TransportationVehicleRouteId == RouteId);
                var UserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Serial == RouteDB.Serial && (((DateTime)x.CheckIn).Date >= FromDate.Date && ((DateTime)x.CheckIn).Date <= ToDate.Date) ||
                                                              ((DateTime)x.CheckOut).Date >= FromDate.Date && ((DateTime)x.CheckOut).Date <= ToDate.Date).ToList();
                Response.Data = new TransportationRouteDetailsVM
                {

                    Id = RouteDB.Id,
                    LineName = RouteDB.TransportationLine?.LineName ?? "N/A",
                    LineCost = RouteDB.LineCost,
                    LineId = RouteDB.TransportationLineId,

                    SupplierName = supplierDb.Where(x => x.Id == RouteDB.SupplierId).Select(y => y.Name).FirstOrDefault() ?? "No Supplier",
                    SupplierId = RouteDB.SupplierId ?? 0,

                    SupplierContactPersonName = supplierContactPersionDb.Where(x => x.Id == RouteDB.SupplierContactPersonId).Select(y => y.Name).FirstOrDefault() ?? "N/A",
                    SupplierContactPersonId = RouteDB.SupplierContactPersonId ?? 0,

                    branchScheduleId = RouteDB.BranchScheduleId ?? 0,
                    ShiftNumber = RouteDB.BranchSchedule?.ShiftNumber ?? 0,

                    Serial = RouteDB.Serial ?? "No Serial",

                    PeriodFrom = RouteDB.PeriodFrom,
                    PeriodTo = RouteDB.PeriodTo,

                    BusSupervisor = RouteDB.Supervisor != null
                        ? $"{RouteDB.Supervisor.FirstName} {RouteDB.Supervisor.MiddleName} {RouteDB.Supervisor.LastName}".Trim()
                        : "No Supervisor",

                    BuSupervisorId = RouteDB.SupervisorId ?? 0,

                    FullCapacity = RouteDB.TransportationVehicle?.Capacity ?? 0,

                    //UserInRoute = usersInRoute?.Count ?? 0,
                    //ExpectionNumFromOtherLines = exceptionNumFromOtherLines?.Count ?? 0,
                    //RouteEmployeesToOtherLines = routeEmployeesToOtherLines?.Count ?? 0,

                    ActualCapacity = usersInRoute,
                    Attendace = UserAttedance.Count(),
                    absence = usersInRoute - UserAttedance.Count(),
                    DirectionNum = DirectionDb.Where(x => x.TransportationVehicleRouteId == RouteDB.Id).Count(),
                    FromoDate = RouteDB.FromoDate,
                    ToDate = RouteDB.ToDate,
                    christianNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == RouteDB.Id && x.HrUser.MaritalStatusId == christianID).Count(),
                    MuslimNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == RouteDB.Id && x.HrUser.MaritalStatusId == MuslimID).Count(),
                };




                Response.Result = true;
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

        public BaseResponse AddTransportationLine(TransportationLineVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {

                _unitOfWork.TransportationLines.Add(new TransportationLine
                {
                    LineName = dto.LineName,

                    CreationBy = validation.userID,
                    CreationDate = DateTime.Now,
                    ModifiedBy = validation.userID,
                    ModifiedDate = DateTime.Now,
                    // IsApproved = false
                });
                _unitOfWork.Complete();
                Response.Result = true;
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

        public async Task<BaseResponse> AddTransportationRoute(TransportationVehicleRouteVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.TransportationLineId <= 0 && string.IsNullOrEmpty(dto.TransportationLineName))
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var lastValueSerial = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id > 0, new[] { "TransportationLine" }).Select(s => s.Serial).LastOrDefault() ?? "0";
                if (lastValueSerial == "0")
                {
                    lastValueSerial = "70000000";
                }
                var NewTransportationLine = new TransportationLine();
                if (!string.IsNullOrEmpty(dto.TransportationLineName))
                {
                    NewTransportationLine = _unitOfWork.TransportationLines.Add(new TransportationLine
                    {
                        LineName = dto.TransportationLineName,
                        CreationBy = validation.userID,
                        CreationDate = DateTime.Now,
                        ModifiedBy = validation.userID,
                        ModifiedDate = DateTime.Now,
                    });
                    _unitOfWork.Complete();
                }



                _unitOfWork.TransportationVehicleRoutes.Add(new TransportationVehicleRoute
                {
                    TransportationLineId = NewTransportationLine.Id > 0 ? NewTransportationLine.Id : dto.TransportationLineId,
                    TransportationVehicleId = dto.TransportationVehicleId,
                    Active = dto.Active,
                    SupplierId = dto.SupplierId,
                    SupplierContactPersonId = dto.SupplierContactPersonId,
                    BranchScheduleId = dto.BranchScheduleId,
                    CreationBy = validation.userID,
                    CreationDate = DateTime.Now,
                    ModifiedBy = validation.userID,
                    ModifiedDate = DateTime.Now,
                    Serial = ((long.Parse(lastValueSerial)) + 1).ToString(),
                    PeriodFrom = dto.PeriodFrom != null ? dto.PeriodFrom : DateTime.Now,
                    PeriodTo = dto.PeriodTo,
                    SupervisorId = dto.HrUserId,
                    FromoDate = dto.FromoDate,
                    ToDate = dto.ToDate,
                    NameOfRoute = dto.NameOfRoute,
                    LineCost = dto.LineCost,
                    OneWay = dto.OneWay
                });
                var resuilt = _unitOfWork.Complete();
                if (resuilt > 0)
                {
                    var RoleId = _unitOfWork.Roles.FindAll(x => x.Name == "Transportation Super Admin").Select(r => r.Id).FirstOrDefault();
                    var UserRoles = _unitOfWork.UserRoles.FindAllQueryable(x => x.RoleId == RoleId, new[] { "User" }).Select(u => u.User);
                    foreach (var item in UserRoles)
                    {
                        _notificationService.CreateNotification(validation.userID, "اضافة خط جديد", "Need Approval", "#", true, item.Id, 0);
                        await _mailService.SendMail(new MailData() { EmailToName = item.Email, EmailToId = item.Email, EmailSubject = "اضافة خط جديد", EmailBody = "Need Approval" });

                    }

                }
                Response.Result = true;
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
        public BaseResponse AddTransportationVehicle(TransportationVehicleVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.VehicleTypeId <= 0 || dto.Capacity <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                _unitOfWork.TransportationVehicles.Add(new TransportationVehicle
                {
                    VehicleTypeId = dto.VehicleTypeId,
                    Active = dto.Active,
                    Capacity = dto.Capacity,
                    CreationBy = validation.userID,
                    CreationDate = DateTime.Now,
                    ModifiedBy = validation.userID,
                    ModifiedDate = DateTime.Now,
                    IsApproved = false
                });
                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse AddUsersAttedance(TransprotationUserAttedanceVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                #region validation 
                if (string.IsNullOrEmpty(dto.Type) || string.IsNullOrEmpty(dto.Serial))
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameters";
                    Response.Errors.Add(error);
                }

                #endregion

                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var FullDateTime = egyptTime;

                if (dto.CheckInOrCheckOut == false)
                {
                    var userAttendanceDB = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Type == dto.Type && x.Serial == dto.Serial && x.CreationDate.Date == FullDateTime.Date).FirstOrDefault();
                    if (userAttendanceDB != null)
                    {
                        userAttendanceDB.CheckOut = FullDateTime;
                        userAttendanceDB.CheckOutLatitude = dto.CheckOutLatitude;
                        userAttendanceDB.CheckOutLongtitud = dto.CheckOutLongtitud;
                        userAttendanceDB.CheckOutRouteDirectionId = dto.CheckOutRouteDirectionId;
                        _unitOfWork.TransprotationUserAttedances.Update(userAttendanceDB);
                    }
                    else
                    {
                        var userAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                        {
                            Type = dto.Type,
                            Serial = dto.Serial,
                            CheckOut = FullDateTime,
                            CreationDate = FullDateTime,
                            CreationBy = validation.userID,
                            CheckOutLatitude = dto.CheckOutLatitude,
                            CheckOutLongtitud = dto.CheckOutLongtitud,
                            CheckOutRouteDirectionId = dto.CheckOutRouteDirectionId
                        });
                    }
                }
                else
                {
                    var userAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                    {
                        Type = dto.Type,
                        Serial = dto.Serial,
                        CheckIn = FullDateTime,
                        CreationDate = FullDateTime,
                        CreationBy = validation.userID,
                        CheckInLatitude = dto.CheckInLatitude,
                        CheckInLongtitud = dto.CheckInLongtitud,
                        CheckInRouteDirectionId = dto.CheckInRouteDirectionId
                    });
                }

                if (dto.Type == "Bus")
                {
                    var transportationlineRoute = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Serial == dto.Serial && x.Active == true, new[] { "TransportationLine" }).FirstOrDefault();
                    var checkAccount = _unitOfWork.TransportationVehicleRouteAccounts.FindAll
                                                                                               (x => x.TransportationVehicleRouteId == transportationlineRoute.Id &&
                                                                                                    x.DateOfMonth.Month == FullDateTime.Month).FirstOrDefault();


                    if (checkAccount == null)
                    {
                        _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                        {
                            TransportationVehicleRouteId = transportationlineRoute.Id,
                            SupplierId = transportationlineRoute.SupplierId ?? 0,
                            Serial = dto.Serial,
                            DateOfMonth = DateTime.Now,
                            CountOfRounds = 1,
                            PriceOfRounds = (bool)transportationlineRoute.OneWay ? (transportationlineRoute.LineCost / 2) : transportationlineRoute.LineCost,
                            CreationBy = validation.userID,
                            CreationDate = DateTime.Now,

                        });
                    }
                    else
                    {
                        if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                        {
                            checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                        }
                        checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                        checkAccount.ModifiedBy = validation.userID;
                        checkAccount.ModifiedDate = FullDateTime;
                        _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                    }
                }


                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse AddVehicleType(VehicleTypeVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (string.IsNullOrEmpty(dto.Type))
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                _unitOfWork.VehicleTypes.Add(new VehicleType
                {
                    Type = dto.Type,
                    Active = dto.Active,
                });
                _unitOfWork.Complete();
                Response.Result = true;
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

        // Approve
        public BaseResponse ApproveRoute(long Id, bool Approve)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldRoute = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == Id).FirstOrDefault();
                if (oldRoute == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                oldRoute.ModifiedBy = validation.userID;
                oldRoute.Active = Approve;

                _unitOfWork.TransportationVehicleRoutes.Update(oldRoute);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse ApproveTransportationVehicle(int Id, bool Approve)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationVehicle = _unitOfWork.TransportationVehicles.FindAll(x => x.Id == Id).FirstOrDefault();
                if (oldTransportationVehicle == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }


                oldTransportationVehicle.ApprovedBy = validation.userID.ToString();
                oldTransportationVehicle.IsApproved = Approve;

                _unitOfWork.TransportationVehicles.Update(oldTransportationVehicle);

                _unitOfWork.Complete();
                Response.Result = true;
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


        public async Task<BaseResponseWithData<string>> AttendanceExcellDur(int Year, long RouteId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, bool? AttendaceFlag, long HrUser, int PageNo, int NoOfItems)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                // CLIENTS
                var client = _unitOfWork.Clients.FindAll(x => x.OwnerCoProfile == true).FirstOrDefault();
                if (client == null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "client not found";
                    Response.Errors.Add(error);
                }


                var clientPhone = _unitOfWork.ClientPhones.FindAll(x => x.ClientId == client.Id).Select(y => y.Phone).FirstOrDefault();
                var clientAddress = _unitOfWork.ClientAddresses.FindAll(x => x.ClientId == client.Id).FirstOrDefault();
                if (clientAddress == null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "clientAddress not found";
                    Response.Errors.Add(error);
                }
                var clientCountry = _unitOfWork.Countries.FindAll(x => x.Id == clientAddress.CountryId).Select(y => y.Name).FirstOrDefault() ?? "Egypt";
                var clientgoverment = _unitOfWork.Governorates.FindAll(x => x.Id == clientAddress.GovernorateId).Select(y => y.Name).FirstOrDefault() ?? "Alexandria";
                var createdUser = _unitOfWork.Users.FindAll(x => x.Id == client.CreatedBy).FirstOrDefault();
                // ACCOUNTS
                var accounts = _unitOfWork.Accounts.FindAll(x => x.ParentCategory != 0);
                if (accounts == null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "accounts not found";
                    Response.Errors.Add(error);
                }
                var accountsNames = accounts.Select(x => x.AccountName);

                int i = 1;
                var transportionLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0);


                if (SupplierId > 0)
                {
                    var transportionLineIds = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.SupplierId == SupplierId).Select(s => s.TransportationLineId).ToList();
                    transportionLineDb = transportionLineDb.Where(x => transportionLineIds.Contains(x.Id));
                }

                //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage excel = new ExcelPackage();
                foreach (var item in transportionLineDb)
                {
                    var hrUsers = DashBoardForAttendence(item.Id, RouteId, SupplierId, supplierContactPersonId, (DateTime)DateSerach, serialBus, FromDate, ToDate, AttendaceFlag, HrUser, PageNo, NoOfItems).Data;

                    //error because 12 months only
                    //    string monthName = new DateTime(2000, i, 1).ToString("MMMM", new CultureInfo("ar-EG"));



                    var sheet = excel.Workbook.Worksheets.Add($"{item.LineName} ");

                    sheet.Cells[5, 1].Value = $"تقرير السعة اليومي باسماء العاملين ";
                    sheet.Cells[5, 1, 5, 3].Merge = true;
                    sheet.Cells[5, 1, 5, 3].Style.Font.Bold = true;

                    sheet.Cells[5, 1, 5, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // Adjust column width if necessary


                    sheet.Cells[6, 3].Value = $"  {item.LineName} تقرير السعة اليومي لخط   ";
                    sheet.Cells[6, 2].Value = $"بتاريخ";


                    sheet.Cells[6, 1].Value = $" {DateSerach?.ToString("yyyy:MMMM:dd")} ";
                    //sheet.Cells[6, 1, 5, 3].Merge = true;
                    sheet.Cells[6, 3].Style.Font.Bold = true;
                    sheet.Cells[6, 2].Style.Font.Bold = true;
                    sheet.Cells[6, 1].Style.Font.Bold = true;

                    sheet.Cells[6, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells[1, 3].Value = $"{client.Name}";
                    //sheet.Cells[1, 7, 1, 10].Merge = true;
                    sheet.Cells[4, 3].Value = $" TEL : {clientPhone}";

                    //sheet.Cells[2, 7, 2, 10].Merge = true;

                    sheet.Cells[2, 3].Value = $"{clientCountry}, {clientgoverment}";
                    //sheet.Cells[3, 7, 3, 10].Merge = true;

                    sheet.Cells[3, 3].Value = $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.AreaId}";
                    // sheet.Cells[4, 7, 4, 10].Merge = true;

                    //sheet.Cells[1, 7, 3, 10].Merge = true;

                    //sheet.Cells[1, 7].Value = $"{client.Name} \n" +$"{clientPhone }\n" + $"{clientCountry}, {clientgoverment} " + $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.Area}";


                    sheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //sheet.Cells[1, 7, 3, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Optionally format the header cells
                    //sheet.Cells[1, 1, 5, 1].Style.Font.Bold = true; // Make the text bold
                    //sheet.Cells[1, 1, 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    // Adjust column width to fit the content
                    // Assuming a total width equivalent to a typical A4 page (approximately 100 units)
                    double totalPageWidth = 90;
                    double columnWidth = totalPageWidth / 3; // Divide equally into 3 columns

                    // Set the width for each column
                    sheet.Column(1).Width = columnWidth; // البيان
                    sheet.Column(2).Width = columnWidth; // دائن
                    sheet.Column(3).Width = columnWidth; // مدين


                    //sheet.HeaderFooter.OddHeader.RightAlignedText = client.Name;
                    //sheet.HeaderFooter.EvenHeader.RightAlignedText = client.Name;
                    sheet.HeaderFooter.EvenFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.EvenFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.EvenFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.OddFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.OddFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.OddFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.FirstFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.FirstFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.FirstFooter.RightAlignedText = $"{createdUser.FirstName}  {createdUser.LastName}";


                    sheet.PrinterSettings.Orientation = eOrientation.Landscape;



                    // Load the original image

                    //string imagePath3 = Path.Combine(_environment.WebRootPath, "Attachments\\garastest\\HrUser\\10024", "133567184185534210_Img.jpg");

                    //string imagePath3 = client.LogoUrl != null ? Globals.baseURL + "/" + client.LogoUrl : null;
                    //if (imagePath3 != null)
                    //{
                    //    string relativeImagePath = client.LogoUrl;
                    //    string imagePath4 = !string.IsNullOrEmpty(relativeImagePath)
                    //        ? Path.Combine(_environment.WebRootPath, relativeImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()))
                    //        : null;


                    //    Image originalImage = Image.FromFile(imagePath4);

                    //    // Define the target dimensions
                    //    int targetWidth = 64 * 4;  // مساحة عرض (عرض 3 أعمدة)
                    //    int targetHeight = 60; // مساحة ارتفاع (ارتفاع 3 صفوف)

                    //    // Calculate new dimensions while maintaining aspect ratio
                    //    float aspectRatio = (float)originalImage.Width / originalImage.Height;
                    //    int newWidth, newHeight;

                    //    if (targetWidth / (float)targetHeight > aspectRatio)
                    //    {
                    //        // Fit by height
                    //        newHeight = targetHeight;
                    //        newWidth = (int)(targetHeight * aspectRatio);
                    //    }
                    //    else
                    //    {
                    //        // Fit by width
                    //        newWidth = targetWidth;
                    //        newHeight = (int)(targetWidth / aspectRatio);
                    //    }

                    //    // Resize the image
                    //    Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

                    //    // Save the resized image to a temporary file
                    //    // string tempImagePath = Path.Combine(_environment.WebRootPath, "img\\scaled.jpg");
                    //    string tempImagePath = Path.Combine(_environment.WebRootPath, "img", "scaled.jpg");
                    //    string tempDirectory = Path.GetDirectoryName(tempImagePath);

                    //    // Ensure the directory exists
                    //    if (!Directory.Exists(tempDirectory))
                    //    {
                    //        Directory.CreateDirectory(tempDirectory);
                    //    }
                    //    if (i == 1)
                    //    {
                    //        if (File.Exists(tempImagePath))
                    //        {
                    //            File.Delete(tempImagePath);
                    //        }
                    //        resizedImage.Save(tempImagePath);
                    //    }
                    //    // Create a FileInfo object for the resized image
                    //    FileInfo resizedImageFile = new FileInfo(tempImagePath);
                    //    // Add the image to the Excel sheet
                    //    var picture = sheet.Drawings.AddPicture("MyPicture", resizedImageFile);

                    //    // Position the picture in the desired cell
                    //    picture.SetPosition(0, 0, 0, 0); // Top-left corner of Row 1, Column 1

                    //    // Set the calculated size
                    //    picture.SetSize(newWidth, newHeight);

                    //}


                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        // Build physical path from relative URL
                        string relativeImagePath = client.LogoUrl.Replace("/", Path.DirectorySeparatorChar.ToString());
                        string imagePath = Path.Combine(_environment.WebRootPath, relativeImagePath);

                        // Check if file exists
                        if (File.Exists(imagePath) && i == 1)
                        {

                            using (Image originalImage = Image.FromFile(imagePath))
                            {
                                // Define target dimensions
                                int targetWidth = 64 * 4;
                                int targetHeight = 60;

                                // Calculate aspect ratio
                                float aspectRatio = (float)originalImage.Width / originalImage.Height;
                                int newWidth, newHeight;

                                if (targetWidth / (float)targetHeight > aspectRatio)
                                {
                                    newHeight = targetHeight;
                                    newWidth = (int)(targetHeight * aspectRatio);
                                }
                                else
                                {
                                    newWidth = targetWidth;
                                    newHeight = (int)(targetWidth / aspectRatio);
                                }

                                // Resize the image
                                using (Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero))
                                {
                                    string tempImagePath = Path.Combine(_environment.WebRootPath, "img", "scaled.jpg");

                                    // Ensure directory exists
                                    Directory.CreateDirectory(Path.GetDirectoryName(tempImagePath));

                                    if (i == 1)
                                    {
                                        if (File.Exists(tempImagePath))
                                        {
                                            File.Delete(tempImagePath);
                                        }

                                        resizedImage.Save(tempImagePath);
                                    }

                                    FileInfo resizedImageFile = new FileInfo(tempImagePath);

                                    var picture = sheet.Drawings.AddPicture("MyPicture", resizedImageFile);
                                    picture.SetPosition(0, 0, 0, 0);
                                    picture.SetSize(newWidth, newHeight);
                                }
                            }

                        }
                    }




                    // Set column headers
                    // sheet.Cells [1 , 1].Value="No Row";
                    sheet.Cells[8, 1].Value = "Name";
                    sheet.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(50, 205, 50));

                    // sheet.Cells[7, 2, 7, 3].Merge = true;
                    //sheet.Cells[7, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    sheet.Cells[8, 2].Value = "Attendance ";
                    sheet.Cells[8, 3].Value = "Absence";
                    sheet.Cells[8, 4].Value = "checkIN";
                    sheet.Cells[8, 5].Value = "checkOut";





                    int y = 9;
                    int row = 0;
                    if (hrUsers.Count > 0)
                    {
                        foreach (var user in hrUsers)
                        {
                            sheet.Row(y).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            sheet.Cells[y, 1].Value = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                            sheet.Cells[y, 2].Value = user.ISAttendace == true ? true : ' ';
                            sheet.Cells[y, 3].Value = user.ISAttendace == false ? true : ' ';
                            sheet.Cells[y, 4].Value = user.CheckIn.ToString();
                            sheet.Cells[y, 5].Value = user.CheckOut.ToString();

                            y++;

                        }
                        row = y;

                    }







                    //var totaldain = 1000;
                    //var totalmaden = 1200;
                    //if (row >= 11)
                    //{
                    //    sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //    sheet.Cells[row, 1].Value = "الاجمالي";
                    //    sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    sheet.Cells[row, 2].Value = totaldain;
                    //    sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    sheet.Cells[row, 3].Value = totalmaden;
                    //    sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    row = row + 1;
                    //}

                    //if (row >= 12)
                    //{
                    //    sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    //    sheet.Cells[row, 2].Value = totaldain - totalmaden == 0 ? true : false;
                    //    sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

                    //    sheet.Cells[row, 3].Value = totalmaden - totalmaden;
                    //    sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

                    //}





                    sheet.Cells.Style.Font.Name = "Arial";
                    sheet.Cells.Style.Font.Size = 8;



                    sheet.PrinterSettings.Scale = 150;
                    sheet.PrinterSettings.FitToHeight = 0;
                    sheet.PrinterSettings.BottomMargin = 0.5M;
                    sheet.PrinterSettings.TopMargin = 0;
                    sheet.PrinterSettings.LeftMargin = 0;
                    sheet.PrinterSettings.RightMargin = 0;

                    sheet.PrinterSettings.PaperSize = ePaperSize.A4;


                    sheet.Row(5).Style.Font.Bold = true;
                    // sheet.Cells[1, 4, 1, 5].Style.Font.Bold = true;
                    //sheet.Cells[1, 4, 1, 5].Style.Font.Size = 9;
                    sheet.Cells[5, 1, 5, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[5, 1, 5, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;


                    sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));

                    sheet.Cells[8, 5].Style.Font.Bold = true;
                    sheet.Cells[8, 4].Style.Font.Bold = true;
                    sheet.Cells[8, 3].Style.Font.Bold = true;
                    sheet.Cells[8, 2].Style.Font.Bold = true;
                    sheet.Cells[8, 1].Style.Font.Bold = true;


                    sheet.Row(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Save the Excel file

                    i++;


                }

                var path = $"TransportLineExcel";
                var savedPath = Path.Combine(_environment.WebRootPath, path);
                if (System.IO.File.Exists(savedPath))
                    System.IO.File.Delete(savedPath);

                // Create excel file on physical disk  
                Directory.CreateDirectory(savedPath);
                var dateNow = DateTime.Now.ToString("yyyyMMddHHssFFF");
                var excelPath = Path.Combine(savedPath, $"AttendanceReport.xlsx");
                excel.SaveAs(new FileInfo(excelPath));

                var filePath = Globals.baseURL + '\\' + path + $"\\AttendanceReport.xlsx";
                Response.Data = filePath;

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

        //        var filePath = Globals.baseURL + '\\' + path + $"\\AttendanceReport.xlsx";
        //        Response.Data = filePath;
        public BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>> CostOfLine(int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? date, DateTime? dateFrom, DateTime? dateTo, string serialBus, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>> Response = new BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>>();
            Response.Data = new List<CostOfTransportLineVM>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {
                // get all CostOfLines
                var Alldata = new List<CostOfTransportLineVM>();
                var data = new CostOfTransportLineVM();
                int countOfRounds = 0;
                int countOfCheckIn = 0;
                int countOfCheckOut = 0;
                int DeductRoundNum = 0;
                decimal DeductPerRound = 0;
                var transportionAccount = _unitOfWork.TransportationVehicleRouteAccounts.FindAllQueryable(x => x.Id > 0);
                var transportionlineRouteDb = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine", "Supplier" });
                var transprotationBusAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Type == "Bus");
                var transprotationBusAbsence = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => x.Id > 0);

                if (TransportionlineId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.TransportationLineId == TransportionlineId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }


                if (SupplierId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierId == SupplierId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }
                if (supplierContactPersonId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }

                if (!string.IsNullOrEmpty(serialBus))
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.Serial == serialBus && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }
                if (supplierContactPersonId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));

                }
                if (date != null)
                {

                    transportionAccount = transportionAccount.Where(x => x.DateOfMonth.Year == date.Value.Year && x.DateOfMonth.Month == date.Value.Month);

                }
                if (dateFrom != null && dateTo != null)
                {
                    transportionAccount = transportionAccount.Where(x => x.DateOfMonth.Date >= dateFrom.Value.Date && x.DateOfMonth.Date <= dateTo.Value.Date).Distinct();

                }

                var PagedList = PagedList<TransportationVehicleRouteAccount>.Create(transportionAccount, PageNo, NoOfItems);

                Alldata = PagedList.ToList().Select(u =>
                {
                    var route = transportionlineRouteDb.FirstOrDefault(x => x.Id == u.TransportationVehicleRouteId);
                    if (route == null) return null;

                    var lineCost = route?.LineCost ?? 0;
                    var isOneWay = route?.OneWay ?? false;


                    if (date != null && !isOneWay)
                    {
                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date == date.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date == date.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfRounds = countOfCheckIn + countOfCheckOut;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Count();

                    }
                    if (dateFrom != null && dateTo != null && !isOneWay)
                    {
                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date >= dateFrom.Value.Date && x.OneWayDate.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date >= dateFrom.Value.Date && x.CheckOut.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfRounds = countOfCheckIn + countOfCheckOut;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Count();
                    }

                    // oneWay 
                    if (date != null && isOneWay)
                    {
                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date == date.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        //countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date == date.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfRounds = countOfCheckIn;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Count();

                    }
                    if (dateFrom != null && dateTo != null && isOneWay)
                    {
                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date >= dateFrom.Value.Date && x.OneWayDate.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        //countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date >= dateFrom.Value.Date && x.CheckOut.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).DistinctBy(u => u.Id).Count();
                        countOfRounds = countOfCheckIn;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Count();
                    }




                    return new CostOfTransportLineVM
                    {
                        LineName = route.TransportationLine?.LineName,
                        NameOfRoute = route.NameOfRoute,
                        Serial = route.Serial,
                        LineCost = lineCost,
                        SupplierName = route.Supplier?.Name,
                        OneWay = isOneWay,
                        CountOfround = countOfRounds,
                        DeductRoundNum = DeductRoundNum,
                        DeductPerRound = DeductPerRound,
                        NetCost = (decimal)(isOneWay
                            ? ((lineCost * countOfRounds) - (DeductPerRound))
                            : ((lineCost / 2) * countOfRounds) - (DeductPerRound))
                    };
                }).Where(x => x != null).ToList();



                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = PagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = PagedList.TotalCount
                };
                Response.Data = Alldata;

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

        // DashBoard Numbers and Percent 
        //public BaseResponseWithData<DashBoardVM> DashBoard(int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus)
        //{
        //    BaseResponseWithData<DashBoardVM> Response = new BaseResponseWithData<DashBoardVM>();
        //    Response.Data = new DashBoardVM();
        //    Response.Errors = new List<Error>();
        //    Response.Result = true;

        //    try
        //    {
        //        if (DateSerach == null)
        //        {
        //            Response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err10";
        //            error.ErrorMSG = "add date ";
        //            Response.Errors.Add(error);
        //            return Response;
        //        }
        //        DateTime targetDate = DateSerach.Value.Date;
        //        var dashboard = new DashBoardVM();
        //        var transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x =>
        //(x.CheckIn.HasValue && x.CheckIn.Value.Date == targetDate) ||
        //(x.CheckOut.HasValue && x.CheckOut.Value.Date == targetDate) ||
        //(x.OneWayDate.HasValue && x.OneWayDate.Value.Date == targetDate)).DistinctBy(y => y.Serial);

        //        var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);
        //        var TransportationVehicleDB = _unitOfWork.TransportationVehicles.Count(x => true);
        //        if (TransportionlineId > 0)
        //        {
        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.TransportationLineId == TransportionlineId &&  x.Active == true, new[] { "TransportationLine" });
        //            if (!VehicleRoute.Any())
        //            {
        //                Response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err10";
        //                error.ErrorMSG = "هذا الخط غير نشط في هذا المسار";
        //                Response.Errors.Add(error);
        //                return Response;
        //            }
        //            dashboard.VehiclesNum = VehicleRoute.Count();
        //            var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //            dashboard.HrUsersNum = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUserId).Distinct().Count();
        //            dashboard.SuppliersNum = VehicleRoute.Select(s => s.SupplierId).Distinct().Count();
        //            dashboard.TransportLinesNum = 1;
        //            var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //            var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

        //            var AllAttedanceUserIds = transprotationUserAttedance
        //                .Where(x => x.Type == "Person")
        //                .Select(y => long.Parse(y.Serial))
        //                .ToList();
        //            var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
        //            var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();
        //            var commonUserAttedanceAndLine = AllAttedanceUserIds.Intersect(emailInHrUsers.Select(long.Parse)).ToList();
        //            dashboard.HrUsersAttendanceNum = commonUserAttedanceAndLine.Count;
        //            dashboard.VehiclesAttendanceNum = AttedanceBus;
        //            dashboard.VehiclePercent = AttedanceBus == 0 ? 0 : ((decimal)AttedanceBus / (decimal)dashboard.VehiclesNum) * 100;
        //            dashboard.HrUsersPercent = commonUserAttedanceAndLine.Count() == 0 ? 0 : ((decimal)commonUserAttedanceAndLine.Count() / (decimal)AllUsersForLine.Count()) * 100;
        //            //   dashboard.VehiclesTypeNum = TransportationVehicleDB;


        //        }


        //        else if (SupplierId > 0)
        //        {
        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierId == SupplierId &&  x.Active == true, new[] { "TransportationLine" });
        //            if (VehicleRoute == null)
        //            {
        //                Response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err10";
        //                error.ErrorMSG = "هذا المقاول غير نشط في هذا المسار";
        //                Response.Errors.Add(error);
        //                return Response;
        //            }
        //            dashboard.VehiclesNum = VehicleRoute.Count();
        //            var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //            dashboard.HrUsersNum = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUserId).Distinct().Count();
        //            dashboard.TransportLinesNum = VehicleRoute.Select(s => s.TransportationLineId).Distinct().Count();
        //            dashboard.SuppliersNum = 1;
        //            var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //            var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

        //            var AllAttedanceUserIds = transprotationUserAttedance
        //               .Where(x => x.Type == "Person")
        //               .Select(y => long.Parse(y.Serial))
        //               .ToList();
        //            var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();

        //            var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();
        //            var commonUserAttedanceAndLine = AllAttedanceUserIds.Intersect(emailInHrUsers.Select(long.Parse)).ToList(); dashboard.HrUsersAttendanceNum = commonUserAttedanceAndLine.Count;
        //            dashboard.VehiclesAttendanceNum = AttedanceBus;
        //            dashboard.VehiclePercent = AttedanceBus == 0 ? 0 : ((decimal)AttedanceBus / (decimal)dashboard.VehiclesNum) * 100;
        //            dashboard.HrUsersPercent = commonUserAttedanceAndLine.Count() == 0 ? 0 : ((decimal)commonUserAttedanceAndLine.Count() / (decimal)AllUsersForLine.Count()) * 100;
        //            //  dashboard.VehiclesTypeNum = TransportationVehicleDB;


        //        }
        //        else if (supplierContactPersonId > 0)
        //        {
        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId &&  x.Active == true, new[] { "TransportationLine" });
        //            if (VehicleRoute == null)
        //            {
        //                Response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err10";
        //                error.ErrorMSG = "هذا السائق غير نشط في هذا المسار";
        //                Response.Errors.Add(error);
        //                return Response;
        //            }
        //            dashboard.VehiclesNum = VehicleRoute.Count();
        //            var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //            dashboard.HrUsersNum = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUserId).Distinct().Count();
        //            dashboard.TransportLinesNum = VehicleRoute.Select(s => s.TransportationLineId).Distinct().Count();

        //            var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //            var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();
        //            var AllAttedanceUserIds = transprotationUserAttedance
        //                                  .Where(x => x.Type == "Person")
        //                                  .Select(y => long.Parse(y.Serial))
        //                                  .ToList();
        //            var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();

        //            var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();
        //            var commonUserAttedanceAndLine = AllAttedanceUserIds.Intersect(emailInHrUsers.Select(long.Parse)).ToList();
        //            dashboard.HrUsersAttendanceNum = commonUserAttedanceAndLine.Count;
        //            dashboard.VehiclesAttendanceNum = AttedanceBus;

        //            dashboard.VehiclePercent = AttedanceBus == 0 ? 0 : ((decimal)AttedanceBus / (decimal)dashboard.VehiclesNum) * 100;
        //            dashboard.HrUsersPercent = commonUserAttedanceAndLine.Count() == 0 ? 0 : ((decimal)commonUserAttedanceAndLine.Count() / (decimal)AllUsersForLine.Count()) * 100;
        //            // dashboard.VehiclesTypeNum = TransportationVehicleDB;


        //        }

        //        else if (!string.IsNullOrEmpty(serialBus))
        //        {
        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.Serial == serialBus &&  x.Active == true, new[] { "TransportationLine" });
        //            if (VehicleRoute == null)
        //            {
        //                Response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err10";
        //                error.ErrorMSG = "هذه المركبه غير نشطه في هذا المسار";
        //                Response.Errors.Add(error);
        //                return Response;
        //            }
        //            dashboard.VehiclesNum = VehicleRoute.Count();
        //            var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //            dashboard.HrUsersNum = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUserId).Distinct().Count();
        //            dashboard.TransportLinesNum = VehicleRoute.Select(s => s.TransportationLineId).Distinct().Count();

        //            var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //            var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();
        //            var AllAttedanceUserIds = transprotationUserAttedance
        //                                  .Where(x => x.Type == "Person")
        //                                  .Select(y => long.Parse(y.Serial))
        //                                  .ToList();
        //            var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();

        //            var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();
        //            var commonUserAttedanceAndLine = AllAttedanceUserIds.Intersect(emailInHrUsers.Select(long.Parse)).ToList();
        //            dashboard.VehiclesAttendanceNum = AttedanceBus;

        //            dashboard.VehiclePercent = AttedanceBus == 0 ? 0 : ((decimal)AttedanceBus / (decimal)dashboard.VehiclesNum) * 100;
        //            dashboard.HrUsersPercent = commonUserAttedanceAndLine.Count() == 0 ? 0 : ((decimal)commonUserAttedanceAndLine.Count() / (decimal)AllUsersForLine.Count()) * 100;
        //            // dashboard.VehiclesTypeNum = TransportationVehicleDB;


        //        }

        //        else
        //        {
        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true &&  x.Active == true, new[] { "TransportationLine" });
        //            if (VehicleRoute == null)
        //            {
        //                Response.Result = false;
        //                Error error = new Error();
        //                error.ErrorCode = "Err10";
        //                error.ErrorMSG = "لا يوجد مسارات نشطه";
        //                Response.Errors.Add(error);
        //                return Response;
        //            }
        //            dashboard.VehiclesNum = VehicleRoute.Count();
        //            var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //            dashboard.TransportLinesNum = _unitOfWork.TransportationLines.Count(x => x.Active == true);
        //            dashboard.HrUsersNum = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true).Select(x => x.HrUserId).Distinct().Count();
        //            dashboard.SuppliersNum = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true &&  x.Active == true, new[] { "TransportationLine" }).Select(s => s.SupplierId).Distinct().Count();

        //            var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //            var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

        //            var Users = transportationVehicleRouteEmployee.Count();
        //            dashboard.HrUsersAttendanceNum = AttedanceUsers;
        //            dashboard.VehiclesAttendanceNum = AttedanceBus;

        //            dashboard.VehiclePercent = AttedanceBus == 0 ? 0 : ((decimal)AttedanceBus / (decimal)dashboard.VehiclesNum) * 100;
        //            dashboard.HrUsersPercent = AttedanceUsers == 0 ? 0 : ((decimal)AttedanceUsers / (decimal)Users) * 100;


        //        }
        //        dashboard.AllHrUsersNum = _unitOfWork.HrUsers.Count(x => x.Active == true);
        //        dashboard.AllSuppliersNum = _unitOfWork.Suppliers.Count(x => x.Active == true);
        //        dashboard.VehiclesTypeNum = TransportationVehicleDB;

        //        Response.Data = dashboard;
        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Result = false;
        //        Error error = new Error();
        //        error.ErrorCode = "Err10";
        //        error.ErrorMSG = ex.InnerException.Message;
        //        Response.Errors.Add(error);
        //        return Response;
        //    }
        //}

        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence(int TransportionlineId, long RouteId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, bool? AttendaceFlag, long HrUser, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> Response = new BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>>();
            Response.Data = new List<DashBoardForHrUsers>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {


                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;
                if (DateSerach != null)
                {
                    Day = DateSerach?.DayOfWeek.ToString();
                    FullDateTime = (DateTime)DateSerach;
                }




                var Alldata = new List<DashBoardForHrUsers>();
                var data = new DashBoardForHrUsers();
                var transprotationUserAttedance = new List<TransprotationUserAttedance>();
                if (FromDate != null && ToDate != null)
                {
                    transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => ((((DateTime)x.CheckIn).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckIn).Date <= ((DateTime)ToDate).Date) || ((DateTime)x.CheckOut).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckOut).Date <= ((DateTime)ToDate).Date) && x.Type == "Person").ToList();
                }
                else
                {
                    transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => (x.CheckIn.HasValue && x.CheckIn.Value.Date == FullDateTime) ||
        (x.CheckOut.HasValue && x.CheckOut.Value.Date == FullDateTime) && x.Type == "Person").ToList();

                }
                var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);
                var AllHrUsersId = transportationVehicleRouteEmployee.Select(x => x.HrUserId).ToList();
                var HrUserDb = _unitOfWork.HrUsers.FindAllQueryable(x => AllHrUsersId.Contains(x.Id));
                var transportionlineDB = _unitOfWork.TransportationLines.FindAllQueryable(x => true);
                var SupplierDB = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
                var supplierContactPersonDB = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0);

                var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine", "SupplierContactPerson", "Supplier", "TransportationVehicle.VehicleType", "Supervisor" });

                if (TransportionlineId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.TransportationLineId == TransportionlineId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    var HrUsersForLine = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Count();
                    var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

                    var AllAttedanceUserIds = transprotationUserAttedance
                        .Where(x => x.Type == "Person")
                        .Select(y => long.Parse(y.Serial))
                        .ToList();
                    var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();

                    HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));


                }
                else if (RouteId > 0)
                {
                    var HrUsersForLine = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && x.TransportationVehicleRouteId == RouteId).Count();
                    var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

                    var AllAttedanceUserIds = transprotationUserAttedance
                        .Where(x => x.Type == "Person")
                        .Select(y => long.Parse(y.Serial))
                        .ToList();
                    var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => x.TransportationVehicleRouteId == RouteId).Select(y => (long)y.HrUserId).ToList();
                    var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();

                    HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));


                }
                else if (SupplierId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierId == SupplierId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
                    var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

                    var AllAttedanceUserIds = transprotationUserAttedance
                       .Where(x => x.Type == "Person")
                       .Select(y => long.Parse(y.Serial))
                       .ToList();
                    var AllUsersForSupplier = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForSupplier.Contains(x.Id)).Select(y => y.Email).ToList();

                    HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));



                }
                else if (supplierContactPersonId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    var AllAttedanceUserIds = transprotationUserAttedance
                                          .Where(x => x.Type == "Person")
                                          .Select(y => long.Parse(y.Serial))
                                          .ToList();
                    var AllUsersForsupplierContact = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForsupplierContact.Contains(x.Id)).Select(y => y.Email).ToList();

                    HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));


                }

                else if (!string.IsNullOrEmpty(serialBus))
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.Serial == serialBus);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    var AllAttedanceUserIds = transprotationUserAttedance
                                          .Where(x => x.Type == "Person")
                                          .Select(y => long.Parse(y.Serial))
                                          .ToList();
                    var AllUsersForserialBus = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForserialBus.Contains(x.Id)).Select(y => y.Email).ToList();

                    HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));




                }
                if (HrUser > 0)
                {
                    HrUserDb = HrUserDb.Where(x => x.Id == HrUser);



                }

                var attendedUserIds = transprotationUserAttedance.Select(x => x.Serial).ToList();
                if (AttendaceFlag == true)
                {
                    HrUserDb = HrUserDb.Where(x => attendedUserIds.Contains(x.Email));



                }
                if (AttendaceFlag == false)
                {
                    HrUserDb = HrUserDb.Where(x => !attendedUserIds.Contains(x.Email));


                }
                var usersPagedList = PagedList<HrUser>.Create(HrUserDb, PageNo, NoOfItems);


                var MaritalStatusDB = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0);






                Alldata = usersPagedList.Where(x => AllHrUsersId.Contains((long)x.Id)).Select(u => new DashBoardForHrUsers
                {

                    Id = u.Id,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    ISAttendace = attendedUserIds.Contains(u.Email),
                    TransportionlineName = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                            VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationLine.LineName).FirstOrDefault(),
                    SupplierName = SupplierId > 0 ? SupplierDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                            VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),
                    supplierContactPersonName = supplierContactPersonId > 0 ? supplierContactPersonDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                            VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.SupplierContactPerson.Name).FirstOrDefault(),

                    VehicleType = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                            VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationVehicle.VehicleType.Type).FirstOrDefault(),
                    CheckIn = transprotationUserAttedance.Where(x => x.Serial == u.Email).Select(c => c.CheckIn).FirstOrDefault(),
                    CheckOut = transprotationUserAttedance.Where(x => x.Serial == u.Email).Select(c => c.CheckOut).FirstOrDefault(),
                    SupervisorName = VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supervisor.FirstName).FirstOrDefault() ?? null,
                    MaritalStatus = MaritalStatusDB.Where(x => x.Id == u.MaritalStatusId).Select(n => n.Name).FirstOrDefault() ?? null
                    //  ISAttendace = transprotationUserAttedance.Where(x => long.Parse(x.Serial) == u.Id).FirstOrDefault() != null ? true : false
                }).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = usersPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = usersPagedList.TotalCount
                };
                Response.Data = Alldata;

                return Response;

            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException?.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }




        //    public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence(int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, long HrUser, int PageNo, int NoOfItems)
        //    {
        //        BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> Response = new BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>>();
        //        Response.Data = new List<DashBoardForHrUsers>();
        //        Response.Errors = new List<Error>();
        //        Response.Result = true;

        //        try
        //        {


        //            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
        //            DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

        //            var Day = egyptTime.ToString("dddd");
        //            var FullDateTime = egyptTime;
        //            if (DateSerach != null)
        //            {
        //                Day = DateSerach?.DayOfWeek.ToString();
        //                FullDateTime = (DateTime)DateSerach;
        //            }




        //            var Alldata = new List<DashBoardForHrUsers>();
        //            var data = new DashBoardForHrUsers();
        //            var transprotationUserAttedance = new List<TransprotationUserAttedance>();
        //            if (FromDate != null && ToDate != null)
        //            {
        //                transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => ((((DateTime)x.CheckIn).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckIn).Date <= ((DateTime)ToDate).Date) || ((DateTime)x.CheckOut).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckOut).Date <= ((DateTime)ToDate).Date) && x.Type == "Person").ToList();
        //            }
        //            else
        //            {
        //                transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => (x.CheckIn.HasValue && x.CheckIn.Value.Date == FullDateTime) ||
        //    (x.CheckOut.HasValue && x.CheckOut.Value.Date == FullDateTime) && x.Type == "Person").ToList();

        //            }
        //            var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);
        //            var AllHrUsersId = transportationVehicleRouteEmployee.Select(x => x.HrUserId).ToList();
        //            var HrUserDb = _unitOfWork.HrUsers.FindAllQueryable(x => AllHrUsersId.Contains(x.Id));
        //            var transportionlineDB = _unitOfWork.TransportationLines.FindAllQueryable(x => x.Id > 0 && x.Active == true);
        //            var SupplierDB = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
        //            var supplierContactPersonDB = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0);

        //            var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 &&  x.Active == true, new[] { "TransportationLine", "SupplierContactPerson", "Supplier", "TransportationVehicle.VehicleType", "Supervisor" });

        //            if (TransportionlineId > 0)
        //            {
        //                VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.TransportationLineId == TransportionlineId);
        //                var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //                var HrUsersForLine = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Count();
        //                var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

        //                var AllAttedanceUserIds = transprotationUserAttedance
        //                    .Where(x => x.Type == "Person")
        //                    .Select(y => long.Parse(y.Serial))
        //                    .ToList();
        //                var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
        //                var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();

        //                HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));


        //            }


        //            else if (SupplierId > 0)
        //            {
        //                VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierId == SupplierId);
        //                var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
        //                var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
        //                var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

        //                var AllAttedanceUserIds = transprotationUserAttedance
        //                   .Where(x => x.Type == "Person")
        //                   .Select(y => long.Parse(y.Serial))
        //                   .ToList();
        //                var AllUsersForSupplier = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
        //                var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForSupplier.Contains(x.Id)).Select(y => y.Email).ToList();

        //                HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));



        //            }
        //            else if (supplierContactPersonId > 0)
        //            {
        //                VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId);
        //                var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

        //                var AllAttedanceUserIds = transprotationUserAttedance
        //                                      .Where(x => x.Type == "Person")
        //                                      .Select(y => long.Parse(y.Serial))
        //                                      .ToList();
        //                var AllUsersForsupplierContact = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
        //                var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForsupplierContact.Contains(x.Id)).Select(y => y.Email).ToList();

        //                HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));


        //            }

        //            else if (!string.IsNullOrEmpty(serialBus))
        //            {
        //                VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.Serial == serialBus);
        //                var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

        //                var AllAttedanceUserIds = transprotationUserAttedance
        //                                      .Where(x => x.Type == "Person")
        //                                      .Select(y => long.Parse(y.Serial))
        //                                      .ToList();
        //                var AllUsersForserialBus = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
        //                var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForserialBus.Contains(x.Id)).Select(y => y.Email).ToList();

        //                HrUserDb = HrUserDb.Where(x => emailInHrUsers.Contains(x.Email));




        //            }
        //            if (HrUser > 0)
        //            {
        //                HrUserDb = HrUserDb.Where(x => x.Id == HrUser);



        //            }

        //            var usersPagedList = PagedList<HrUser>.Create(HrUserDb, PageNo, NoOfItems);


        //            var attendedUserIds = transprotationUserAttedance
        //.Select(x => x.Serial).ToList();


        //            Alldata = usersPagedList.Where(x => AllHrUsersId.Contains((long)x.Id)).Select(u => new DashBoardForHrUsers
        //            {

        //                Id = u.Id,
        //                FirstName = u.FirstName,
        //                MiddleName = u.MiddleName,
        //                LastName = u.LastName,
        //                ISAttendace = attendedUserIds.Contains(u.Email),
        //                TransportionlineName = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
        //                                        VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationLine.LineName).FirstOrDefault(),
        //                SupplierName = SupplierId > 0 ? SupplierDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
        //                                        VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),
        //                supplierContactPersonName = supplierContactPersonId > 0 ? supplierContactPersonDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
        //                                        VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.SupplierContactPerson.Name).FirstOrDefault(),

        //                VehicleType = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
        //                                        VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n..TransportationVehicle.VehicleType.Type).FirstOrDefault(),
        //                CheckIn = transprotationUserAttedance.Where(x => x.Serial == u.Email).Select(c => c.CheckIn).FirstOrDefault(),
        //                CheckOut = transprotationUserAttedance.Where(x => x.Serial == u.Email).Select(c => c.CheckOut).FirstOrDefault(),
        //                SupervisorName = VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supervisor.FirstName).FirstOrDefault() ?? null,
        //                //  ISAttendace = transprotationUserAttedance.Where(x => long.Parse(x.Serial) == u.Id).FirstOrDefault() != null ? true : false
        //            }).ToList();

        //            Response.PaginationHeader = new PaginationHeader
        //            {
        //                CurrentPage = PageNo,
        //                TotalPages = usersPagedList.TotalPages,
        //                ItemsPerPage = NoOfItems,
        //                TotalItems = usersPagedList.TotalCount
        //            };
        //            Response.Data = Alldata;

        //            return Response;

        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err10";
        //            error.ErrorMSG = ex.InnerException?.Message;
        //            Response.Errors.Add(error);
        //            return Response;
        //        }
        //    }

        public BaseResponse DeleteTransportationDirection(long Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var transportationDirection = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.Id == Id).FirstOrDefault();
                if (transportationDirection == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                    return Response;
                }
                var transportationEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.TransportationVehicleRouteDirectionId == Id).FirstOrDefault();
                if (transportationEmployee != null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "المحطة مسجلة لمستخدم";
                    Response.Errors.Add(error);
                    return Response;
                }
                _unitOfWork.TransportationVehicleRouteDirections.Delete(transportationDirection);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse DeleteTransportationEmployee(long Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                    return Response;
                }
                var transportationEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Id == Id).FirstOrDefault();
                if (transportationEmployee == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                    return Response;
                }
                _unitOfWork.TransportationVehicleRouteEmployees.Delete(transportationEmployee);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse DeleteTransportationLine(int Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                    return Response;
                }
                var transportationLine = _unitOfWork.TransportationLines.FindAll(x => x.Id == Id).FirstOrDefault();
                if (transportationLine == null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id Or line Not Active";
                    Response.Errors.Add(error);
                    return Response;
                }
                //transportationLine.Active = false;
                _unitOfWork.TransportationLines.Delete(transportationLine);
                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse DeleteTransportationRoute(long Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    Response.Result = true;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                    return Response;

                }
                var transportationLineRoutes = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == Id && x.Active == true).FirstOrDefault();
                if (transportationLineRoutes == null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                    return Response;
                }
                var transportationEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.TransportationVehicleRouteId == Id).FirstOrDefault();
                if (transportationEmployees != null)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "يجب مسح الموظفين اولا في هذا الخط";
                    Response.Errors.Add(error);
                    return Response;
                }
                var transportationDirections = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.TransportationVehicleRouteId == Id).ToList();
                if (transportationDirections != null)
                {
                    _unitOfWork.TransportationVehicleRouteDirections.DeleteRange(transportationDirections);
                }

                _unitOfWork.TransportationVehicleRoutes.Delete(transportationLineRoutes);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse DeleteTransportationVehicle(int Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var transportationVehicle = _unitOfWork.TransportationVehicles.FindAll(x => x.Id == Id).FirstOrDefault();
                if (transportationVehicle == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                _unitOfWork.TransportationVehicles.Delete(transportationVehicle);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse DeleteRouteEmployee(int Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var RouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Id == Id).FirstOrDefault();
                if (RouteEmployee == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                _unitOfWork.TransportationVehicleRouteEmployees.Delete(RouteEmployee);

                _unitOfWork.Complete();
                Response.Result = true;
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
        public BaseResponse DeleteVehicleType(int Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var VehicleType = _unitOfWork.VehicleTypes.FindAll(x => x.Id == Id).FirstOrDefault();
                if (VehicleType == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                _unitOfWork.VehicleTypes.Delete(VehicleType);

                _unitOfWork.Complete();
                Response.Result = true;
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



        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence88(int TransportionlineId, long RouteId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, bool? AttendaceFlag, long HrUser, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> Response = new BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {
                // 1. تحديد النطاق الزمني
                List<DateTime> dateRange = new List<DateTime>();
                if (FromDate.HasValue && ToDate.HasValue)
                {
                    for (var date = FromDate.Value.Date; date <= ToDate.Value.Date; date = date.AddDays(1))
                    {
                        dateRange.Add(date);
                    }
                }
                else
                {
                    TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                    DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                    dateRange.Add(DateSerach?.Date ?? egyptTime.Date);
                }

                // 2. جلب بيانات الحضور للنطاق المحدد وتحويلها لـ Lookup لسرعة البحث
                var startRange = dateRange.Min();
                var endRange = dateRange.Max();

                var attendanceData = _unitOfWork.TransprotationUserAttedances
                    .FindAll(x => x.Type == "Person" &&
                                 ((x.CheckIn.HasValue && x.CheckIn.Value.Date >= startRange && x.CheckIn.Value.Date <= endRange) ||
                                  (x.CheckOut.HasValue && x.CheckOut.Value.Date >= startRange && x.CheckOut.Value.Date <= endRange)))
                    .ToLookup(x => new { Email = x.Serial, Date = (x.CheckIn ?? x.CheckOut).Value.Date });

                // 3. بناء الفلاتر الأساسية للموظفين
                var query = _unitOfWork.HrUsers.FindAllQueryable(u => u.Id > 0);

                // ربط الموظفين بمسارات الحافلات لجلب بيانات الخط والمورد
                var userRoutes = _unitOfWork.TransportationVehicleRouteEmployees
                    .FindAllQueryable(x => x.Active == true, new[] { "TransportationVehicleRoute.TransportationVehicle.VehicleType", "TransportationVehicleRoute.Supplier", "TransportationVehicleRoute.SupplierContactPerson", "TransportationVehicleRoute.Supervisor" });

                // تطبيق الفلاتر (خط، مسار، مورد... إلخ)
                if (TransportionlineId > 0)
                    userRoutes = userRoutes.Where(x => x.TransportationVehicleRoute.TransportationLineId == TransportionlineId);
                if (RouteId > 0)
                    userRoutes = userRoutes.Where(x => x.TransportationVehicleRouteId == RouteId);
                if (SupplierId > 0)
                    userRoutes = userRoutes.Where(x => x.TransportationVehicleRoute.SupplierId == SupplierId);
                if (supplierContactPersonId > 0)
                    userRoutes = userRoutes.Where(x => x.TransportationVehicleRoute.SupplierContactPersonId == supplierContactPersonId);
                if (!string.IsNullOrEmpty(serialBus))
                    userRoutes = userRoutes.Where(x => x.TransportationVehicleRoute.Serial == serialBus);

                if (HrUser > 0)
                    query = query.Where(x => x.Id == HrUser);

                // تصفية الموظفين بناءً على المسارات المختارة
                var validUserIds = userRoutes.Select(x => x.HrUserId).Distinct().ToList();
                query = query.Where(x => validUserIds.Contains(x.Id));

                var allFilteredUsers = query.ToList();
                var allUserRoutesInfo = userRoutes.ToList().ToLookup(x => x.HrUserId);
                var maritalStatusDB = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0).ToDictionary(x => x.Id, x => x.Name);

                // 4. بناء القائمة النهائية (تكرار الموظف لكل يوم في النطاق)
                var flatData = new List<DashBoardForHrUsers>();

                foreach (var date in dateRange)
                {
                    foreach (var user in allFilteredUsers)
                    {
                        var attendance = attendanceData[new { Email = user.Email, Date = date }].FirstOrDefault();
                        var routeInfo = allUserRoutesInfo[user.Id].FirstOrDefault()?.TransportationVehicleRoute;

                        bool isAttended = attendance != null;

                        // تطبيق فلتر الحضور/الغياب إذا وجد
                        if (AttendaceFlag.HasValue && AttendaceFlag.Value != isAttended)
                            continue;

                        flatData.Add(new DashBoardForHrUsers
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            MiddleName = user.MiddleName,
                            LastName = user.LastName,
                            ISAttendace = isAttended,
                            CheckIn = attendance?.CheckIn,
                            CheckOut = attendance?.CheckOut,
                            TransportionlineName = routeInfo?.TransportationLine?.LineName,
                            SupplierName = routeInfo?.Supplier?.Name,
                            supplierContactPersonName = routeInfo?.SupplierContactPerson?.Name,
                            VehicleType = routeInfo?.TransportationVehicle?.VehicleType?.Type,
                            SupervisorName = routeInfo?.Supervisor?.FirstName,
                            MaritalStatus = user.MaritalStatusId.HasValue && maritalStatusDB.ContainsKey(user.MaritalStatusId.Value) ? maritalStatusDB[user.MaritalStatusId.Value] : null
                        });
                    }
                }

                // 5. تطبيق الترقيم (Pagination) على القائمة النهائية المسطحة
                var pagedData = flatData.Skip((PageNo - 1) * NoOfItems).Take(NoOfItems).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    ItemsPerPage = NoOfItems,
                    TotalItems = flatData.Count,
                    TotalPages = (int)Math.Ceiling((double)flatData.Count / NoOfItems)
                };

                Response.Data = pagedData;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }

        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>> DashBoardForAttendenceDuration(int TransportionlineId, long RouteId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, bool? AttendaceFlag, long HrUser, string? Email, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>> Response = new BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {
                // 1. تحديد النطاق الزمني (الأيام المطلوبة)
                DateTime startDate = (FromDate ?? DateSerach ?? DateTime.Now).Date;
                DateTime endDate = (ToDate ?? DateSerach ?? DateTime.Now).Date;
                var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                          .Select(offset => startDate.AddDays(offset))
                                          .ToList();

                // 2. جلب بيانات الحضور وتحويلها لـ Lookup لسرعة البحث (Email + Date)
                var attendanceRaw = _unitOfWork.TransprotationUserAttedances.FindAll(x =>
                    x.Type == "Person" &&
                    ((x.CheckIn.HasValue && x.CheckIn.Value.Date >= startDate && x.CheckIn.Value.Date <= endDate) ||
                     (x.CheckOut.HasValue && x.CheckOut.Value.Date >= startDate && x.CheckOut.Value.Date <= endDate)));

                var attendanceLookup = attendanceRaw.ToLookup(a => new
                {
                    Email = a.Serial,
                    Date = (a.CheckIn ?? a.CheckOut).Value.Date
                });

                // 3. فلاتر الموظفين والمسارات (نفس منطقك السابق)
                var userRoutesQuery = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true,
                    new[] { "TransportationVehicleRoute.TransportationLine", "TransportationVehicleRoute.Supplier", "TransportationVehicleRoute.Supervisor", "TransportationVehicleRoute.TransportationVehicle.VehicleType" });

                if (TransportionlineId > 0) userRoutesQuery = userRoutesQuery.Where(x => x.TransportationVehicleRoute.TransportationLineId == TransportionlineId);
                if (RouteId > 0) userRoutesQuery = userRoutesQuery.Where(x => x.TransportationVehicleRouteId == RouteId);
                if (SupplierId > 0) userRoutesQuery = userRoutesQuery.Where(x => x.TransportationVehicleRoute.SupplierId == SupplierId);

                var filteredUserRoutes = userRoutesQuery.ToList();
                var filteredUserIds = filteredUserRoutes.Select(x => x.HrUserId).Distinct().ToList();

                var query = _unitOfWork.HrUsers.FindAllQueryable(u => filteredUserIds.Contains(u.Id));
                if (HrUser > 0) query = query.Where(u => u.Id == HrUser);
                if (!string.IsNullOrEmpty(Email)) query = query.Where(u => u.Email == Email);

                // الترقيم على مستوى الموظفين
                var pagedUsers = PagedList<HrUser>.Create(query, PageNo, NoOfItems);
                var maritalStatusDB = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0);

                // 4. بناء النتيجة النهائية (موظف واحد وبداخله قائمة أيام)
                var result = pagedUsers.Select(user =>
                {
                    var userRoute = filteredUserRoutes.FirstOrDefault(r => r.HrUserId == user.Id)?.TransportationVehicleRoute;

                    var userDto = new DashBoardForHrUsersFromDuration
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        TransportionlineName = userRoute?.TransportationLine?.LineName,
                        NameOfRoute = userRoute?.NameOfRoute ?? null,
                        SupplierName = userRoute?.Supplier?.Name,
                        SupervisorName = userRoute?.Supervisor?.FirstName,
                        MaritalStatus = maritalStatusDB.FirstOrDefault(x => x.Id == user.MaritalStatusId)?.Name,
                        Email = user.Email,
                        VehicleType = userRoute?.TransportationVehicle?.VehicleType?.Type,
                        AttendanceHistory = new List<AttendanceDayDetail>()
                    };

                    // ملء قائمة الأيام للموظف بناءً على المدة المطلوبة
                    foreach (var day in dateRange)
                    {
                        var dayAtt = attendanceLookup[new { Email = user.Email, Date = day }].FirstOrDefault();
                        bool isAttended = dayAtt != null;

                        // تطبيق فلتر الحضور/الغياب إذا وجد
                        if (AttendaceFlag.HasValue && AttendaceFlag.Value != isAttended)
                            continue;

                        userDto.AttendanceHistory.Add(new AttendanceDayDetail
                        {
                            Date = day,
                            ISAttendace = dayAtt != null,
                            CheckIn = dayAtt?.CheckIn,
                            CheckOut = dayAtt?.CheckOut
                        });
                    }

                    return userDto;
                }).ToList();

                Response.Data = result;
                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = pagedUsers.TotalPages,
                    TotalItems = pagedUsers.TotalCount,
                    ItemsPerPage = NoOfItems
                };

                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }
        public BaseResponseWithData<List<getTransportationRoutesForHrUserVm>> getTransportationRoutesForHrUser(long HrUserId)
        {
            BaseResponseWithData<List<getTransportationRoutesForHrUserVm>> Response = new BaseResponseWithData<List<getTransportationRoutesForHrUserVm>>();
            Response.Data = new List<getTransportationRoutesForHrUserVm>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var RoutesInHrUserDb = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(c => c.HrUserId == HrUserId, new[] { "TransportationVehicleRoute.TransportationLine" });


                Response.Data = RoutesInHrUserDb.Select(a => new getTransportationRoutesForHrUserVm
                {
                    Id = a.Id,
                    RouteId = a.TransportationVehicleRouteId,
                    TranspotionLineName = a.TransportationVehicleRoute.TransportationLine.LineName,
                    Serial = a.TransportationVehicleRoute.Serial,
                    PeriodFrom = a.TransportationVehicleRoute.PeriodFrom,
                    PeriodTo = a.TransportationVehicleRoute.PeriodTo,
                    Active = a.Active,
                    FromDate = a.FromDate,
                    ToDate = a.ToDate,
                    CreationDate = a.CreationDate,
                    Period = a.Period,
                    DurationLatitude = a.DurationLatitude,
                    DurationLongtitud = a.DurationLongtitud,
                    TransportationVehicleRouteDirectionId = a.TransportationVehicleRouteDirectionId,
                    NameOfRoute = a.TransportationVehicleRoute.NameOfRoute ?? null
                }).ToList();




                Response.Result = true;
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

        public BaseResponseWithDataAndHeader<List<RouteDeductionVM>> getAllDeduction(int Id, DateTime? FromDate, DateTime? ToDate, long SupplierId, long RouteId, string? Name, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<RouteDeductionVM>> Response = new BaseResponseWithDataAndHeader<List<RouteDeductionVM>>();
            Response.Data = new List<RouteDeductionVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var RouteDeductionsDB = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => x.Id > 0, new[] { "TransportationVehicleRoute.TransportationLine", "CreationByNavigation" });
                var SupplierDb = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
                var SupplierContentPersonDb = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0);
                var allroutes = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => true);

                if (!string.IsNullOrEmpty(Name))
                {
                    Name = HttpUtility.UrlDecode(Name);
                    var allroutesIds = allroutes.Where(a => a.NameOfRoute.Trim().Contains(Name.Trim()) || (a.Serial.Trim().Contains(Name.Trim()))).Select(x => x.Id).AsQueryable();
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => allroutesIds.Contains(x.TransportationVehicleRouteId));

                }

                if (Id > 0)
                {
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => x.Id == Id);


                }
                if (SupplierId > 0)
                {
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => x.SupplierId == SupplierId);


                }
                if (RouteId > 0)
                {
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => x.TransportationVehicleRouteId == RouteId);


                }

                if (FromDate != null)
                {
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => x.DateOfDeduction.Date >= ((DateTime)FromDate).Date);


                }
                if (ToDate != null)
                {
                    RouteDeductionsDB = RouteDeductionsDB.Where(x => x.DateOfDeduction.Date <= ((DateTime)ToDate).Date);


                }
                RouteDeductionsDB = RouteDeductionsDB.OrderByDescending(x => x.Id);
                var transportPagedList = PagedList<TransportationVehicleRouteDeduction>.Create(RouteDeductionsDB, PageNo, NoOfItems);
                Response.Data = transportPagedList.Select(a => new RouteDeductionVM
                {
                    Id = a.Id,
                    SupplierId = a.SupplierId,
                    SupplierName = SupplierDb.Where(x => x.Id == a.SupplierId).Select(n => n.Name).FirstOrDefault(),
                    TransportationVehicleRouteName = a.TransportationVehicleRoute.NameOfRoute,
                    TransportationVehicleRouteId = a.TransportationVehicleRouteId,
                    Serial = a.Serial,
                    DeductPerRound = a.DeductPerRound,
                    DateOfDeduction = a.DateOfDeduction,
                    Cause = a.Cause,
                    CreationBy = a.CreationBy,
                    CreationName = a.CreationByNavigation.FirstName + " " + a.CreationByNavigation.MiddleName,
                    CreationDate = a.CreationDate,
                    //SupplierContentPersonId = a.TransportationVehicleRoute.SupplierContactPersonId,
                    //SupplierContentPersonIdName = SupplierContentPersonDb.Where(x => x.Id == a.TransportationVehicleRoute.SupplierContactPersonId).Select(r => r.Name).FirstOrDefault()
                    SupplierContentPersonIdName = SupplierContentPersonDb.Where(x => x.Id == a.TransportationVehicleRoute.SupplierContactPersonId).Select(r => r.Name).FirstOrDefault() ?? "N/A",
                    SupplierContentPersonId = a.TransportationVehicleRoute.SupplierContactPersonId ?? 0,
                    routePrice = a.TransportationVehicleRoute.LineCost,
                    typeOfDeduction = a.TypeOfDedct

                }).ToList();
                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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

        // TransportationVehicleRouteDeduction
        public BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>> getAllModifyPriceOfTransportationLine(int PageNo, int NoOfItems, bool? Approve)
        {
            BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>> Response = new BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>>();
            Response.Data = new List<getAllModifyPriceVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {

                var VehicleTypeDb = _unitOfWork.VehicleTypes.FindAll(x => x.Id > 0);
                var AllTransportationLineIncreaseRequests = _unitOfWork.TransportationLineIncreaseRequests.FindAllQueryable(x => x.Id > 0, new[] { "CreationByNavigation" });
                var AllTransportationLineIncreaseRequestsLines = _unitOfWork.TransportationLineIncreaseRequestLines.FindAll(x => x.Id > 0 && x.Route.Active == true, new[] { "Route" }).AsQueryable();
                if (Approve == true)
                {
                    AllTransportationLineIncreaseRequests = AllTransportationLineIncreaseRequests.Where(x => x.Approve != false).AsQueryable();
                }
                else if (Approve == false)
                {
                    AllTransportationLineIncreaseRequests = AllTransportationLineIncreaseRequests.Where(x => x.Approve == false).AsQueryable();
                }
                var transportPagedList = PagedList<TransportationLineIncreaseRequest>.Create(AllTransportationLineIncreaseRequests, PageNo, NoOfItems);

                Response.Data = transportPagedList.Select(a => new getAllModifyPriceVM
                {
                    Id = a.Id,
                    IsPercent = a.IsPercent,
                    IncreaseCost = a.IncreaseCost,
                    ForAllLines = a.ForAllLines,
                    ApprovedBy = a.ApprovedBy,
                    ApprovedByName = a.CreationByNavigation?.FirstName + " " + a.CreationByNavigation?.MiddleName + " " + a.CreationByNavigation?.LastName,
                    ApprovedDate = a.ApprovedDate,
                    CreationDate = a.CreationDate,
                    CreationBy = a.CreationBy,
                    CreationName = a.CreationByNavigation.FirstName + " " + a.CreationByNavigation.MiddleName + " " + a.CreationByNavigation.LastName,
                    Approve = a.Approve,
                    ApproximateToFiveFlag = a.ApproximateToFiveFlag,
                    transportationLineDetails = a.ForAllLines == false ? AllTransportationLineIncreaseRequestsLines.Where(x => x.TransportationLineIncreaseRequestId == a.Id).Select(t => new TransportationLineDetailsVM
                    {
                        RouteId = t.RouteId,
                        RouteName = t.Route.NameOfRoute,
                        PriceBefore = a.Approve == true ? null : t.Route.LineCost,
                        PriceAfter = a.Approve == true ? null : a.IsPercent == false && a.ApproximateToFiveFlag == false ? t.Route.LineCost + a.IncreaseCost :
                                     a.IsPercent == false && a.ApproximateToFiveFlag == true ? CustomApproximateToFive(t.Route.LineCost + a.IncreaseCost) :
                                     a.IsPercent == true && a.ApproximateToFiveFlag == true ?
                                  CustomApproximateToFive(((t.Route.LineCost * a.IncreaseCost) / 100))
                                 : (((t.Route.LineCost * a.IncreaseCost) / 100)),

                    }).ToList() : null
                }).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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

        // TransportationLine
        public BaseResponseWithDataAndHeader<List<TransportationLineVM>> getAllRoutes(long RouteId, string Name, int transportationLineId, long supplierId, long supplierContactPersonId, string serialBus, int PageNo, int NoOfItems, bool Active)
        {


            BaseResponseWithDataAndHeader<List<TransportationLineVM>> Response = new BaseResponseWithDataAndHeader<List<TransportationLineVM>>();
            Response.Data = new List<TransportationLineVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;



            try
            {
                // var AllTransportationLines = _unitOfWork.TransportationLines.FindAllQueryable(x => x.Id > 0 && x.Active == Active);
                var AllTransportationLines = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0, new[] { "TransportationLine", "TransportationVehicle.VehicleType" });
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = HttpUtility.UrlDecode(Name);
                    AllTransportationLines = AllTransportationLines.Where(a => a.NameOfRoute.Trim().Contains(Name.Trim())).AsQueryable();

                }
                if (transportationLineId > 0 || supplierId > 0 || supplierContactPersonId > 0 || !string.IsNullOrEmpty(serialBus))

                {
                    AllTransportationLines = GetVehicleRoutes(RouteId, transportationLineId, supplierId, supplierContactPersonId, serialBus);
                    if (!AllTransportationLines.Any())
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err10";
                        error.ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر";
                        Response.Errors.Add(error);
                        return Response;
                    }
                    //var vehicleRoutesIDs = vehicleRoutes.Select(x => x.TransportationLineId).ToList();
                    //AllTransportationLines = AllTransportationLines.Where(x => vehicleRoutesIDs.Contains(x.Id));
                }



                var transportPagedList = PagedList<TransportationVehicleRoute>.Create(AllTransportationLines, PageNo, NoOfItems);
                //var routeLine = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine" });

                Response.Data = transportPagedList.Where(x => x.Id > 0).Select(a => new TransportationLineVM
                {
                    Id = a.Id,
                    TransportationVehicleId = a.TransportationVehicleId,
                    Active = a.Active,
                    CreationBy = a.CreationBy,
                    CreationDate = a.CreationDate,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedDate = a.ModifiedDate,
                    LineCost = a.LineCost,
                    NameOfRoute = a.NameOfRoute,
                    OneWay = a.OneWay ?? false,
                    ApprovedBy = a.ApprovedBy,
                    IsApproved = a.IsApproved ?? false,
                    LineName = a.TransportationLine.LineName,
                    Capacity = a.TransportationVehicle.Capacity,
                    RouteNum = AllTransportationLines.Where(x => x.TransportationLineId == a.Id).Count(),
                    TransportationVehicleName = a.TransportationVehicle.VehicleType.Type ?? "N/A"
                }).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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
        // SupplierListForTransportationLine
        // get pagelist route 
        public BaseResponseWithDataAndHeader<List<TransportationRouteVM>> getAllTransportationRoute(long RouteId, string Name, int transportationLineId, long supplierId, long supplierContactPersonId, string serialBus, int PageNo, int NoOfItems, bool Active)
        {
            BaseResponseWithDataAndHeader<List<TransportationRouteVM>> Response = new BaseResponseWithDataAndHeader<List<TransportationRouteVM>>();
            Response.Data = new List<TransportationRouteVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;

                //var TransportationLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0).AsQueryable();
                var supplierDb = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
                var supplierContactPersionDb = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0).AsQueryable();
                var AllTransportationRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "Supervisor", "BranchSchedule", "TransportationVehicle.VehicleType", "TransportationLine" });
                var AllTransportationLines = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0, new[] { "TransportationLine", "TransportationVehicle.VehicleType" });
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = HttpUtility.UrlDecode(Name);
                    AllTransportationLines = AllTransportationLines.Where(a => a.NameOfRoute.Trim().Contains(Name.Trim()) || (a.Serial.Trim().Contains(Name.Trim()))).AsQueryable();

                }
                if (transportationLineId > 0 || supplierId > 0 || supplierContactPersonId > 0 || !string.IsNullOrEmpty(serialBus))

                {
                    AllTransportationLines = GetVehicleRoutes(RouteId, transportationLineId, supplierId, supplierContactPersonId, serialBus);
                    if (!AllTransportationLines.Any())
                    {
                        Response.Result = false;
                        Error error = new Error();
                        error.ErrorCode = "Err10";
                        error.ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر";
                        Response.Errors.Add(error);
                        return Response;
                    }
                    //var vehicleRoutesIDs = vehicleRoutes.Select(x => x.TransportationLineId).ToList();
                    //AllTransportationLines = AllTransportationLines.Where(x => vehicleRoutesIDs.Contains(x.Id));
                }

                var transportPagedList = PagedList<TransportationVehicleRoute>.Create(AllTransportationLines, PageNo, NoOfItems);

                var AllemployeeExpection = _unitOfWork.TransportationVehicleRouteEmployeeExceptions
   .FindAll(r => r.Id > 0 && ((((DateTime)r.FromDate).Date <= FullDateTime.Date && (((DateTime)r.ToDate).Date >= FullDateTime.Date || r.ToDate == null) && r.DayName == Day) || (r.ExceptionDate == FullDateTime.Date)), new[] { "HrUser" }).ToList();

                var TransportationVehicleRouteEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true);
                var transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances
                                                             .FindAll(x =>
                                                              ((DateTime)x.CheckIn).Date == FullDateTime.Date ||
                                                              ((DateTime)x.CheckOut).Date == FullDateTime.Date).Select(u => long.Parse(u.Serial)).ToList();
                var DirectionDb = _unitOfWork.TransportationVehicleRouteDirections.FindAllQueryable(x => x.Id > 0);

                var christianID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "C").Select(c => c.Id).FirstOrDefault();
                var MuslimID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "M").Select(c => c.Id).FirstOrDefault();
                var RoutesEmployesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                Response.Data = transportPagedList.Where(x => x.Id > 0).Select(a =>
                {
                    var usersInRoute = TransportationVehicleRouteEmployees.Where(r => r.FromDate.HasValue)
                                    .Where(r => r.TransportationVehicleRouteId == a.Id &&
                                    ((DateTime)r.FromDate).Date <= FullDateTime.Date &&
                                    (r.ToDate?.Date >= FullDateTime.Date || r.ToDate == null)
                                    ).Select(u => u.HrUserId)
                                     .ToList();

                    //var userInRouteCount = usersInRoute.Count;

                    var exceptionNumFromOtherLines = AllemployeeExpection
                        .Where(r => r.TransportationVehicleRouteId == a.Id).Select(u => u.HrUserId)
                        .ToList();

                    var routeEmployeesToOtherLines = AllemployeeExpection
                        .Where(ex =>
                            usersInRoute
                                .Contains(ex.HrUser.Id)
                        ).Select(r => r.HrUserId).ToList();

                    var userINRouteAndFromOtherRoute = usersInRoute.Union(exceptionNumFromOtherLines).ToList();
                    var userINRouteExpectToOtherroute = userINRouteAndFromOtherRoute.Except(routeEmployeesToOtherLines).ToList();
                    var ActualUserAttedance = userINRouteExpectToOtherroute.Intersect(transprotationUserAttedance).ToList();



                    return new TransportationRouteVM
                    {
                        Id = a.Id,
                        NameOfRoute = a.NameOfRoute ?? null,
                        LineName = a.TransportationLine.LineName ?? "N/A",
                        LineCost = a.LineCost,
                        LineId = a.TransportationLineId,

                        SupplierName = supplierDb.Where(x => x.Id == a.SupplierId).Select(y => y.Name).FirstOrDefault() ?? "No Supplier",
                        SupplierId = a.SupplierId ?? 0,

                        SupplierContactPersonName = supplierContactPersionDb.Where(x => x.Id == a.SupplierContactPersonId).Select(y => y.Name).FirstOrDefault() ?? "N/A",
                        SupplierContactPersonId = a.SupplierContactPersonId ?? 0,

                        branchScheduleId = a.BranchScheduleId ?? 0,
                        ShiftNumber = a.BranchSchedule?.ShiftNumber ?? 0,

                        Serial = a.Serial ?? "No Serial",

                        PeriodFrom = a.PeriodFrom,
                        PeriodTo = a.PeriodTo,

                        BusSupervisor = a.Supervisor != null
                        ? $"{a.Supervisor.FirstName} {a.Supervisor.MiddleName} {a.Supervisor.LastName}".Trim()
                        : "No Supervisor",

                        BuSupervisorId = a.SupervisorId ?? 0,

                        FullCapacity = a.TransportationVehicle?.Capacity ?? 0,

                        UserInRoute = usersInRoute?.Count ?? 0,
                        ExpectionNumFromOtherLines = exceptionNumFromOtherLines?.Count ?? 0,
                        RouteEmployeesToOtherLines = routeEmployeesToOtherLines?.Count ?? 0,

                        ActualCapacity = ((usersInRoute?.Count ?? 0) + (exceptionNumFromOtherLines?.Count ?? 0)) - (routeEmployeesToOtherLines?.Count ?? 0),
                        ActualUserAttedance = ActualUserAttedance?.Count() ?? 0,

                        DirectionNum = DirectionDb.Where(x => x.TransportationVehicleRouteId == a.Id).Count(),
                        FromoDate = a.FromoDate,
                        ToDate = a.ToDate,
                        christianNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == a.Id && x.HrUser != null && x.HrUser.MaritalStatusId == christianID).Count(),
                        MuslimNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == a.Id && x.HrUser != null && x.HrUser.MaritalStatusId == MuslimID).Count(),
                        TransportationVehicleId = a.TransportationVehicleId,
                        TransportationVehicleName = a.TransportationVehicle?.VehicleType?.Type ?? "N/A",
                        OneWay = a.OneWay ?? false,

                    };
                }).ToList();


                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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
        //public BaseResponseWithData<TransportationRouteDetailsVM> getTransportationRouteDetails(int RouteId ,DateTime FromDate ,DateTime ToDate)
        //{
        //    BaseResponseWithData<TransportationRouteDetailsVM> Response = new BaseResponseWithData<TransportationRouteDetailsVM>();
        //    Response.Data = new TransportationRouteDetailsVM();
        //    Response.Errors = new List<Error>();
        //    Response.Result = false;

        //    try
        //    {
        //        TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
        //        DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
        //        var Day = egyptTime.ToString("dddd");
        //        var FullDateTime = egyptTime;

        //            var Routes = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id == RouteId &&  x.Active == true, new[] { "Supervisor", "BranchSchedule", "TransportationVehicle" });

        //        var UserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => x. == RouteId && (((DateTime)x.CheckIn).Date >= FromDate.Date && ((DateTime)x.CheckIn).Date <= ToDate.Date) ||
        //                                                      ((DateTime)x.CheckOut).Date >= FromDate.Date && ((DateTime)x.CheckOut).Date <= ToDate.Date)).ToList();
        //        return new TransportationRouteDetailsVM
        //            {

        //                Id = a.Id,
        //                LineName = a.TransportationLine?.LineName ?? "N/A",
        //                LineCost = a.TransportationLine?.LineCost ?? 0,
        //                LineId = a.TransportationLineId,

        //                SupplierName = supplierDb.Where(x => x.Id == a.SupplierId).Select(y => y.Name).FirstOrDefault() ?? "No Supplier",
        //                SupplierId = a.SupplierId ?? 0,

        //                SupplierContactPersonName = supplierContactPersionDb.Where(x => x.Id == a.SupplierContactPersonId).Select(y => y.Name).FirstOrDefault() ?? "N/A",
        //                SupplierContactPersonId = a.SupplierContactPersonId ?? 0,

        //                branchScheduleId = a.BranchScheduleId ?? 0,
        //                ShiftNumber = a.BranchSchedule?.ShiftNumber ?? 0,

        //                Serial = a.Serial ?? "No Serial",

        //                PeriodFrom = a.PeriodFrom,
        //                PeriodTo = a.PeriodTo,

        //                BusSupervisor = a.Supervisor != null
        //                ? $"{a.Supervisor.FirstName} {a.Supervisor.MiddleName} {a.Supervisor.LastName}".Trim()
        //                : "No Supervisor",

        //                BuSupervisorId = a.SupervisorId ?? 0,

        //                FullCapacity = a.TransportationLine?.TransportationVehicle?.Capacity ?? 0,

        //                UserInRoute = usersInRoute?.Count ?? 0,
        //                ExpectionNumFromOtherLines = exceptionNumFromOtherLines?.Count ?? 0,
        //                RouteEmployeesToOtherLines = routeEmployeesToOtherLines?.Count ?? 0,

        //                ActualCapacity = ((usersInRoute?.Count ?? 0) + (exceptionNumFromOtherLines?.Count ?? 0)) - (routeEmployeesToOtherLines?.Count ?? 0),
        //                ActualUserAttedance = ActualUserAttedance?.Count() ?? 0,

        //                DirectionNum = DirectionDb.Where(x => x.TransportationVehicleRouteId == a.Id).Count(),
        //                FromoDate = a.FromoDate,
        //                ToDate = a.ToDate,
        //                christianNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == a.Id && x.HrUser.MaritalStatusId == christianID).Count(),
        //                MuslimNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == a.Id && x.HrUser.MaritalStatusId == MuslimID).Count(),
        //            };




        //        Response.Result = true;
        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Result = false;
        //        Error error = new Error();
        //        error.ErrorCode = "Err10";
        //        error.ErrorMSG = ex.InnerException.Message;
        //        Response.Errors.Add(error);
        //        return Response;
        //    }
        //}

        // TransportationVehicle
        public BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>> getAllTransportationVehicle(long RouteId, int transportationLineId, long supplierId, long supplierContactPersonId, DateTime? dateSearch, string serialBus, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>> Response = new BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>>();
            Response.Data = new List<getTransportationVehicleDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {

                var VehicleTypeDb = _unitOfWork.VehicleTypes.FindAll(x => x.Id > 0);
                var AllTransportationVehicles = _unitOfWork.TransportationVehicles.FindAll(x => x.Id > 0).AsQueryable();
                if (dateSearch != null)
                {
                    if (transportationLineId > 0 || supplierId > 0 || supplierContactPersonId > 0 || !string.IsNullOrEmpty(serialBus))
                    {
                        var vehicleRoutes = GetVehicleRoutes(RouteId, transportationLineId, supplierId, supplierContactPersonId, serialBus);
                        if (!vehicleRoutes.Any())
                        {
                            Response.Result = false;
                            Error error = new Error();
                            error.ErrorCode = "Err10";
                            error.ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر";
                            Response.Errors.Add(error);
                            return Response;
                        }
                        var TransportationVehicleIds = vehicleRoutes.Select(x => x.TransportationVehicleId).ToList();
                        AllTransportationVehicles = AllTransportationVehicles.Where(x => TransportationVehicleIds.Contains(x.Id));
                    }

                }
                var transportPagedList = PagedList<TransportationVehicle>.Create(AllTransportationVehicles, PageNo, NoOfItems);

                Response.Data = transportPagedList.Where(x => x.Id > 0).Select(a => new getTransportationVehicleDto
                {
                    Id = a.Id,
                    VehicleTypeId = a.VehicleTypeId,
                    Capacity = a.Capacity,
                    IsApproved = a.IsApproved,
                    ApprovedBy = a.ApprovedBy,
                    Active = a.Active,
                    CreationDate = a.CreationDate,
                    CreationBy = a.CreationBy,
                    ModifiedDate = a.ModifiedDate,
                    ModifiedBy = a.ModifiedBy,
                    VehicleTypeName = VehicleTypeDb.Where(x => x.Id == a.VehicleTypeId).Select(y => y.Type).FirstOrDefault()
                }).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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

        // VehicleType 
        public BaseResponseWithData<List<VehicleType>> getAllVehicleType()
        {
            BaseResponseWithData<List<VehicleType>> Response = new BaseResponseWithData<List<VehicleType>>();
            Response.Data = new List<VehicleType>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {

                Response.Result = true;
                Response.Data = _unitOfWork.VehicleTypes.FindAll(x => x.Id > 0).ToList();
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

        public BaseResponseWithData<List<transportionLineNameVn>> getAllTransportationLine(string? Name)
        {
            BaseResponseWithData<List<transportionLineNameVn>> Response = new BaseResponseWithData<List<transportionLineNameVn>>();
            Response.Data = new List<transportionLineNameVn>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var AllTransportationLinesDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0).ToList();
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = HttpUtility.UrlDecode(Name);
                    AllTransportationLinesDb = AllTransportationLinesDb.Where(a => a.LineName.Trim().Contains(Name.Trim())).ToList();

                }
                Response.Result = true;
                Response.Data = AllTransportationLinesDb.Select(x => new transportionLineNameVn
                {
                    Id = x.Id,
                    Name = x.LineName,
                    RouteNum = _unitOfWork.TransportationVehicleRoutes.FindAll(r => r.TransportationLineId == x.Id).Count()
                }).ToList();
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

        // get pagelist route 
        //public BaseResponseWithDataAndHeader<List<EmployeeBasicInfoVM>> GetTransportationEmployeeList(long RouteId, int PageNo, int NoOfItems)
        //{
        //    BaseResponseWithDataAndHeader<List<EmployeeBasicInfoVM>> Response = new BaseResponseWithDataAndHeader<List<EmployeeBasicInfoVM>>();
        //    Response.Data = new List<EmployeeBasicInfoVM>();
        //    Response.Errors = new List<Error>();
        //    Response.Result = false;

        //    try
        //    {
        //        if (RouteId <= 0)
        //        {
        //            // Response.Result = false;
        //            Error error = new Error();
        //            error.ErrorCode = "Err10";
        //            error.ErrorMSG = "add RouteId";
        //            Response.Errors.Add(error);
        //        }
        //        var DirecionDb = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.Id > 0);
        //        var TransportationEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.TransportationVehicleRouteId == RouteId, new[] { "HrUser" });
        //        var AllHrUsersInRoute = TransportationEmployees.Select(u => u.HrUser);
        //        var AllHrUsersPagedList = PagedList<HrUser>.Create(AllHrUsersInRoute, PageNo, NoOfItems);

        //        //Response.Data = AllHrUsersPagedList.Where(x => x.Id > 0).Select(a => new EmployeeBasicInfoVM
        //        //{    

        //        //    ID = a.Id,
        //        //    FirstNane = a.FirstName,
        //        //    MiddleName = a.MiddleName,
        //        //    LastName = a.LastName,
        //        //    Mobile = a.Mobile,
        //        //    Email = a.Email,
        //        //    Photo = a.ImgPath != null ? Globals.baseURL + "/" + a.ImgPath : null,
        //        //    Latitude = a.Latitude,
        //        //    Longtitud = a.Longtitud,
        //        //    DirectionName = DirecionDb.Where(x=>x.Id == (TransportationEmployees.Where(w=>w.HrUserId== a.Id).Select(r=>r.TransportationVehicleRouteDirectionId).FirstOrDefault())).Select(n=>n.RouteDirection).FirstOrDefault()

        //        //}).ToList();

        //        Response.Data = AllHrUsersPagedList.Where(x => x.Id > 0).Select(a =>
        //        {
        //            var employeeRoute = TransportationEmployees.FirstOrDefault(w => w.HrUserId == a.Id);
        //            return new EmployeeBasicInfoVM
        //            {
        //                ID = employeeRoute.Id,
        //                HrUserId = a.Id,
        //                FirstNane = a.FirstName,
        //                MiddleName = a.MiddleName,
        //                LastName = a.LastName,
        //                Mobile = a.Mobile,
        //                Email = a.Email,
        //                Photo = a.ImgPath != null ? Globals.baseURL + "/" + a.ImgPath : null,
        //                Latitude = a.Latitude,
        //                Longtitud = a.Longtitud,
        //                DirectionName = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.RouteDirection).FirstOrDefault(),
        //                DirectionId = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Id).FirstOrDefault(),
        //                DirectionLatitude = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Latitude).FirstOrDefault(),
        //                DirectionLongtitud = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Longtitud).FirstOrDefault(),

        //            };
        //        }).ToList();

        //        Response.PaginationHeader = new PaginationHeader
        //        {
        //            CurrentPage = PageNo,
        //            TotalPages = AllHrUsersPagedList.TotalPages,
        //            ItemsPerPage = NoOfItems,
        //            TotalItems = AllHrUsersPagedList.TotalCount
        //        };
        //        Response.Result = true;
        //        return Response;

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Result = false;
        //        Error error = new Error();
        //        error.ErrorCode = "Err10";
        //        error.ErrorMSG = ex.InnerException.Message;
        //        Response.Errors.Add(error);
        //        return Response;
        //    }
        //}
        public BaseResponseWithData<List<EmployeeBasicInfoVM>> GetTransportationEmployeeList(long RouteId)
        {
            BaseResponseWithData<List<EmployeeBasicInfoVM>> Response = new BaseResponseWithData<List<EmployeeBasicInfoVM>>();
            Response.Data = new List<EmployeeBasicInfoVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (RouteId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add RouteId";
                    Response.Errors.Add(error);
                }
                var DirecionDb = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.Id > 0);
                var TransportationEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.TransportationVehicleRouteId == RouteId, new[] { "HrUser" });
                var AllHrUsersInRoute = TransportationEmployees.Select(u => u.HrUser).ToList();
                // var AllHrUsersPagedList = PagedList<HrUser>.Create(AllHrUsersInRoute, PageNo, NoOfItems);

                //Response.Data = AllHrUsersPagedList.Where(x => x.Id > 0).Select(a => new EmployeeBasicInfoVM
                //{    

                //    ID = a.Id,
                //    FirstNane = a.FirstName,
                //    MiddleName = a.MiddleName,
                //    LastName = a.LastName,
                //    Mobile = a.Mobile,
                //    Email = a.Email,
                //    Photo = a.ImgPath != null ? Globals.baseURL + "/" + a.ImgPath : null,
                //    Latitude = a.Latitude,
                //    Longtitud = a.Longtitud,
                //    DirectionName = DirecionDb.Where(x=>x.Id == (TransportationEmployees.Where(w=>w.HrUserId== a.Id).Select(r=>r.TransportationVehicleRouteDirectionId).FirstOrDefault())).Select(n=>n.RouteDirection).FirstOrDefault()

                //}).ToList();

                Response.Data = AllHrUsersInRoute.Where(x => x.Id > 0).Select(a =>
                {
                    var employeeRoute = TransportationEmployees.FirstOrDefault(w => w.HrUserId == a.Id);
                    return new EmployeeBasicInfoVM
                    {
                        ID = employeeRoute.Id,
                        HrUserId = a.Id,
                        FirstNane = a.FirstName,
                        MiddleName = a.MiddleName,
                        LastName = a.LastName,
                        Mobile = a.Mobile,
                        Email = a.Email,
                        Photo = a.ImgPath != null ? Globals.baseURL + "/" + a.ImgPath : null,
                        Latitude = a.Latitude,
                        Longtitud = a.Longtitud,
                        DirectionName = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.RouteDirection).FirstOrDefault(),
                        DirectionId = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Id).FirstOrDefault(),
                        DirectionLatitude = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Latitude).FirstOrDefault(),
                        DirectionLongtitud = DirecionDb.Where(x => x.Id == employeeRoute.TransportationVehicleRouteDirectionId).Select(n => n.Longtitud).FirstOrDefault(),

                    };
                }).ToList();


                Response.Result = true;
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

        public BaseResponseWithData<List<TransportationDirectionByRouteIdVm>> GetTransportationDirection(long routeId)
        {
            var response = new BaseResponseWithData<List<TransportationDirectionByRouteIdVm>>
            {
                Data = new List<TransportationDirectionByRouteIdVm>(),
                Errors = new List<Error>(),
                Result = false
            };

            try
            {
                IQueryable<TransportationVehicleRouteDirection> query = _unitOfWork.TransportationVehicleRouteDirections.FindAllQueryable(x => x.Id > 0);

                if (routeId > 0)
                {
                    query = query.Where(x => x.TransportationVehicleRouteId == routeId);
                }

                response.Data = query.Select(a => new TransportationDirectionByRouteIdVm
                {
                    Id = a.Id,
                    RouteDirection = a.RouteDirection,
                    Description = a.Description,
                    Latitude = a.Latitude,
                    Longtitud = a.Longtitud
                }).ToList();

                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Errors.Add(new Error
                {
                    ErrorCode = "Err10",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });
                return response;
            }
        }

        public BaseResponseWithData<TransportationRouteVM> getTransportationRoute(long RouteId)
        {
            BaseResponseWithData<TransportationRouteVM> Response = new BaseResponseWithData<TransportationRouteVM>();
            // Response.Data = new TransportationRouteVM();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                #region validation 
                if (RouteId <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add RouteId";
                    Response.Errors.Add(error);
                    return Response;
                }
                #endregion
                var TransportationLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0).AsQueryable();
                var supplierDb = _unitOfWork.Suppliers.FindAll(x => x.Id > 0).AsQueryable();
                var supplierContactPersionDb = _unitOfWork.SupplierContactPeople.FindAll(x => x.Id > 0).AsQueryable();
                var TransportationRout = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == RouteId && x.Active == true, new[] { "Supervisor", "BranchSchedule", "TransportationVehicle.VehicleType", "TransportationLine" }).FirstOrDefault();
                var christianID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "C").Select(c => c.Id).FirstOrDefault();
                var MuslimID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "M").Select(c => c.Id).FirstOrDefault();
                var RoutesEmployesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                if (TransportationRout != null)
                {
                    Response.Data = new TransportationRouteVM
                    {
                        Id = TransportationRout.Id,
                        NameOfRoute = TransportationRout.NameOfRoute ?? null,
                        LineId = TransportationRout.TransportationLineId,
                        LineName = TransportationRout.TransportationLine.LineName ?? "N/A",
                        LineCost = TransportationRout.LineCost,
                        SupplierId = (long)TransportationRout.SupplierId,
                        SupplierName = supplierDb.Where(x => x.Id == TransportationRout.SupplierId).Select(y => y.Name).FirstOrDefault(),
                        SupplierContactPersonId = TransportationRout.SupplierContactPersonId ?? 0,
                        SupplierContactPersonName = supplierContactPersionDb.Where(x => x.Id == TransportationRout.SupplierContactPersonId).Select(y => y.Name).FirstOrDefault() ?? "N/A",
                        Serial = TransportationRout?.Serial ?? "",

                        branchScheduleId = TransportationRout?.BranchScheduleId ?? 0,

                        ShiftNumber = TransportationRout?.BranchSchedule?.ShiftNumber ?? 0,

                        PeriodFrom = TransportationRout.PeriodFrom,
                        PeriodTo = TransportationRout?.PeriodTo,

                        BusSupervisor =
                                        (TransportationRout?.Supervisor?.FirstName ?? "") + " " +
                                        (TransportationRout?.Supervisor?.MiddleName ?? "") + " " +
                                        (TransportationRout?.Supervisor?.LastName ?? ""),

                        BuSupervisorId = TransportationRout?.SupervisorId ?? 0,
                        FromoDate = TransportationRout.FromoDate,
                        ToDate = TransportationRout.ToDate,
                        christianNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == TransportationRout.Id && x.HrUser.MaritalStatusId == christianID).Count(),
                        MuslimNum = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == TransportationRout.Id && x.HrUser.MaritalStatusId == MuslimID).Count(),
                        TransportationVehicleId = TransportationRout.TransportationVehicleId,
                        TransportationVehicleName = TransportationRout.TransportationVehicle.VehicleType.Type ?? "N/A",
                        OneWay = TransportationRout.OneWay,
                    };
                }
                Response.Result = true;
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

        public BaseResponseWithData<List<TransportationRouteVM>> getTransportationRouteByHrUser(long HrUserId)
        {
            BaseResponseWithData<List<TransportationRouteVM>> Response = new BaseResponseWithData<List<TransportationRouteVM>>();
            Response.Data = new List<TransportationRouteVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {

                var TransportationLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0);
                var supplierDb = _unitOfWork.Suppliers.FindAll(x => x.Id > 0);
                var supplierContactPersionDb = _unitOfWork.SupplierContactPeople.FindAll(x => x.Id > 0).AsQueryable();
                var TransportationRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.SupervisorId == HrUserId && x.Active == true, new[] { "Supervisor", "BranchSchedule", "TransportationVehicle.VehicleType" }).ToList();

                // var data = new TransportationRouteVM();

                Response.Data = TransportationRoute.Select(a => new TransportationRouteVM
                {
                    Id = a.Id,
                    LineName = TransportationLineDb.Where(x => x.Id == a.TransportationLineId).Select(n => n.LineName).FirstOrDefault(),
                    NameOfRoute = a.NameOfRoute,
                    LineId = a.TransportationLineId,
                    SupplierName = supplierDb.Where(x => x.Id == a.SupplierId).Select(y => y.Name).FirstOrDefault(),
                    SupplierId = (long)a.SupplierId,
                    SupplierContactPersonName = supplierContactPersionDb.Where(x => x.Id == a.SupplierContactPersonId).Select(y => y.Name).FirstOrDefault(),
                    SupplierContactPersonId = (long)a.SupplierContactPersonId,
                    branchScheduleId = (long)a.BranchScheduleId,
                    ShiftNumber = a.BranchSchedule.ShiftNumber,
                    Serial = a.Serial,
                    PeriodFrom = a.PeriodFrom,
                    PeriodTo = a.PeriodTo,
                    BusSupervisor = a.Supervisor.FirstName + " " + a.Supervisor.MiddleName + " " + a.Supervisor.LastName,
                    BuSupervisorId = (long)a.SupervisorId,
                    BusType = a.TransportationVehicle.VehicleType.Type
                }).ToList();



                Response.Result = true;
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

        // update Modify of line  by super admin
        public async Task<BaseResponse> ModifyPriceOfTransportationLine(ModifyTransportationLinePriceVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.IncreaseCost <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add IncreaseCost";
                    Response.Errors.Add(error);
                }
                var TransportationLineIncreaseRequest = new TransportationLineIncreaseRequest();
                //if (dto.IsPrecent == true)
                //{
                //    Error error = new Error();
                //    error.ErrorCode = "Err10";
                //    error.ErrorMSG = "enter correct Id";
                //    Response.Errors.Add(error);
                //}

                TransportationLineIncreaseRequest.IsPercent = dto.IsPrecent;
                TransportationLineIncreaseRequest.ForAllLines = dto.ForAllLines;
                TransportationLineIncreaseRequest.ApproximateToFiveFlag = dto.ApproximateToFiveFlag;
                TransportationLineIncreaseRequest.CreationBy = validation.userID;
                TransportationLineIncreaseRequest.CreationDate = DateTime.Now;
                TransportationLineIncreaseRequest.IncreaseCost = dto.IncreaseCost;
                TransportationLineIncreaseRequest.Approve = false;
                TransportationLineIncreaseRequest.StartDate = dto.StartDate;
                _unitOfWork.TransportationLineIncreaseRequests.Add(TransportationLineIncreaseRequest);
                _unitOfWork.Complete();

                var resuilt = 0;
                if (dto.ForAllLines == false && dto.TransportationLineIds != null)
                {

                    var ALLTransportationLineIncreaseRequestLines = new List<TransportationLineIncreaseRequestLine>();

                    foreach (var item in dto.TransportationLineIds)
                    {
                        var TransportationLineIncreaseRequestLine = new TransportationLineIncreaseRequestLine();

                        TransportationLineIncreaseRequestLine.TransportationLineIncreaseRequestId = TransportationLineIncreaseRequest.Id;
                        TransportationLineIncreaseRequestLine.RouteId = item;
                        ALLTransportationLineIncreaseRequestLines.Add(TransportationLineIncreaseRequestLine);
                    }
                    _unitOfWork.TransportationLineIncreaseRequestLines.AddRange(ALLTransportationLineIncreaseRequestLines);
                    resuilt = _unitOfWork.Complete();

                }
                if (resuilt > 0)
                {
                    var RoleId = _unitOfWork.Roles.FindAll(x => x.Name == "Transportation Super Admin").Select(r => r.Id).FirstOrDefault();
                    var UserRoles = _unitOfWork.UserRoles.FindAllQueryable(x => x.RoleId == RoleId, new[] { "User" }).Select(u => u.User);
                    foreach (var item in UserRoles)
                    {
                        _notificationService.CreateNotification(validation.userID, "تغيير اسعار الخطوط", "Need Approval", "#", true, item.Id, 0);
                        await _mailService.SendMail(new MailData() { EmailToName = item.Email, EmailToId = item.Email, EmailSubject = "تغيير اسعار الخطوط", EmailBody = "Need Approval" });

                    }

                }



                Response.Result = true;
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

        // Reject UpdatePrice
        public BaseResponse RejectUpdatePrice(int Id)
        {
            var response = new BaseResponse
            {
                Errors = new List<Error>(),
                Result = false
            };

            try
            {
                if (Id <= 0)
                {
                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err10",
                        ErrorMSG = "Invalid or missing parameter."
                    });
                    return response;
                }

                var increaseRequest = _unitOfWork.TransportationLineIncreaseRequests.FindAll(x => x.Id == Id).FirstOrDefault();
                if (increaseRequest == null)
                {
                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err11",
                        ErrorMSG = "Request not found."
                    });
                    return response;
                }

                increaseRequest.Approve = false;
                increaseRequest.ApprovedDate = DateTime.Now;
                increaseRequest.ApprovedBy = validation.userID;
                _unitOfWork.TransportationLineIncreaseRequests.Update(increaseRequest);
                _unitOfWork.Complete();
                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error
                {
                    ErrorCode = "Err12",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });
                return response;
            }
        }

        //        // Create excel file on physical disk  
        //        Directory.CreateDirectory(savedPath);
        //        var dateNow = DateTime.Now.ToString("yyyyMMddHHssFFF");
        //        var excelPath = Path.Combine(savedPath, $"AttendanceReport.xlsx");
        //        excel.SaveAs(new FileInfo(excelPath));
        public BaseResponseWithData<string> ReportCostOfLinesExcell(int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? date, DateTime? dateFrom, DateTime? dateTo, string serialBus, int PageNo, int NoOfItems)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {

                // get all CostOfLines
                var Alldata = new List<CostOfTransportLineVM>();
                var data = new CostOfTransportLineVM();
                int countOfRounds = 0;
                int countOfCheckIn = 0;
                int countOfCheckOut = 0;
                int DeductRoundNum = 0;
                decimal DeductPerRound = 0;
                var transportionAccount = _unitOfWork.TransportationVehicleRouteAccounts.FindAllQueryable(x => x.Id > 0);
                var transportionlineRouteDb = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine", "Supplier" });
                var transprotationBusAttedance = _unitOfWork.TransprotationUserAttedances.FindAllQueryable(x => x.Type == "Bus");
                var transprotationBusAbsence = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => x.Id > 0);
                var roundRoutesDb = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => true, new[] { "TransportationVehicleRoute" });

                if (TransportionlineId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.TransportationLineId == TransportionlineId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }


                if (SupplierId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierId == SupplierId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }
                if (supplierContactPersonId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId && x.Active == true, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }

                if (!string.IsNullOrEmpty(serialBus))
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.Active == true && x.Serial == serialBus, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));


                }
                if (supplierContactPersonId > 0)
                {
                    var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active == true && x.Active == true && x.SupplierContactPersonId == supplierContactPersonId, new[] { "TransportationLine" });
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    transportionAccount = transportionAccount.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId));

                }
                if (date != null)
                {

                    transportionAccount = transportionAccount.Where(x => x.DateOfMonth.Year == date.Value.Year && x.DateOfMonth.Month == date.Value.Month);

                }
                if (dateFrom != null && dateTo != null)
                {
                    transportionAccount = transportionAccount.Where(x => x.DateOfMonth.Date >= dateFrom.Value.Date && x.DateOfMonth.Date <= dateTo.Value.Date).Distinct();

                }

                // var PagedList = PagedList<TransportationVehicleRouteAccount>.Create(transportionAccount, PageNo, NoOfItems);

                Alldata = transportionAccount.OrderBy(x => x.Serial).ToList().Select(u =>
                {
                    decimal totalCompleteRoundsDue = 0;
                    IQueryable<TransportationRoundForRoute> currentRounds = roundRoutesDb.Where(x => false);

                    var route = transportionlineRouteDb.FirstOrDefault(x => x.Id == u.TransportationVehicleRouteId);

                    var lineCost = route.LineCost;
                    var isOneWay = route.OneWay ?? false;

                    if (route == null) return null;
                    if (date != null && !isOneWay)
                    {
                        currentRounds = roundRoutesDb.Where(x => x.TransportationVehicleRouteId == u.TransportationVehicleRouteId &&
                                    ((x.CheckInDate.HasValue && x.CheckInDate.Value.Date == date.Value.Date && x.CheckInDate.Value.Year == date.Value.Year) ||
                                     (x.CheckOutDate.HasValue && x.CheckOutDate.Value.Date == date.Value.Date && x.CheckOutDate.Value.Year == date.Value.Year)));


                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date == date.Value.Date)) && x.Serial == route.Serial).Select(x => x.Id)
                            .Distinct()
                            .Count();
                        countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date == date.Value.Date)) && x.Serial == route.Serial).Select(x => x.Id)
                            .Distinct()
                            .Count();
                        countOfRounds = countOfCheckIn + countOfCheckOut;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Count();

                    }
                    if (dateFrom != null && dateTo != null && !isOneWay)
                    {
                        currentRounds = roundRoutesDb.Where(x => x.TransportationVehicleRouteId == u.TransportationVehicleRouteId &&
                                     ((x.CheckInDate.HasValue && x.CheckInDate.Value.Date >= dateFrom.Value.Date && x.CheckInDate.Value.Date <= dateTo.Value.Date && x.CheckInDate.Value.Year == dateFrom.Value.Year) ||
                                      (x.CheckOutDate.HasValue && x.CheckOutDate.Value.Date >= dateFrom.Value.Date && x.CheckOutDate.Value.Date <= dateTo.Value.Date && x.CheckOutDate.Value.Year == dateFrom.Value.Year)));


                        countOfCheckIn = transprotationBusAttedance
                            .Where(x => x.OneWayDate.HasValue
                                     && x.OneWayDate.Value.Date >= dateFrom.Value.Date
                                     && x.OneWayDate.Value.Date <= dateTo.Value.Date
                                     && x.Serial == route.Serial)
                            .Select(x => x.Id)
                            .Distinct()
                            .Count();
                        countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date >= dateFrom.Value.Date && x.CheckOut.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).Select(x => x.Id)
                            .Distinct()
                            .Count();
                        countOfRounds = (countOfCheckIn + countOfCheckOut) / 2;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Count();
                    }


                    if (date != null && isOneWay)
                    {
                        currentRounds = roundRoutesDb.Where(x => x.TransportationVehicleRouteId == u.TransportationVehicleRouteId &&
                                    ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date == date.Value.Date && x.OneWayDate.Value.Year == date.Value.Year)));



                        countOfCheckIn = transprotationBusAttedance.Where(x => ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date == date.Value.Date)) && x.Serial == route.Serial).Select(x => x.Id)
                            .Distinct()
                            .Count();

                        countOfRounds = countOfCheckIn;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date == date.Value.Date && x.Serial == route.Serial).Count();

                    }
                    if (dateFrom != null && dateTo != null && isOneWay)
                    {

                        currentRounds = roundRoutesDb.Where(x => x.TransportationVehicleRouteId == u.TransportationVehicleRouteId &&
                                   ((x.OneWayDate.HasValue && x.OneWayDate.Value.Date >= dateFrom.Value.Date
                                       && x.OneWayDate.Value.Date <= dateTo.Value.Date
                                       && x.OneWayDate.Value.Year == dateFrom.Value.Year)));



                        countOfCheckIn = transprotationBusAttedance
                            .Where(x => x.OneWayDate.HasValue
                                     && x.OneWayDate.Value.Date >= dateFrom.Value.Date
                                     && x.OneWayDate.Value.Date <= dateTo.Value.Date
                                     && x.Serial == route.Serial)
                            .Select(x => x.Id)
                            .Distinct()
                            .Count();
                        //countOfCheckOut = transprotationBusAttedance.Where(x => ((x.CheckOut.HasValue && x.CheckOut.Value.Date >= dateFrom.Value.Date && x.CheckOut.Value.Date <= dateTo.Value.Date)) && x.Serial == route.Serial).Select(x => x.Id)
                        //    .Distinct()
                        //    .Count();
                        countOfRounds = countOfCheckIn;
                        DeductPerRound = (decimal)transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Sum(r => r.DeductPerRound);
                        DeductRoundNum = transprotationBusAbsence.Where(x => x.DateOfDeduction.Date >= dateFrom.Value.Date && x.DateOfDeduction.Date <= dateTo.Value.Date && x.Serial == route.Serial).Count();
                    }

                    if (currentRounds.Any())
                    {
                        foreach (var round in currentRounds)
                        {
                            totalCompleteRoundsDue = totalCompleteRoundsDue + CalculatePricePerRouteFromExceptionPrice((long)round.TransportationVehicleRouteId, (DateTime)(round.CheckInDate ?? round.CheckOutDate ?? round.OneWayDate));
                        }
                    }

                    return new CostOfTransportLineVM
                    {
                        LineName = route.TransportationLine?.LineName,
                        NameOfRoute = route.NameOfRoute,
                        Serial = route.Serial,
                        LineCost = lineCost,
                        SupplierName = route.Supplier?.Name,
                        OneWay = isOneWay,
                        CountOfround = countOfRounds,
                        DeductRoundNum = DeductRoundNum,
                        DeductPerRound = DeductPerRound,
                        NetCost = (decimal)(isOneWay
                            ? ((lineCost * countOfRounds) - (DeductPerRound))
                            : ((lineCost) * countOfRounds) - (DeductPerRound))
                    };
                }).Where(x => x != null).ToList();

                // excell 


                // CLIENTS
                var client = _unitOfWork.Clients.FindAll(x => x.OwnerCoProfile == true).FirstOrDefault();
                var clientPhone = _unitOfWork.ClientPhones.FindAll(x => x.ClientId == client.Id).Select(y => y.Phone).FirstOrDefault();
                var clientAddress = _unitOfWork.ClientAddresses.FindAll(x => x.ClientId == client.Id).FirstOrDefault();
                var clientCountry = _unitOfWork.Countries.FindAll(x => x.Id == clientAddress.CountryId).Select(y => y.Name).FirstOrDefault();
                var clientgoverment = _unitOfWork.Governorates.FindAll(x => x.Id == clientAddress.GovernorateId).Select(y => y.Name).FirstOrDefault();
                var createdUser = _unitOfWork.Users.FindAll(x => x.Id == client.CreatedBy).FirstOrDefault();
                // ACCOUNTS
                var accounts = _unitOfWork.Accounts.FindAll(x => x.ParentCategory != 0);
                var accountsNames = accounts.Select(x => x.AccountName);

                int i = 1;
                var transportionLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0);

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage excel = new ExcelPackage();
                if (Alldata.Count() > 0)
                {

                    string monthName = new DateTime(2000, i, 1).ToString("MMMM", new CultureInfo("ar-EG"));



                    var sheet = excel.Workbook.Worksheets.Add($"تقرير الاسعار");

                    sheet.Cells[5, 1].Value = $"كشف حساب الخطوط  ";
                    sheet.Cells[5, 1, 5, 7].Merge = true;
                    sheet.Cells[5, 1, 5, 3].Style.Font.Bold = true;

                    sheet.Cells[5, 1, 5, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // Adjust column width if necessary


                    //sheet.Cells[6, 3].Value = $"  {Alldata.LineName} تقرير السعة اليومي لخط   ";
                    sheet.Cells[6, 2].Value = $"بتاريخ";

                    if (data != null)
                        sheet.Cells[6, 1].Value = $" {date?.ToString("yyyy:MMMM:dd")} ";
                    if (dateFrom != null && dateTo != null)
                        //sheet.Cells[6, 1].Value = $" {dateFrom?.ToString("yyyy:MMMM:dd")} الي {dateTo?.ToString("yyyy:MMMM:dd")} ";
                        // In your Excel generation code (where you're creating the sheet):
                        sheet.Cells[6, 1].Value = $"من {dateFrom?.ToString("yyyy:MMMM:dd", new CultureInfo("ar-EG"))} إلى {dateTo?.ToString("yyyy:MMMM:dd", new CultureInfo("ar-EG"))}";

                    sheet.Cells[6, 3].Style.Font.Bold = true;
                    sheet.Cells[6, 2].Style.Font.Bold = true;
                    sheet.Cells[6, 1].Style.Font.Bold = true;

                    sheet.Cells[6, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells[1, 3].Value = $"{client.Name}";
                    //sheet.Cells[1, 7, 1, 10].Merge = true;
                    sheet.Cells[4, 3].Value = $" TEL : {clientPhone}";

                    //sheet.Cells[2, 7, 2, 10].Merge = true;

                    sheet.Cells[2, 3].Value = $"{clientCountry}, {clientgoverment}";
                    //sheet.Cells[3, 7, 3, 10].Merge = true;

                    sheet.Cells[3, 3].Value = $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.AreaId}";
                    // sheet.Cells[4, 7, 4, 10].Merge = true;

                    //sheet.Cells[1, 7, 3, 10].Merge = true;

                    //sheet.Cells[1, 7].Value = $"{client.Name} \n" +$"{clientPhone }\n" + $"{clientCountry}, {clientgoverment} " + $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.Area}";


                    sheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //sheet.Cells[1, 7, 3, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Optionally format the header cells
                    //sheet.Cells[1, 1, 5, 1].Style.Font.Bold = true; // Make the text bold
                    //sheet.Cells[1, 1, 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    // Adjust column width to fit the content
                    // Assuming a total width equivalent to a typical A4 page (approximately 100 units)
                    double totalPageWidth = 90;
                    double columnWidth = totalPageWidth / 3; // Divide equally into 3 columns

                    // Set the width for each column
                    sheet.Column(1).Width = columnWidth; // البيان
                    sheet.Column(2).Width = columnWidth; // دائن
                    sheet.Column(3).Width = columnWidth; // مدين
                    sheet.Column(4).Width = columnWidth; // مدين
                    sheet.Column(5).Width = columnWidth; // مدين
                    sheet.Column(6).Width = columnWidth; // مدين
                    sheet.Column(7).Width = columnWidth; // مدين


                    //sheet.HeaderFooter.OddHeader.RightAlignedText = client.Name;
                    //sheet.HeaderFooter.EvenHeader.RightAlignedText = client.Name;
                    sheet.HeaderFooter.EvenFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.EvenFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.EvenFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.OddFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.OddFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.OddFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.FirstFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.FirstFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.FirstFooter.RightAlignedText = $"{createdUser.FirstName}  {createdUser.LastName}";


                    sheet.PrinterSettings.Orientation = eOrientation.Landscape;



                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        // Build physical path from relative URL
                        string relativeImagePath = client.LogoUrl.Replace("/", Path.DirectorySeparatorChar.ToString());
                        string imagePath = Path.Combine(_environment.WebRootPath, relativeImagePath);

                        // Check if file exists
                        if (File.Exists(imagePath) && i == 1)
                        {

                            using (Image originalImage = Image.FromFile(imagePath))
                            {
                                // Define target dimensions
                                int targetWidth = 64 * 4;
                                int targetHeight = 60;

                                // Calculate aspect ratio
                                float aspectRatio = (float)originalImage.Width / originalImage.Height;
                                int newWidth, newHeight;

                                if (targetWidth / (float)targetHeight > aspectRatio)
                                {
                                    newHeight = targetHeight;
                                    newWidth = (int)(targetHeight * aspectRatio);
                                }
                                else
                                {
                                    newWidth = targetWidth;
                                    newHeight = (int)(targetWidth / aspectRatio);
                                }

                                // Resize the image
                                using (Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero))
                                {
                                    string tempImagePath = Path.Combine(_environment.WebRootPath, "img", "scaled.jpg");

                                    // Ensure directory exists
                                    Directory.CreateDirectory(Path.GetDirectoryName(tempImagePath));

                                    if (i == 1)
                                    {
                                        if (File.Exists(tempImagePath))
                                        {
                                            // File.Delete(tempImagePath);
                                        }

                                        // resizedImage.Save(tempImagePath);
                                    }

                                    FileInfo resizedImageFile = new FileInfo(tempImagePath);

                                    var picture = sheet.Drawings.AddPicture("MyPicture", resizedImageFile);
                                    picture.SetPosition(0, 0, 0, 0);
                                    picture.SetSize(newWidth, newHeight);
                                }
                            }

                        }
                    }






                    // Set column headers
                    // sheet.Cells [1 , 1].Value="No Row";
                    sheet.Cells[8, 1].Value = "SupplierName";
                    sheet.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(50, 205, 50));

                    // sheet.Cells[7, 2, 7, 3].Merge = true;
                    //sheet.Cells[7, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    sheet.Cells[8, 2].Value = "Serial";
                    sheet.Cells[8, 3].Value = "Name ";
                    sheet.Cells[8, 4].Value = "LineCost ";
                    sheet.Cells[8, 5].Value = "CountOfrounds";
                    sheet.Cells[8, 6].Value = "DeductRoundNum";
                    sheet.Cells[8, 7].Value = "DeductPerRound";
                    sheet.Cells[8, 8].Value = "NetCost";





                    int y = 9;
                    int row = 0;
                    if (Alldata.Count > 0)
                    {
                        foreach (var u in Alldata)
                        {
                            sheet.Row(y).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            sheet.Cells[y, 1].Value = u.SupplierName;
                            sheet.Cells[y, 2].Value = u.Serial;
                            sheet.Cells[y, 3].Value = u.NameOfRoute;
                            sheet.Cells[y, 4].Value = u.LineCost;
                            sheet.Cells[y, 5].Value = u.CountOfround;
                            sheet.Cells[y, 6].Value = u.DeductRoundNum;
                            sheet.Cells[y, 7].Value = u.DeductPerRound;
                            sheet.Cells[y, 8].Value = u.NetCost;

                            y++;

                        }
                        row = y;

                    }







                    //var totaldain = 1000;
                    //var totalmaden = 1200;
                    //if (row >= 11)
                    //{
                    //    sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //    sheet.Cells[row, 1].Value = "الاجمالي";
                    //    sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    sheet.Cells[row, 2].Value = totaldain;
                    //    sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    sheet.Cells[row, 3].Value = totalmaden;
                    //    sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

                    //    row = row + 1;
                    //}

                    //if (row >= 12)
                    //{
                    //    sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    //    sheet.Cells[row, 2].Value = totaldain - totalmaden == 0 ? true : false;
                    //    sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

                    //    sheet.Cells[row, 3].Value = totalmaden - totalmaden;
                    //    sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

                    //}





                    sheet.Cells.Style.Font.Name = "Arial";
                    sheet.Cells.Style.Font.Size = 8;



                    sheet.PrinterSettings.Scale = 150;
                    sheet.PrinterSettings.FitToHeight = 0;
                    sheet.PrinterSettings.BottomMargin = 0.5M;
                    sheet.PrinterSettings.TopMargin = 0;
                    sheet.PrinterSettings.LeftMargin = 0;
                    sheet.PrinterSettings.RightMargin = 0;

                    sheet.PrinterSettings.PaperSize = ePaperSize.A4;


                    sheet.Row(5).Style.Font.Bold = true;
                    // sheet.Cells[1, 4, 1, 5].Style.Font.Bold = true;
                    //sheet.Cells[1, 4, 1, 5].Style.Font.Size = 9;
                    sheet.Cells[5, 1, 5, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[5, 1, 5, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;


                    sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                    sheet.Cells[8, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));

                    sheet.Cells[8, 3].Style.Font.Bold = true;
                    sheet.Cells[8, 2].Style.Font.Bold = true;
                    sheet.Cells[8, 1].Style.Font.Bold = true;
                    sheet.Cells[8, 4].Style.Font.Bold = true;
                    sheet.Cells[8, 5].Style.Font.Bold = true;
                    sheet.Cells[8, 6].Style.Font.Bold = true;
                    sheet.Cells[8, 7].Style.Font.Bold = true;
                    sheet.Cells[8, 8].Style.Font.Bold = true;


                    sheet.Row(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Save the Excel file

                    i++;




                }
                else
                {

                    var sheet = excel.Workbook.Worksheets.Add($"{"LineName"} ");

                    sheet.Cells[5, 1].Value = $"كشف حساب الخطوط  ";
                    sheet.Cells[5, 1, 5, 7].Merge = true;
                    sheet.Cells[5, 1, 5, 3].Style.Font.Bold = true;

                    sheet.Cells[5, 1, 5, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // Adjust column width if necessary


                    sheet.Cells[6, 3].Value = $"   تقرير السعة اليومي لخط   ";
                    sheet.Cells[6, 2].Value = $"بتاريخ";

                    if (data != null)
                        sheet.Cells[6, 1].Value = $" {date?.ToString("yyyy:MMMM:dd")} ";
                    if (dateFrom != null && dateTo != null)
                        //sheet.Cells[6, 1].Value = $" {dateFrom?.ToString("yyyy:MMMM:dd")} الي {dateTo?.ToString("yyyy:MMMM:dd")} ";
                        // In your Excel generation code (where you're creating the sheet):
                        sheet.Cells[6, 1].Value = $"من {dateFrom?.ToString("yyyy:MMMM:dd", new CultureInfo("ar-EG"))} إلى {dateTo?.ToString("yyyy:MMMM:dd", new CultureInfo("ar-EG"))}";

                    sheet.Cells[6, 3].Style.Font.Bold = true;
                    sheet.Cells[6, 2].Style.Font.Bold = true;
                    sheet.Cells[6, 1].Style.Font.Bold = true;

                    sheet.Cells[6, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[6, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells[1, 3].Value = $"{client.Name}";
                    //sheet.Cells[1, 7, 1, 10].Merge = true;
                    sheet.Cells[4, 3].Value = $" TEL : {clientPhone}";

                    //sheet.Cells[2, 7, 2, 10].Merge = true;

                    sheet.Cells[2, 3].Value = $"{clientCountry}, {clientgoverment}";
                    //sheet.Cells[3, 7, 3, 10].Merge = true;

                    sheet.Cells[3, 3].Value = $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.AreaId}";
                    // sheet.Cells[4, 7, 4, 10].Merge = true;

                    //sheet.Cells[1, 7, 3, 10].Merge = true;

                    //sheet.Cells[1, 7].Value = $"{client.Name} \n" +$"{clientPhone }\n" + $"{clientCountry}, {clientgoverment} " + $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.Area}";


                    sheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //sheet.Cells[1, 7, 3, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Optionally format the header cells
                    //sheet.Cells[1, 1, 5, 1].Style.Font.Bold = true; // Make the text bold
                    //sheet.Cells[1, 1, 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    // Adjust column width to fit the content
                    // Assuming a total width equivalent to a typical A4 page (approximately 100 units)
                    double totalPageWidth = 90;
                    double columnWidth = totalPageWidth / 3; // Divide equally into 3 columns

                    // Set the width for each column
                    sheet.Column(1).Width = columnWidth; // البيان
                    sheet.Column(2).Width = columnWidth; // دائن
                    sheet.Column(3).Width = columnWidth; // مدين
                    sheet.Column(4).Width = columnWidth; // مدين
                    sheet.Column(5).Width = columnWidth; // مدين
                    sheet.Column(6).Width = columnWidth; // مدين
                    sheet.Column(7).Width = columnWidth; // مدين


                    //sheet.HeaderFooter.OddHeader.RightAlignedText = client.Name;
                    //sheet.HeaderFooter.EvenHeader.RightAlignedText = client.Name;
                    sheet.HeaderFooter.EvenFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.EvenFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.EvenFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.OddFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.OddFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.OddFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
                    sheet.HeaderFooter.FirstFooter.CenteredText = "Page &P of &N ";
                    sheet.HeaderFooter.FirstFooter.LeftAlignedText = $"{DateTime.Now}";
                    sheet.HeaderFooter.FirstFooter.RightAlignedText = $"{createdUser.FirstName}  {createdUser.LastName}";


                    sheet.PrinterSettings.Orientation = eOrientation.Landscape;



                    if (!string.IsNullOrEmpty(client.LogoUrl))
                    {
                        // Build physical path from relative URL
                        string relativeImagePath = client.LogoUrl.Replace("/", Path.DirectorySeparatorChar.ToString());
                        string imagePath = Path.Combine(_environment.WebRootPath, relativeImagePath);

                        // Check if file exists
                        if (File.Exists(imagePath) && i == 1)
                        {

                            using (Image originalImage = Image.FromFile(imagePath))
                            {
                                // Define target dimensions
                                int targetWidth = 64 * 4;
                                int targetHeight = 60;

                                // Calculate aspect ratio
                                float aspectRatio = (float)originalImage.Width / originalImage.Height;
                                int newWidth, newHeight;

                                if (targetWidth / (float)targetHeight > aspectRatio)
                                {
                                    newHeight = targetHeight;
                                    newWidth = (int)(targetHeight * aspectRatio);
                                }
                                else
                                {
                                    newWidth = targetWidth;
                                    newHeight = (int)(targetWidth / aspectRatio);
                                }

                                // Resize the image
                                using (Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero))
                                {
                                    string tempImagePath = Path.Combine(_environment.WebRootPath, "img", "scaled.jpg");

                                    // Ensure directory exists
                                    Directory.CreateDirectory(Path.GetDirectoryName(tempImagePath));

                                    if (i == 1)
                                    {
                                        if (File.Exists(tempImagePath))
                                        {
                                            File.Delete(tempImagePath);
                                        }

                                        resizedImage.Save(tempImagePath);
                                    }

                                    FileInfo resizedImageFile = new FileInfo(tempImagePath);

                                    var picture = sheet.Drawings.AddPicture("MyPicture", resizedImageFile);
                                    picture.SetPosition(0, 0, 0, 0);
                                    picture.SetSize(newWidth, newHeight);
                                }
                            }

                        }
                    }






                    // Set column headers
                    // sheet.Cells [1 , 1].Value="No Row";
                    sheet.Cells[8, 1].Value = "Name";
                    sheet.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(50, 205, 50));

                    // sheet.Cells[7, 2, 7, 3].Merge = true;
                    //sheet.Cells[7, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    sheet.Cells[8, 2].Value = "LineCost ";
                    sheet.Cells[8, 3].Value = "OneWay";
                    sheet.Cells[8, 4].Value = "CountOfrounds";
                    sheet.Cells[8, 5].Value = "DeductRoundNum";
                    sheet.Cells[8, 6].Value = "DeductPerRound";
                    sheet.Cells[8, 7].Value = "SupplierName";
                    sheet.Cells[8, 8].Value = "NetCost";

                }

                var path = $"TransportLineCostExcel";
                var savedPath = Path.Combine(_environment.WebRootPath, path);
                if (System.IO.File.Exists(savedPath))
                    System.IO.File.Delete(savedPath);

                // Create excel file on physical disk  
                Directory.CreateDirectory(savedPath);
                var dateNow = DateTime.Now.ToString("yyyyMMddHHssFFF");
                var excelPath = Path.Combine(savedPath, $"ReportCostOfLinesExcell.xlsx");
                excel.SaveAs(new FileInfo(excelPath));

                var filePath = Globals.baseURL + '\\' + path + $"\\ReportCostOfLinesExcell.xlsx";
                Response.Data = filePath;

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


        // update price of line  by super admin
        public BaseResponse UpdatePriceOfTransportationLine(int Id)
        {
            var response = new BaseResponse
            {
                Errors = new List<Error>(),
                Result = false
            };

            try
            {
                if (Id <= 0)
                {
                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err10",
                        ErrorMSG = "Invalid or missing parameter."
                    });
                    return response;
                }

                var increaseRequest = _unitOfWork.TransportationLineIncreaseRequests.FindAll(x => x.Id == Id).FirstOrDefault();
                if (increaseRequest == null)
                {
                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err11",
                        ErrorMSG = "Request not found."
                    });
                    return response;
                }

                increaseRequest.Approve = true;
                increaseRequest.ApprovedDate = DateTime.Now;
                increaseRequest.ApprovedBy = validation.userID;
                _unitOfWork.TransportationLineIncreaseRequests.Update(increaseRequest);

                IEnumerable<TransportationVehicleRoute> linesToUpdate;

                if (increaseRequest.ForAllLines)
                {
                    linesToUpdate = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id > 0);
                }
                else
                {
                    var selectedIds = _unitOfWork.TransportationLineIncreaseRequestLines
                                                 .FindAll(x => x.TransportationLineIncreaseRequestId == Id)
                                                 .Select(x => x.RouteId)
                                                 .ToList();

                    linesToUpdate = _unitOfWork.TransportationVehicleRoutes.FindAll(x => selectedIds.Contains(x.Id) && x.Active == true);
                }

                foreach (var line in linesToUpdate)
                {
                    decimal newCost;

                    if (!increaseRequest.IsPercent)
                    {
                        var total = line.LineCost + increaseRequest.IncreaseCost;
                        newCost = increaseRequest.ApproximateToFiveFlag
                                 ? CustomApproximateToFive(total)
                                 : total;
                    }
                    else
                    {
                        var increaseValue = (line.LineCost * increaseRequest.IncreaseCost) / 100;
                        var total = line.LineCost + increaseValue;

                        newCost = increaseRequest.ApproximateToFiveFlag
                                 ? CustomApproximateToFive(total)
                                 : total;
                    }

                    line.LineCost = newCost;
                    _unitOfWork.TransportionLineExceptionPrice.Add(new TransportionLineExceptionPrice
                    {
                        RouteId = line.Id,
                        Price = newCost,
                        FromDate = increaseRequest.StartDate
                    });
                    // Optional: only update explicitly if tracking is off or detached
                    //  _unitOfWork.TransportationLines.Update(line);
                }

                _unitOfWork.Complete();
                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error
                {
                    ErrorCode = "Err12",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });
                return response;
            }
        }
        public BaseResponseWithData<List<SuppliersAccountDto>> SuppliersAccount(long SupplierId, int Mounth)
        {
            var response = new BaseResponseWithData<List<SuppliersAccountDto>>
            {
                Errors = new List<Error>(),
                Result = false,
                Data = new List<SuppliersAccountDto>()
            };
            var routesDb = _unitOfWork.TransportationVehicleRoutes
                .FindAllQueryable(x => true, new[] { "Supplier" })
                .AsQueryable();
            var suppliers = _unitOfWork.Suppliers.FindAllQueryable(x => true).AsQueryable();

            if (SupplierId > 0)
            {
                suppliers = suppliers.Where(x => x.Id == SupplierId);
            }


            try
            {

                foreach (var supplier in suppliers)
                {

                    var supplierAccount = new SuppliersAccountDto();

                    supplierAccount.SupplierId = (long)supplier.Id;
                    supplierAccount.SupplierName = supplier.Name;
                    foreach (var route in routesDb.Where(x => x.SupplierId == supplier.Id))
                    {
                        // var routeCount = route.TransportationLine.LineName;
                        var roundOfLine = new RoundDto();
                        roundOfLine.LineName = route.TransportationLine.LineName;
                        var priceRound = CalculatePricePerRoute(route.Id, Mounth);
                        roundOfLine.TotalPrice = priceRound.TotalPrice;
                        roundOfLine.TotalRounds = priceRound.TotalRounds;
                        supplierAccount.TotalPrice += priceRound.TotalPrice;
                        supplierAccount.DetailsRounds.Add(roundOfLine);
                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error
                {
                    ErrorCode = "Err12",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });
                return response;
            }
        }

        public BaseResponse UpdateTransportationDirection(UpdateTransportationDirectionVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationDirection = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldTransportationDirection == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }

                oldTransportationDirection.TransportationVehicleRouteId = dto.TransportationVehicleRouteId;
                oldTransportationDirection.Active = dto.Active;
                oldTransportationDirection.Description = dto.Description;
                oldTransportationDirection.RouteDirection = dto.RouteDirection;
                oldTransportationDirection.Latitude = dto.Latitude;
                oldTransportationDirection.Longtitud = dto.Longtitud;

                _unitOfWork.TransportationVehicleRouteDirections.Update(oldTransportationDirection);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse UpdateTransportationEmployee(UpdateTransportationEmployeeVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldTransportationEmployee == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }

                oldTransportationEmployee.TransportationVehicleRouteId = dto.TransportationVehicleRouteId;
                oldTransportationEmployee.Active = dto.Active;
                oldTransportationEmployee.HrUserId = dto.hrUserId;
                oldTransportationEmployee.TransportationVehicleRouteDirectionId = dto.TransportationVehicleRouteDirectionId;

                _unitOfWork.TransportationVehicleRouteEmployees.Update(oldTransportationEmployee);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse UpdateTransportationLine(TransportationLineVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;
            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationLine = _unitOfWork.TransportationLines.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldTransportationLine == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }


                oldTransportationLine.LineName = dto.LineName;
                oldTransportationLine.ModifiedDate = DateTime.Now;
                oldTransportationLine.ModifiedBy = validation.userID;
                _unitOfWork.TransportationLines.Update(oldTransportationLine);

                _unitOfWork.Complete();
                Response.Result = true;
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

        // TransportationVehicleRoute
        public BaseResponse UpdateTransportationRoute(TransportationRouteUpdateVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationRoute = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == dto.Id && x.Active == true).FirstOrDefault();
                if (oldTransportationRoute == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }

                oldTransportationRoute.TransportationLineId = dto.TransportationLineId;
                oldTransportationRoute.Active = dto.Active;
                oldTransportationRoute.SupplierId = dto.SupplierId;
                oldTransportationRoute.SupplierContactPersonId = dto.SupplierContactPersonId;
                // oldTransportationRoute.BranchScheduleId = dto.BranchScheduleId;
                oldTransportationRoute.PeriodTo = dto.PeriodTo;
                oldTransportationRoute.ModifiedDate = DateTime.Now;
                oldTransportationRoute.ModifiedBy = validation.userID;
                //oldTransportationRoute.SupervisorId = dto.HrUserId;
                oldTransportationRoute.FromoDate = dto.FromoDate;
                oldTransportationRoute.ToDate = dto.ToDate;
                oldTransportationRoute.NameOfRoute = dto.NameOfRoute;
                oldTransportationRoute.LineCost = dto.LineCost;
                oldTransportationRoute.OneWay = dto.OneWay;
                oldTransportationRoute.TransportationVehicleId = dto.TransportationVehicleId;
                oldTransportationRoute.IsApproved = dto.IsApproved;

                _unitOfWork.TransportationVehicleRoutes.Update(oldTransportationRoute);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse UpdateTransportationVehicle(TransportationVehicleVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldTransportationVehicle = _unitOfWork.TransportationVehicles.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldTransportationVehicle == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }

                oldTransportationVehicle.VehicleTypeId = dto.VehicleTypeId;
                oldTransportationVehicle.Active = dto.Active;
                // oldTransportationVehicle.ApprovedBy = dto.ApprovedBy;
                //oldTransportationVehicle.IsApproved = dto.IsApproved;
                oldTransportationVehicle.Capacity = dto.Capacity;
                oldTransportationVehicle.ModifiedDate = DateTime.Now;
                oldTransportationVehicle.ModifiedBy = validation.userID;
                _unitOfWork.TransportationVehicles.Update(oldTransportationVehicle);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse UpdateVehicleType(VehicleTypeVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldVehicleType = _unitOfWork.VehicleTypes.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldVehicleType == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                oldVehicleType.Active = dto.Active;
                oldVehicleType.Type = dto.Type;
                _unitOfWork.VehicleTypes.Update(oldVehicleType);

                _unitOfWork.Complete();
                Response.Result = true;
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

        public BaseResponse UpdateRouteEmployee(UpdateRouteEmployeeVM dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldRouteEmploye = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldRouteEmploye == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }
                oldRouteEmploye.Active = dto.Active;
                oldRouteEmploye.TransportationVehicleRouteId = dto.TransportationVehicleRouteId;
                oldRouteEmploye.HrUserId = dto.HrUserId;
                oldRouteEmploye.TransportationVehicleRouteDirectionId = dto.TransportationVehicleRouteDirectionId;
                oldRouteEmploye.FromDate = dto.FromDate;
                oldRouteEmploye.ToDate = dto.ToDate;
                oldRouteEmploye.Period = dto.Period;
                oldRouteEmploye.DurationLatitude = dto.DurationLatitude;
                oldRouteEmploye.DurationLongtitud = dto.DurationLongtitud;
                _unitOfWork.TransportationVehicleRouteEmployees.Update(oldRouteEmploye);

                _unitOfWork.Complete();
                Response.Result = true;
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



        //public async Task<BaseResponseWithData<string>> AttendanceExcell(int Year, int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime date, string serialBus, int PageNo, int NoOfItems)
        //{
        //    BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
        //    Response.Result = true;
        //    Response.Errors = new List<Error>();
        //    try
        //    {
        //        // CLIENTS
        //        var client = _unitOfWork.Clients.FindAll(x => x.OwnerCoProfile == true).FirstOrDefault();
        //        var clientPhone = _unitOfWork.ClientPhones.FindAll(x => x.ClientId == client.Id).Select(y => y.Phone).FirstOrDefault();
        //        var clientAddress = _unitOfWork.ClientAddresses.FindAll(x => x.ClientId == client.Id).FirstOrDefault();
        //        var clientCountry = _unitOfWork.Countries.FindAll(x => x.Id == clientAddress.CountryId).Select(y => y.Name).FirstOrDefault();
        //        var clientgoverment = _unitOfWork.Governorates.FindAll(x => x.Id == clientAddress.GovernorateId).Select(y => y.Name).FirstOrDefault();
        //        var createdUser = _unitOfWork.Users.FindAll(x => x.Id == client.CreatedBy).FirstOrDefault();
        //        // ACCOUNTS
        //        var accounts = _unitOfWork.Accounts.FindAll(x => x.ParentCategory != 0);
        //        var accountsNames = accounts.Select(x => x.AccountName);


        //        var hrUsers = DashBoardForAttendence(TransportionlineId,SupplierId, supplierContactPersonId, date, serialBus, PageNo, NoOfItems).Data;

        //        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
        //        ExcelPackage excel = new ExcelPackage();
        //        for (int i = 1; i <= 12; i++)
        //        {

        //            string monthName = new DateTime(2000, i, 1).ToString("MMMM", new CultureInfo("ar-EG"));



        //            var sheet = excel.Workbook.Worksheets.Add($"{monthName} ");

        //            sheet.Cells[5, 1].Value = $"سند قيد يومية ";
        //            sheet.Cells[5, 1, 5, 3].Merge = true;
        //            sheet.Cells[5, 1, 5, 3].Style.Font.Bold = true;

        //            sheet.Cells[5, 1, 5, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            // Adjust column width if necessary


        //            sheet.Cells[6, 3].Value = $" سند قيد يومية عن شهر {monthName}";
        //            sheet.Cells[6, 2].Value = $"بتاريخ";

        //            int month = i;

        //            // حساب عدد الأيام في الشهر
        //            int lastDay = DateTime.DaysInMonth(Year, month);

        //            // إنشاء كائن يمثل آخر يوم
        //            DateTime lastDayOfMonth = new DateTime(Year, month, lastDay);

        //            sheet.Cells[6, 1].Value = $" {lastDayOfMonth.ToString("yyyy:MMMM:dd")} ";
        //            //sheet.Cells[6, 1, 5, 3].Merge = true;
        //            sheet.Cells[6, 3].Style.Font.Bold = true;
        //            sheet.Cells[6, 2].Style.Font.Bold = true;
        //            sheet.Cells[6, 1].Style.Font.Bold = true;

        //            sheet.Cells[6, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            sheet.Cells[6, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            sheet.Cells[6, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            //sheet.Cells[5, 1, 5, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



        //            sheet.Cells[1, 3].Value = $"{client.Name}";
        //            //sheet.Cells[1, 7, 1, 10].Merge = true;
        //            sheet.Cells[4, 3].Value = $" TEL : {clientPhone}";

        //            //sheet.Cells[2, 7, 2, 10].Merge = true;

        //            sheet.Cells[2, 3].Value = $"{clientCountry}, {clientgoverment}";
        //            //sheet.Cells[3, 7, 3, 10].Merge = true;

        //            sheet.Cells[3, 3].Value = $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.AreaId}";
        //            // sheet.Cells[4, 7, 4, 10].Merge = true;

        //            //sheet.Cells[1, 7, 3, 10].Merge = true;

        //            //sheet.Cells[1, 7].Value = $"{client.Name} \n" +$"{clientPhone }\n" + $"{clientCountry}, {clientgoverment} " + $"{clientAddress.Address}, BuildingNumber: {clientAddress.BuildingNumber}, Area: {clientAddress.Area}";


        //            sheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            sheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            sheet.Cells[4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            //sheet.Cells[1, 7, 3, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            // Optionally format the header cells
        //            //sheet.Cells[1, 1, 5, 1].Style.Font.Bold = true; // Make the text bold
        //            //sheet.Cells[1, 1, 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

        //            // Adjust column width to fit the content
        //            // Assuming a total width equivalent to a typical A4 page (approximately 100 units)
        //            double totalPageWidth = 90;
        //            double columnWidth = totalPageWidth / 3; // Divide equally into 3 columns

        //            // Set the width for each column
        //            sheet.Column(1).Width = columnWidth; // البيان
        //            sheet.Column(2).Width = columnWidth; // دائن
        //            sheet.Column(3).Width = columnWidth; // مدين


        //            //sheet.HeaderFooter.OddHeader.RightAlignedText = client.Name;
        //            //sheet.HeaderFooter.EvenHeader.RightAlignedText = client.Name;
        //            sheet.HeaderFooter.EvenFooter.CenteredText = "Page &P of &N ";
        //            sheet.HeaderFooter.EvenFooter.LeftAlignedText = $"{DateTime.Now}";
        //            sheet.HeaderFooter.EvenFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
        //            sheet.HeaderFooter.OddFooter.CenteredText = "Page &P of &N ";
        //            sheet.HeaderFooter.OddFooter.LeftAlignedText = $"{DateTime.Now}";
        //            sheet.HeaderFooter.OddFooter.RightAlignedText = $"{createdUser.FirstName} {createdUser.LastName}";
        //            sheet.HeaderFooter.FirstFooter.CenteredText = "Page &P of &N ";
        //            sheet.HeaderFooter.FirstFooter.LeftAlignedText = $"{DateTime.Now}";
        //            sheet.HeaderFooter.FirstFooter.RightAlignedText = $"{createdUser.FirstName}  {createdUser.LastName}";


        //            sheet.PrinterSettings.Orientation = eOrientation.Landscape;



        //            // Load the original image
        //            string imagePath3 = Path.Combine(_environment.WebRootPath) + "\\img\\STMark-Logo.png";
        //            Image originalImage = Image.FromFile(imagePath3);

        //            // Define the target dimensions
        //            int targetWidth = 64 * 4;  // مساحة عرض (عرض 3 أعمدة)
        //            int targetHeight = 60; // مساحة ارتفاع (ارتفاع 3 صفوف)

        //            // Calculate new dimensions while maintaining aspect ratio
        //            float aspectRatio = (float)originalImage.Width / originalImage.Height;
        //            int newWidth, newHeight;

        //            if (targetWidth / (float)targetHeight > aspectRatio)
        //            {
        //                // Fit by height
        //                newHeight = targetHeight;
        //                newWidth = (int)(targetHeight * aspectRatio);
        //            }
        //            else
        //            {
        //                // Fit by width
        //                newWidth = targetWidth;
        //                newHeight = (int)(targetWidth / aspectRatio);
        //            }

        //            // Resize the image
        //            Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

        //            // Save the resized image to a temporary file
        //            string tempImagePath = Path.Combine(_environment.WebRootPath, "img\\scaled.jpg");
        //            string tempDirectory = Path.GetDirectoryName(tempImagePath);

        //            // Ensure the directory exists
        //            if (!Directory.Exists(tempDirectory))
        //            {
        //                Directory.CreateDirectory(tempDirectory);
        //            }
        //            if (i == 1)
        //            {
        //                resizedImage.Save(tempImagePath);
        //            }
        //            // Create a FileInfo object for the resized image
        //            FileInfo resizedImageFile = new FileInfo(tempImagePath);
        //            // Add the image to the Excel sheet
        //            var picture = sheet.Drawings.AddPicture("MyPicture", resizedImageFile);

        //            // Position the picture in the desired cell
        //            picture.SetPosition(0, 0, 0, 0); // Top-left corner of Row 1, Column 1

        //            // Set the calculated size
        //            picture.SetSize(newWidth, newHeight);

        //            //resizedImageFile.Delete();






        //            // Set column headers
        //            // sheet.Cells [1 , 1].Value="No Row";
        //            sheet.Cells[8, 1].Value = "من المذكورين ";
        //            sheet.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(50, 205, 50));

        //            // sheet.Cells[7, 2, 7, 3].Merge = true;
        //            //sheet.Cells[7, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            sheet.Cells[7, 1].Value = "البيان";
        //            sheet.Cells[7, 2].Value = "دائن";
        //            sheet.Row(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            sheet.Cells[8, 3].Value = "مدين";





        //            int y = 9;
        //            int row = 0;
        //            if (hrUsers.Count > 0)
        //            {
        //                foreach (var item in hrUsers)
        //                {
        //                    sheet.Row(y).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                    sheet.Cells[y, 1].Value = item.FirstName;
        //                    sheet.Cells[y, 3].Value = item.ISAttendace;

        //                    y++;

        //                }
        //                row = y;

        //            }



        //            if (row >= 10)
        //            {
        //                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                sheet.Cells[row, 1].Value = "الي مذكورين";
        //                sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(50, 205, 50));

        //                row = row + 1;
        //            }


        //            if (hrUsers.Count > 0)
        //            {
        //                foreach (var item in hrUsers)
        //                {
        //                    sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                    sheet.Cells[row, 1].Value = item.MiddleName;
        //                    sheet.Cells[row, 2].Value = item.ISAttendace;

        //                    row++;

        //                }

        //            }



        //            var totaldain =1000;
        //            var totalmaden = 1200;
        //            if (row >= 11)
        //            {
        //                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                sheet.Cells[row, 1].Value = "الاجمالي";
        //                sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

        //                sheet.Cells[row, 2].Value = totaldain;
        //                sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

        //                sheet.Cells[row, 3].Value = totalmaden;
        //                sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144));

        //                row = row + 1;
        //            }

        //            if (row >= 12)
        //            {
        //                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


        //                sheet.Cells[row, 2].Value = totaldain - totalmaden == 0 ? true : false;
        //                sheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

        //                sheet.Cells[row, 3].Value = totalmaden - totalmaden;
        //                sheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                sheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));

        //            }





        //            sheet.Cells.Style.Font.Name = "Arial";
        //            sheet.Cells.Style.Font.Size = 8;



        //            //if (first <= alldataDB.Count + 1)
        //            //{
        //            //    sheet.Cells[first, 1, end, 1].Merge = true;
        //            //    sheet.Cells[first, 2, end, 2].Merge = true;
        //            //    sheet.Cells[first, 3, end, 3].Merge = true;
        //            //    sheet.Cells[first, 4, end, 4].Merge = true;
        //            //    sheet.Cells[first, 5, end, 5].Merge = true;
        //            //    sheet.Cells[first, 6, end, 6].Merge = true;
        //            //    //sheet.Cells[first, 7, end, 7].Merge = true;

        //            //    // Center the text in merged cells
        //            //    sheet.Cells[first, 1, end, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            //    //sheet.Cells[first, 1, end, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            //    sheet.Cells[first, 1, end, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
        //            //}

        //            //sheet.PrinterSettings.FitToPage = true;
        //            //sheet.PrinterSettings.FitToWidth = 1;
        //            sheet.PrinterSettings.Scale = 150;
        //            sheet.PrinterSettings.FitToHeight = 0;
        //            sheet.PrinterSettings.BottomMargin = 0.5M;
        //            sheet.PrinterSettings.TopMargin = 0;
        //            sheet.PrinterSettings.LeftMargin = 0;
        //            sheet.PrinterSettings.RightMargin = 0;

        //            sheet.PrinterSettings.PaperSize = ePaperSize.A4;
        //            // sheet.PrinterSettings.TopMargin = 1.5M;

        //            // Increase the row height of the first row


        //            // Adjust print settings to include gridlines and borders
        //            //sheet.View.ShowGridLines=true;
        //            //sheet.PrinterSettings.ShowGridLines=true;
        //            //sheet.PrinterSettings.ShowHeaders=true;

        //            sheet.Row(5).Style.Font.Bold = true;
        //            // sheet.Cells[1, 4, 1, 5].Style.Font.Bold = true;
        //            //sheet.Cells[1, 4, 1, 5].Style.Font.Size = 9;
        //            sheet.Cells[5, 1, 5, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            sheet.Cells[5, 1, 5, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
        //            sheet.Cells[8, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            sheet.Cells[8, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            sheet.Cells[8, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;


        //            sheet.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
        //            sheet.Cells[8, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
        //            sheet.Cells[8, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));

        //            sheet.Cells[8, 3].Style.Font.Bold = true;
        //            sheet.Cells[8, 2].Style.Font.Bold = true;
        //            sheet.Cells[8, 1].Style.Font.Bold = true;


        //            sheet.Row(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            sheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            // Save the Excel file



        //        }

        //        var path = $"TransportLineExcel";
        //        var savedPath = Path.Combine(_environment.WebRootPath, path);
        //        if (System.IO.File.Exists(savedPath))
        //            System.IO.File.Delete(savedPath);
        public BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>> GetEmployeeException(long? HrUserId, long? Id, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>> Response = new BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>>();
            Response.Data = new List<EmployeeExceptionVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                #region validation 
                if (HrUserId <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add RouteId";
                    Response.Errors.Add(error);
                    return Response;
                }
                #endregion
                var EmployeeExceptionDb = _unitOfWork.TransportationVehicleRouteEmployeeExceptions.FindAllQueryable(x => x.Id > 0, new[] { "HrUser" }).OrderBy(w => w.CreationDate);

                if (HrUserId != null)
                {
                    EmployeeExceptionDb = EmployeeExceptionDb.Where(x => x.HrUserId == HrUserId).OrderBy(w => w.CreationDate);

                }

                if (Id != null)
                {
                    EmployeeExceptionDb = EmployeeExceptionDb.Where(x => x.Id == Id).OrderBy(w => w.CreationDate);

                }

                var EmployeeExceptionList = PagedList<TransportationVehicleRouteEmployeeException>.Create(EmployeeExceptionDb, PageNo, NoOfItems);
                var DirectionDB = _unitOfWork.TransportationVehicleRouteDirections.FindAll(x => x.Id > 0);
                var RouteDB = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine" });
                var Users = _unitOfWork.Users.FindAllQueryable(x => x.Id > 0);
                Response.Data = EmployeeExceptionList.Select(a => new EmployeeExceptionVM
                {
                    Id = a.Id,
                    TransportationVehicleRouteId = a.TransportationVehicleRouteId,
                    HrUserId = a.HrUserId,
                    HrUserName = a.HrUser.FirstName + " " + a.HrUser.MiddleName + " " + a.HrUser.LastName,
                    Active = a.Active,
                    CreationBy = a.CreationBy,
                    CreationName = Users.Where(x => x.Id == a.CreationBy).Select(n => n.FirstName + n.MiddleName + n.LastName).FirstOrDefault(),
                    CreationDate = a.CreationDate,
                    TransportationVehicleRouteDirectionId = a.TransportationVehicleRouteDirectionId,
                    TransportationVehicleRouteDirectionName = DirectionDB != null ? DirectionDB.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.RouteDirection).FirstOrDefault() : null,
                    FromDate = a.FromDate,
                    ToDate = a.ToDate,
                    Period = a.Period,
                    DayName = a.DayName,
                    Latitude = a.Latitude,
                    Longtitud = a.Longtitud,
                    ExceptionDate = a.ExceptionDate,
                    ReasonException = a.ReasonException,
                    ContactNumber = a.ContactNumber,
                    TransportationLineName = RouteDB.Where(x => x.Id == a.TransportationVehicleRouteId).Select(w => w.TransportationLine.LineName).FirstOrDefault()
                }).ToList();
                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = EmployeeExceptionList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = EmployeeExceptionList.TotalCount
                };
                Response.Result = true;
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
        // fix mirge 2
        public BaseResponse AddException(TransportationVehicleRouteEmployeeException dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.TransportationVehicleRouteId <= 0 || dto.HrUserId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameters";
                    Response.Errors.Add(error);
                }
                if ((dto.Latitude <= 0 && dto.Longtitud <= 0) && dto.TransportationVehicleRouteDirectionId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add Location";
                    Response.Errors.Add(error);
                }
                _unitOfWork.TransportationVehicleRouteEmployeeExceptions.Add(new TransportationVehicleRouteEmployeeException
                {
                    TransportationVehicleRouteId = dto.TransportationVehicleRouteId,
                    HrUserId = dto.HrUserId,
                    Active = true,
                    CreationDate = DateTime.Now,
                    CreationBy = validation.userID,
                    TransportationVehicleRouteDirectionId = dto.TransportationVehicleRouteDirectionId,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    Period = dto.Period,
                    DayName = dto.DayName,
                    ExceptionDate = dto.ExceptionDate,
                    Latitude = dto.Latitude,
                    Longtitud = dto.Longtitud,
                    ReasonException = dto.ReasonException,
                    ContactNumber = dto.ContactNumber
                });

                var resuilt = _unitOfWork.Complete();
                if (resuilt < 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "error in save";
                    Response.Errors.Add(error);
                }
                Response.Result = true;
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
        public BaseResponse UpdateException(TransportationVehicleRouteEmployeeException dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.TransportationVehicleRouteId <= 0 || dto.HrUserId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameters";
                    Response.Errors.Add(error);
                }
                if ((dto.Latitude <= 0 && dto.Longtitud <= 0) && dto.TransportationVehicleRouteDirectionId <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add Location";
                    Response.Errors.Add(error);
                }
                var EmployeeExceptionDb = _unitOfWork.TransportationVehicleRouteEmployeeExceptions.FindAll(x => x.Id == dto.Id).FirstOrDefault();


                EmployeeExceptionDb.TransportationVehicleRouteId = dto.TransportationVehicleRouteId;
                EmployeeExceptionDb.HrUserId = dto.HrUserId;
                EmployeeExceptionDb.Active = dto.Active;
                EmployeeExceptionDb.CreationDate = DateTime.Now;
                EmployeeExceptionDb.CreationBy = validation.userID;
                EmployeeExceptionDb.TransportationVehicleRouteDirectionId = dto.TransportationVehicleRouteDirectionId;
                EmployeeExceptionDb.FromDate = dto.FromDate;
                EmployeeExceptionDb.ToDate = dto.ToDate;
                EmployeeExceptionDb.Period = dto.Period;
                EmployeeExceptionDb.DayName = dto.DayName;
                EmployeeExceptionDb.ExceptionDate = dto.ExceptionDate;
                EmployeeExceptionDb.Latitude = dto.Latitude;
                EmployeeExceptionDb.Longtitud = dto.Longtitud;
                EmployeeExceptionDb.ReasonException = dto.ReasonException;
                EmployeeExceptionDb.ContactNumber = dto.ContactNumber;

                var resuilt = _unitOfWork.Complete();
                if (resuilt < 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "error in save";
                    Response.Errors.Add(error);
                }
                Response.Result = true;
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

        public BaseResponseWithData<CapacityTransportionNumbersVM> GetCapacityNumbers(long RouteID, string Period, DateTime? DateSearch)
        {
            BaseResponseWithData<CapacityTransportionNumbersVM> Response = new BaseResponseWithData<CapacityTransportionNumbersVM>();
            Response.Data = new CapacityTransportionNumbersVM();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                #region validation 
                if (RouteID <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add RouteId";
                    Response.Errors.Add(error);
                    return Response;
                }
                if (string.IsNullOrEmpty(Period))
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add Period";
                    Response.Errors.Add(error);
                }
                #endregion

                //var RouteTransportionDb = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == RouteID).FirstOrDefault();
                //var VehicleId = _unitOfWork.TransportationLines.FindAll(x => x.Id == RouteTransportionDb.TransportationLineId).Select(y => y.TransportationVehicleId).FirstOrDefault();
                //var TransportationVehicle = _unitOfWork.TransportationVehicles.FindAll(x => x.Id == VehicleId).Select(y => y.Capacity).FirstOrDefault();
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);


                //DateTime date = new DateTime(2024, 1, 15);
                //string dayName = date.DayOfWeek.ToString();

                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;
                if (DateSearch != null)
                {
                    Day = DateSearch?.DayOfWeek.ToString();
                    FullDateTime = (DateTime)DateSearch;
                }
                Response.Data.FullCapacity = _unitOfWork.TransportationVehicleRoutes
    .FindAllQueryable(r => r.Id == RouteID && r.Active == true, new[] { "TransportationVehicle" })
    .Select(r => r.TransportationVehicle.Capacity)
    .FirstOrDefault();

                var allRouteEmployees = _unitOfWork.TransportationVehicleRouteEmployees
      .FindAll(r => r.TransportationVehicleRouteId == RouteID && (((DateTime)r.FromDate).Date <= FullDateTime.Date && (((DateTime)r.ToDate).Date >= FullDateTime.Date || r.ToDate == null)) && (r.Period == Period || r.Period == "Both"));

                var AllemployeeExpection = _unitOfWork.TransportationVehicleRouteEmployeeExceptions
    .FindAll(r => r.Id > 0 && ((((DateTime)r.FromDate).Date <= FullDateTime.Date && (((DateTime)r.ToDate).Date >= FullDateTime.Date || r.ToDate == null) && r.Period == Period && r.DayName == Day) || (r.ExceptionDate == FullDateTime.Date && r.Period == Period)));

                int i = 0;
                foreach (var item in allRouteEmployees)
                {
                    var employeeExpection = AllemployeeExpection.Where(x => x.HrUserId == item.HrUserId).FirstOrDefault();
                    if (employeeExpection != null)
                    {
                        i = i + 1;
                    }
                }

                Response.Data.RouteEmployeesToOtherLines = i;

                Response.Data.ExpectionNumFromOtherLines = AllemployeeExpection
     .Where(r => r.TransportationVehicleRouteId == RouteID).Count();


                Response.Data.CapacityWithoutExpection = allRouteEmployees.Count();

                Response.Data.ActualCapacity = (Response.Data.CapacityWithoutExpection + Response.Data.ExpectionNumFromOtherLines) - Response.Data.RouteEmployeesToOtherLines;

                Response.Result = true;
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




        public BaseResponseWithData<List<EmployeeBasicInfoVM>> GetTransportationEmployees(long RouteID, string Period, DateTime? DateSearch)
        {
            BaseResponseWithData<List<EmployeeBasicInfoVM>> Response = new BaseResponseWithData<List<EmployeeBasicInfoVM>>();
            Response.Data = new List<EmployeeBasicInfoVM>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                #region validation
                if (RouteID <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add RouteId";
                    Response.Errors.Add(error);
                }

                if (string.IsNullOrEmpty(Period))
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add Period";
                    Response.Errors.Add(error);
                }
                #endregion
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;
                if (DateSearch != null)
                {
                    Day = DateSearch?.DayOfWeek.ToString();
                    FullDateTime = (DateTime)DateSearch;
                }
                var AllData = new List<EmployeeBasicInfoVM>();
                var DirecionDb = _unitOfWork.TransportationVehicleRouteDirections.FindAllQueryable(x => x.Id > 0);
                //  var TransportationEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.TransportationVehicleRouteId == RouteId, new[] { "HrUser" });

                var allEmployeesInRoute = _unitOfWork.TransportationVehicleRouteEmployees
      .FindAll(r => r.TransportationVehicleRouteId == RouteID && (((DateTime)r.FromDate).Date <= FullDateTime.Date && (((DateTime)r.ToDate).Date >= FullDateTime.Date || r.ToDate == null) && (r.Period == Period || r.Period == "Both")), new[] { "HrUser" });

                var AllemployeeExpection = _unitOfWork.TransportationVehicleRouteEmployeeExceptions
  .FindAll(r => r.Id > 0 && ((((DateTime)r.FromDate).Date <= FullDateTime.Date && (((DateTime)r.ToDate).Date >= FullDateTime.Date || r.ToDate == null) && r.Period == Period && r.DayName == Day) || (r.ExceptionDate == FullDateTime.Date && r.Period == Period)), new[] { "HrUser" });

                //int i = 0;
                //var hrUsersExpectionInRoute = new List<TransportationVehicleRouteEmployee>();

                //foreach (var item in allEmployeesInRoute)
                //{
                //    var employeeExpection = AllemployeeExpection.Where(x => x.HrUserId == item.HrUserId).Select(h=>h.HrUserId).FirstOrDefault();
                //    if (employeeExpection != null)
                //    {
                //        var hruserExpection = allEmployeesInRoute.Where(x => x.HrUserId == employeeExpection).FirstOrDefault();
                //        hrUsersExpectionInRoute.Add(hruserExpection);
                //        i = i + 1;
                //    }
                //}
                //var allEmployeesInRouteWithoutExpection = allEmployeesInRoute.Except(hrUsersExpectionInRoute);
                var allEmployeesInRouteWithExpection = allEmployeesInRoute.Where(x => !(AllemployeeExpection.Select(h => h.HrUserId).Contains(x.HrUserId)));


                var ExpectionFromOtherLines = AllemployeeExpection.Where(r => r.TransportationVehicleRouteId == RouteID);


                var transprotationUserAttedanceDB = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Id > 0);

                var allEmployeesInRouteWithoutExpectionList = allEmployeesInRouteWithExpection.Select(a =>
                {
                    return new EmployeeBasicInfoVM
                    {
                        //ID = a.HrUser.Id,
                        HrUserId = a.HrUserId,
                        FirstNane = a.HrUser.FirstName,
                        MiddleName = a.HrUser.MiddleName,
                        LastName = a.HrUser.LastName,
                        Mobile = a.HrUser.Mobile,
                        Email = a.HrUser.Email,
                        Photo = a.HrUser.ImgPath != null ? Globals.baseURL + "/" + a.HrUser.ImgPath : null,
                        Latitude = a.DurationLatitude,
                        Longtitud = a.DurationLatitude,
                        DirectionName = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.RouteDirection).FirstOrDefault(),
                        DirectionId = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Id).FirstOrDefault(),
                        DirectionLatitude = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Latitude).FirstOrDefault(),
                        DirectionLongtitud = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Longtitud).FirstOrDefault(),
                        RegisteredInline = true,
                        CheckIn = transprotationUserAttedanceDB
                                                                // switch to LINQ to Objects (client-side)
                                                                .Where(x => long.Parse(x.Serial) == a.HrUserId && ((DateTime)x.CreationDate).Date == ((DateTime)FullDateTime).Date)
                                                                .Select(c => c.CheckIn)
                                                                .FirstOrDefault(),

                        //  CheckIn = transprotationUserAttedanceDB.Where(x=>(long.Parse(x.Serial)) == a.HrUserId && ( ((DateTime)x.CheckIn).Date ) == ((DateTime)DateSearch).Date).Select(c=>c.CheckIn).FirstOrDefault() ?? null,
                        //  CheckOut = transprotationUserAttedanceDB.Where(x=>(long.Parse(x.Serial)) == a.HrUserId && ( ((DateTime)x.CheckOut).Date ) == ((DateTime)DateSearch).Date).Select(c=>c.CheckOut).FirstOrDefault() ?? null,
                        CheckOut = transprotationUserAttedanceDB
                                                        // switch to LINQ to Objects (client-side)
                                                        .Where(x => long.Parse(x.Serial) == a.HrUserId && ((DateTime)x.CreationDate).Date == ((DateTime)FullDateTime).Date)
                                                        .Select(c => c.CheckOut)
                                                        .FirstOrDefault(),

                    };
                }).ToList();

                var ExpectionFromOtherLinesList = ExpectionFromOtherLines.Select(a =>
                {
                    return new EmployeeBasicInfoVM
                    {
                        //  ID = a.HrUser.Id,
                        HrUserId = a.HrUserId,
                        FirstNane = a.HrUser.FirstName,
                        MiddleName = a.HrUser.MiddleName,
                        LastName = a.HrUser.LastName,
                        Mobile = a.HrUser.Mobile,
                        Email = a.HrUser.Email,
                        Photo = a.HrUser.ImgPath != null ? Globals.baseURL + "/" + a.HrUser.ImgPath : null,
                        Latitude = a.Latitude,
                        Longtitud = a.Longtitud,
                        DirectionName = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.RouteDirection).FirstOrDefault(),
                        DirectionId = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Id).FirstOrDefault(),
                        DirectionLatitude = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Latitude).FirstOrDefault(),
                        DirectionLongtitud = DirecionDb.Where(x => x.Id == a.TransportationVehicleRouteDirectionId).Select(n => n.Longtitud).FirstOrDefault(),
                        RegisteredInline = false,
                        CheckIn = null,
                        CheckOut = null
                    };
                }).ToList();

                Response.Data = allEmployeesInRouteWithoutExpectionList.Union(ExpectionFromOtherLinesList).ToList();
                Response.Result = true;
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


        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence22(int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, long HrUser, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> Response = new BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>>();
            Response.Data = new List<DashBoardForHrUsers>();
            Response.Errors = new List<Error>();
            Response.Result = true;

            try
            {


                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

                var Day = egyptTime.ToString("dddd");
                var FullDateTime = egyptTime;
                if (DateSerach != null)
                {
                    Day = DateSerach?.DayOfWeek.ToString();
                    FullDateTime = (DateTime)DateSerach;
                }




                var Alldata = new List<DashBoardForHrUsers>();
                var data = new DashBoardForHrUsers();
                var transprotationUserAttedance = new List<TransprotationUserAttedance>();
                if (FromDate != null && ToDate != null)
                {
                    transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => ((((DateTime)x.CheckIn).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckIn).Date <= ((DateTime)ToDate).Date) || ((DateTime)x.CheckOut).Date >= ((DateTime)FromDate).Date && ((DateTime)x.CheckOut).Date <= ((DateTime)ToDate).Date) && x.Type == "Person").ToList();
                }
                else
                {
                    transprotationUserAttedance = _unitOfWork.TransprotationUserAttedances.FindAll(x => (((DateTime)x.CheckIn).Date == FullDateTime || ((DateTime)x.CheckOut).Date == FullDateTime) && x.Type == "Person").ToList();

                }
                var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);
                var AllHrUsersId = transportationVehicleRouteEmployee.Select(x => x.HrUserId).ToList();
                var HrUserDb = _unitOfWork.HrUsers.FindAllQueryable(x => AllHrUsersId.Contains(x.Id));
                var transportionlineDB = _unitOfWork.TransportationLines.FindAllQueryable(x => x.Id > 0);
                var SupplierDB = _unitOfWork.Suppliers.FindAllQueryable(x => x.Id > 0);
                var supplierContactPersonDB = _unitOfWork.SupplierContactPeople.FindAllQueryable(x => x.Id > 0);

                var VehicleRoute = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0 && x.Active == true, new[] { "TransportationLine", "SupplierContactPerson", "Supplier", "TransportationVehicle.VehicleType", "Supervisor" });

                if (TransportionlineId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.TransportationLineId == TransportionlineId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    var HrUsersForLine = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Count();
                    var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

                    var AllAttedanceUserIds = transprotationUserAttedance
                        .Where(x => x.Type == "Person")
                        .Select(y => long.Parse(y.Serial))
                        .ToList();
                    var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    HrUserDb = HrUserDb.Where(x => AllUsersForLine.Contains(x.Id));


                }


                else if (SupplierId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierId == SupplierId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);
                    var AttedanceBus = transprotationUserAttedance.Where(x => VehicleRoute.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus").Count();
                    var AttedanceUsers = transprotationUserAttedance.Where(x => x.Type == "Person").Count();

                    var AllAttedanceUserIds = transprotationUserAttedance
                       .Where(x => x.Type == "Person")
                       .Select(y => long.Parse(y.Serial))
                       .ToList();
                    var AllUsersForSupplier = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    HrUserDb = HrUserDb.Where(x => AllUsersForSupplier.Contains(x.Id));



                }
                else if (supplierContactPersonId > 0)
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.SupplierContactPersonId == supplierContactPersonId);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    var AllAttedanceUserIds = transprotationUserAttedance
                                          .Where(x => x.Type == "Person")
                                          .Select(y => long.Parse(y.Serial))
                                          .ToList();
                    var AllUsersForsupplierContact = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                    HrUserDb = HrUserDb.Where(x => AllUsersForsupplierContact.Contains(x.Id));


                }

                else if (!string.IsNullOrEmpty(serialBus))
                {
                    VehicleRoute = VehicleRoute.Where(x => x.Active == true && x.Serial == serialBus);
                    var transportationVehicleRouteId = VehicleRoute.Select(t => t.Id);

                    var AllAttedanceUserIds = transprotationUserAttedance
                                          .Where(x => x.Type == "Person")
                                          .Select(y => long.Parse(y.Serial))
                                          .ToList();
                    var AllUsersForserialBus = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();

                    HrUserDb = HrUserDb.Where(x => AllUsersForserialBus.Contains(x.Id));



                }
                if (HrUser > 0)
                {
                    //HrUserDb = HrUserDb.Where(x => x.Id == HrUser).FirstOrDefault();
                    var hrUser = HrUserDb.FirstOrDefault(x => x.Id == HrUser);

                    transprotationUserAttedance = transprotationUserAttedance.Where(x => (long.Parse(x.Serial)) == hrUser.Id).ToList();
                    var usersPagedList22 = PagedList<TransprotationUserAttedance>.Create(transprotationUserAttedance.AsQueryable(), PageNo, NoOfItems);

                    Alldata = usersPagedList22.Select(u => new DashBoardForHrUsers
                    {

                        Id = hrUser.Id,
                        FirstName = hrUser.FirstName,
                        MiddleName = hrUser.MiddleName,
                        LastName = hrUser.LastName,
                        ISAttendace = u.CheckIn != null ? true : u.CheckOut != null ? true : false,
                        TransportionlineName = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                          VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == hrUser.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationLine.LineName).FirstOrDefault(),
                        SupplierName = SupplierId > 0 ? SupplierDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                          VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == hrUser.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),
                        supplierContactPersonName = supplierContactPersonId > 0 ? supplierContactPersonDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                          VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == hrUser.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),

                        VehicleType = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                          VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == hrUser.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationVehicle.VehicleType.Type).FirstOrDefault(),
                        CheckIn = transprotationUserAttedance.Where(x => (long.Parse(x.Serial)) == hrUser.Id).Select(c => c.CheckIn).FirstOrDefault(),
                        CheckOut = transprotationUserAttedance.Where(x => (long.Parse(x.Serial)) == hrUser.Id).Select(c => c.CheckOut).FirstOrDefault(),
                        SupervisorName = VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == hrUser.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supervisor.FirstName).FirstOrDefault() ?? null,
                        //  ISAttendace = transprotationUserAttedance.Where(x => long.Parse(x.Serial) == hrUser.Id).FirstOrDefault() != null ? true : false
                    }).ToList();

                    Response.PaginationHeader = new PaginationHeader
                    {
                        CurrentPage = PageNo,
                        TotalPages = usersPagedList22.TotalPages,
                        ItemsPerPage = NoOfItems,
                        TotalItems = usersPagedList22.TotalCount
                    };


                }

                else
                {
                    var usersPagedList = PagedList<HrUser>.Create(HrUserDb, PageNo, NoOfItems);


                    var attendedUserIds = transprotationUserAttedance
        .Select(x =>
        {
            long id;
            return long.TryParse(x.Serial, out id) ? (long?)id : null;
        })
        .Where(id => id.HasValue)
        .Select(id => id.Value)
        .ToHashSet();

                    Alldata = usersPagedList.Where(x => AllHrUsersId.Contains((long)x.Id)).Select(u => new DashBoardForHrUsers
                    {

                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        ISAttendace = attendedUserIds.Contains(u.Id),
                        TransportionlineName = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                                VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationLine.LineName).FirstOrDefault(),
                        SupplierName = SupplierId > 0 ? SupplierDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                                VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),
                        supplierContactPersonName = supplierContactPersonId > 0 ? supplierContactPersonDB.Where(x => x.Id == SupplierId).Select(n => n.Name).FirstOrDefault() :
                                                VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supplier.Name).FirstOrDefault(),

                        VehicleType = TransportionlineId > 0 ? transportionlineDB.Where(x => x.Id == TransportionlineId).Select(n => n.LineName).FirstOrDefault() :
                                                VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.TransportationVehicle.VehicleType.Type).FirstOrDefault(),
                        CheckIn = transprotationUserAttedance.Where(x => (long.Parse(x.Serial)) == u.Id).Select(c => c.CheckIn).FirstOrDefault(),
                        CheckOut = transprotationUserAttedance.Where(x => (long.Parse(x.Serial)) == u.Id).Select(c => c.CheckOut).FirstOrDefault(),
                        SupervisorName = VehicleRoute.Where(x => x.Id == (transportationVehicleRouteEmployee.Where(x => x.HrUserId == u.Id).Select(c => c.TransportationVehicleRouteId).FirstOrDefault())).Select(n => n.Supervisor.FirstName).FirstOrDefault() ?? null,
                        //  ISAttendace = transprotationUserAttedance.Where(x => long.Parse(x.Serial) == u.Id).FirstOrDefault() != null ? true : false
                    }).ToList();

                    Response.PaginationHeader = new PaginationHeader
                    {
                        CurrentPage = PageNo,
                        TotalPages = usersPagedList.TotalPages,
                        ItemsPerPage = NoOfItems,
                        TotalItems = usersPagedList.TotalCount
                    };
                }


                Response.Data = Alldata;

                return Response;

            }
            catch (Exception ex)
            {
                Response.Result = false;
                Error error = new Error();
                error.ErrorCode = "Err10";
                error.ErrorMSG = ex.InnerException?.Message;
                Response.Errors.Add(error);
                return Response;
            }
        }
        public BaseResponse AddUsersAttedance44()
        {
            BaseResponse Response = new BaseResponse { Result = false, Errors = new List<Error>() };
            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var FullDateTime = egyptTime;
                var TransportionLineAccountDb = _unitOfWork.TransportationVehicleRouteAccounts.FindAll(x => x.Id > 0).ToList();


                var userAttandanceDB = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Id > 0).OrderBy(y => y.CreationDate).ToList();

                var userAttandanceCheckOutExit = new List<AttendanceCheckOutVm>();

                var userAttandanceLast = new TransprotationUserAttedance();
                if (userAttandanceDB != null)
                {
                    userAttandanceLast = userAttandanceDB.LastOrDefault();
                }

                var pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true).ToList();
                var userAttandanceLastDate = new DateTime();
                if (userAttandanceLast != null)
                {
                    userAttandanceLastDate = userAttandanceLast.CreationDate.Date;
                    //pharmaAttandance = _ContextTransportation.v_Bus_Program.Where(x => SafeConvertToDateTime(x.CheckTime).Date >= userAttandanceLastDate).ToList();
                    pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true)
                        .AsEnumerable()
                        .Where(x => SafeConvertToDateTime(x.CheckDate).Date >= userAttandanceLastDate)
                         .OrderBy(l => SafeConvertToDateTime(l.CheckDate + " " + l.CheckTime))
                        .ToList();
                }





                var employeeRoutes = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                var routesInfo = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Active == true, new[] { "TransportationVehicle" }).ToList();

                foreach (var log in pharmaAttandance)
                {
                    DateTime logDateTime = SafeConvertToDateTime(log.CheckDate + " " + log.CheckTime);

                    if (logDateTime.Date == new DateTime(2026, 9, 3))
                    {
                    }
                    var userSubscribedRouteIds = employeeRoutes
                        .Where(x => x.HrUser.Email == log.EmployeeNumber.ToString())
                        .Select(x => x.TransportationVehicleRouteId).ToList();

                    if (!userSubscribedRouteIds.Any()) continue;
                    var potentialRoutes = routesInfo.Where(r => userSubscribedRouteIds.Contains(r.Id)).ToList();

                    if (log.Type == "I") // بصمة دخول
                    {
                        // 1. محاولة مطابقة دقيقة (60 دقيقة)
                        var matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 59);

                        //// 2. إذا لم يجد، محاولة مطابقة واسعة (120 دقيقة)
                        //if (matchedRoute == null)
                        //    matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 120);

                        // 3. إذا لم يجد أي أتوبيس قريب، يأخذ أول أتوبيس مسجل له (Fallback) لضمان عدم ضياع البصمة
                        var routeIdToUse = matchedRoute?.Id;

                        var existing = userAttandanceDB
                            .Where(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckIn.Value.Date == logDateTime.Date).FirstOrDefault();

                        if (existing == null)
                        {
                            var existingRecord = userAttandanceCheckOutExit
                             .FirstOrDefault(x =>
                                 x.Serial == log.EmployeeNumber.ToString() &&
                                 x.CheckOut.Date == logDateTime.Date // نفس اليوم فقط
                             );

                            if (existingRecord != null)
                            {
                                if (logDateTime.AddMinutes(30) >= existingRecord.CheckOut)
                                {
                                    continue;
                                }
                            }
                            var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckIn = logDateTime,
                                CheckInRouteId = routeIdToUse,
                                Type = log.BusType,
                                CreationDate = DateTime.Now,
                                CreationBy = 1
                            });
                            userAttandanceDB.Add(newAttendance);
                        }

                    }
                    else if (log.Type == "O") // بصمة خروج
                    {



                        // أولوية 1: البحث عن أي سجل مفتوح خلال 18 ساعة لإغلاقه
                        var openRecord = userAttandanceDB
                            .Where(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckOut == null && x.CheckIn >= logDateTime.AddHours(-14))
                            .OrderByDescending(x => x.CheckIn).FirstOrDefault();

                        // أولوية 2: تحديد أتوبيس الخروج (نطاق واسع 30 دقيقة)
                        var matchedOutRoute = potentialRoutes.FirstOrDefault(r => r.ToDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.ToDate.Value.TimeOfDay).TotalMinutes) <= 30);

                        if (openRecord != null)
                        {
                            openRecord.CheckOut = logDateTime;
                            // نضع أتوبيس الخروج المكتشف، وإذا لم نجد نضع نفس أتوبيس الدخول
                            openRecord.CheckOutRouteId = matchedOutRoute?.Id;
                            //_unitOfWork.TransprotationUserAttedances.Update(openRecord);
                        }
                        //else
                        //{
                        //    userAttandanceIsFound.Add(openRecord.Id);
                        //}
                        else
                        {
                            // إذا لم يجد دخول، ينشئ سجل خروج فقط لكي لا تضيع البصمة
                            var newAttendance = new AttendanceCheckOutVm
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckOut = logDateTime
                            };

                            userAttandanceCheckOutExit.Add(newAttendance);

                        }

                    }
                }


                // 1. تحديد قائمة الأيام التي وردت في البصمات لمعالجتها
                var datesToProcess = pharmaAttandance
                    .Select(x => SafeConvertToDateTime(x.CheckDate).Date)
                    .Distinct()
                    .ToList();

                foreach (var day in datesToProcess)
                {

                    foreach (var route in routesInfo)
                    {
                        if (route.Id > 154)
                        {
                        }
                        // سعة الأتوبيس
                        int capacity = route.TransportationVehicle?.Capacity ?? 0;
                        if (capacity == 0) continue; // تخطي إذا لم توجد سعة مسجلة

                        // 2. حساب عدد الموظفين الذين بصموا دخول في هذا المسار وهذا اليوم
                        var checkInCount = userAttandanceDB
                            .Where(x => x.CheckIn.HasValue && x.CheckIn.Value.Date == day && x.CheckInRouteId == route.Id)
                            .Count();

                        // 3. حساب عدد الموظفين الذين بصموا خروج في هذا المسار وهذا اليوم
                        var checkOutCount = userAttandanceDB
                            .Where(x => x.CheckOut.HasValue && x.CheckOut.Value.Date == day && x.CheckOutRouteId == route.Id)
                            .Count();

                        // 4. تطبيق قاعدة (أكثر من نصف السعة)
                        bool isBusInActive = checkInCount > (capacity / 2);
                        bool isBusOutActive = checkOutCount > (capacity / 2);

                        // 5. هنا يمكنك حفظ النتيجة في جدول (BusAttendance) أو اتخاذ إجراء
                        if (isBusInActive)
                        {
                            if ((bool)route.OneWay)
                            {
                                var exitOneWay = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                                if (exitOneWay == null)
                                {
                                    var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                        Type = "Bus",
                                        CreationDate = DateTime.Now,
                                        CreationBy = 1
                                    });
                                    userAttandanceDB.Add(newAttendance);

                                    _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                        TransportationVehicleRouteId = route.Id,
                                        PriceRound = route.LineCost
                                    });

                                    var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == FullDateTime.Month).FirstOrDefault();


                                    if (checkAccount == null)
                                    {
                                        decimal CountOfround = 0;
                                        if ((bool)route.OneWay)
                                        {
                                            CountOfround = route.LineCost;
                                        }
                                        else
                                        {
                                            CountOfround = (route.LineCost / 2);

                                        }
                                        var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                        {
                                            TransportationVehicleRouteId = route.Id,
                                            Serial = route.Serial,
                                            DateOfMonth = DateTime.Now,
                                            CountOfRounds = 1,
                                            PriceOfRounds = CountOfround,
                                            CreationBy = 1,
                                            CreationDate = FullDateTime,
                                            ModifiedBy = 1,
                                            ModifiedDate = FullDateTime
                                        });
                                        TransportionLineAccountDb.Add(newAccount);
                                    }
                                    else
                                    {
                                        if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                        {
                                            checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                        }
                                        if ((bool)route.OneWay)
                                        {
                                            checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + route.LineCost;
                                        }
                                        else
                                        {
                                            checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + (route.LineCost / 2);
                                        }
                                        checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                        checkAccount.ModifiedBy = 1;
                                        checkAccount.ModifiedDate = FullDateTime;
                                        // _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                                    }

                                    continue;
                                }
                            }

                            var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            if (exit == null)
                            {
                                var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckIn = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = 1
                                });

                                userAttandanceDB.Add(newAttendance);

                                _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                {
                                    Serial = route.Serial,
                                    CheckInDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    TransportationVehicleRouteId = route.Id,
                                    PriceRound = (route.LineCost / 2)

                                });

                                var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == FullDateTime.Month).FirstOrDefault();


                                if (checkAccount == null)
                                {
                                    decimal CountOfround = 0;
                                    if ((bool)route.OneWay)
                                    {
                                        CountOfround = route.LineCost;
                                    }
                                    else
                                    {
                                        CountOfround = (route.LineCost / 2);

                                    }
                                    // update not add 
                                    var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                    {
                                        TransportationVehicleRouteId = route.Id,
                                        Serial = route.Serial,
                                        SupplierId = route.SupplierId ?? 0,
                                        DateOfMonth = DateTime.Now,
                                        CountOfRounds = 1,
                                        PriceOfRounds = CountOfround,
                                        CreationBy = 1,
                                        CreationDate = FullDateTime,
                                        ModifiedBy = 1,
                                        ModifiedDate = FullDateTime
                                    });
                                    TransportionLineAccountDb.Add(newAccount);
                                }
                                else
                                {
                                    if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                    {
                                        checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                    }
                                    if ((bool)route.OneWay)
                                    {
                                        checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + route.LineCost;
                                    }
                                    else
                                    {
                                        checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + (route.LineCost / 2);
                                    }
                                    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                    checkAccount.ModifiedBy = 1;
                                    checkAccount.ModifiedDate = FullDateTime;
                                    //_unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                                }

                            }
                        }

                        if (isBusOutActive)
                        {
                            var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == FullDateTime.Month).FirstOrDefault();
                            var recoredBefore = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckOut?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            if (recoredBefore != null)
                            {
                                continue;
                            }
                            if ((bool)route.OneWay)
                            {
                                var exitOneWay = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                                if (exitOneWay == null)
                                {
                                    var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                        Type = "Bus",
                                        CreationDate = DateTime.Now,
                                        CreationBy = 1,
                                    });

                                    userAttandanceDB.Add(newAttendance);

                                    _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                        TransportationVehicleRouteId = route.Id,
                                        PriceRound = route.LineCost

                                    });



                                    if (checkAccount == null)
                                    {
                                        decimal CountOfround = 0;
                                        if ((bool)route.OneWay)
                                        {
                                            CountOfround = route.LineCost;
                                        }
                                        else
                                        {
                                            CountOfround = (route.LineCost / 2);

                                        }
                                        var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                        {
                                            TransportationVehicleRouteId = route.Id,
                                            Serial = route.Serial,
                                            DateOfMonth = DateTime.Now,
                                            CountOfRounds = 1,
                                            PriceOfRounds = CountOfround,
                                            CreationBy = 1,
                                            CreationDate = FullDateTime,
                                            ModifiedBy = 1,
                                            ModifiedDate = FullDateTime
                                        });
                                        TransportionLineAccountDb.Add(newAccount);
                                    }
                                    else
                                    {
                                        if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                        {
                                            checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                        }
                                        if ((bool)route.OneWay)
                                        {
                                            checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + route.LineCost;
                                        }
                                        else
                                        {
                                            checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + (route.LineCost / 2);
                                        }
                                        checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                        checkAccount.ModifiedBy = 1;
                                        checkAccount.ModifiedDate = FullDateTime;
                                        // _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                                    }

                                }

                                continue;
                            }


                            //var exitChechOut = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            //if (exitChechOut == null)
                            //{
                            var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn >= day.AddHours(-22) && x.Serial == route.Serial).FirstOrDefault();
                            if (exit != null)
                            {
                                exit.CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault();
                                // _unitOfWork.TransprotationUserAttedances.Update(exit);
                            }
                            else
                            {
                                var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = 1,
                                });
                                userAttandanceDB.Add(newAttendance);

                            }


                            _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                            {
                                Serial = route.Serial,
                                CheckOutDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                TransportationVehicleRouteId = route.Id,
                                PriceRound = (route.LineCost / 2)
                            });

                            checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == FullDateTime.Month).FirstOrDefault();


                            if (checkAccount == null)
                            {
                                decimal CountOfround = 0;
                                if ((bool)route.OneWay)
                                {
                                    CountOfround = route.LineCost;
                                }
                                else
                                {
                                    CountOfround = (route.LineCost / 2);

                                }
                                var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                {
                                    TransportationVehicleRouteId = route.Id,
                                    Serial = route.Serial,
                                    DateOfMonth = DateTime.Now,
                                    CountOfRounds = 1,
                                    PriceOfRounds = CountOfround,
                                    CreationBy = 1,
                                    CreationDate = FullDateTime,
                                    ModifiedBy = 1,
                                    ModifiedDate = FullDateTime
                                });
                                TransportionLineAccountDb.Add(newAccount);
                            }
                            else
                            {
                                if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                {
                                    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                }
                                if ((bool)route.OneWay)
                                {
                                    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + route.LineCost;
                                }
                                else
                                {
                                    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + (route.LineCost / 2);
                                }
                                checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                checkAccount.ModifiedBy = 1;
                                checkAccount.ModifiedDate = FullDateTime;
                                //  _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                            }


                        }
                    }
                }


                _unitOfWork.Complete();

                Response.Result = true;
            }
            catch (Exception ex) { Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message }); }
            return Response;
        }

        public BaseResponse AddUsersAttedance66()
        {
            BaseResponse Response = new BaseResponse { Result = false, Errors = new List<Error>() };
            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var FullDateTime = egyptTime;
                var TransportionLineAccountDb = _unitOfWork.TransportationVehicleRouteAccounts.FindAll(x => x.Id > 0).ToList();


                var userAttandanceDB = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Id > 0).OrderBy(y => y.CreationDate).ToList();

                var userAttandanceCheckOutExit = new List<AttendanceCheckOutVm>();

                var userAttandanceLast = new TransprotationUserAttedance();
                if (userAttandanceDB != null)
                {
                    userAttandanceLast = userAttandanceDB.LastOrDefault();
                }

                var pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true).ToList();
                var userAttandanceLastDate = new DateTime();
                if (userAttandanceLast != null)
                {
                    userAttandanceLastDate = userAttandanceLast.CreationDate.Date;
                    //pharmaAttandance = _ContextTransportation.v_Bus_Program.Where(x => SafeConvertToDateTime(x.CheckTime).Date >= userAttandanceLastDate).ToList();
                    pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true)
                        .AsEnumerable()
                        .Where(x => SafeConvertToDateTime(x.CheckDate).Date >= userAttandanceLastDate)
                         .OrderBy(l => SafeConvertToDateTime(l.CheckDate + " " + l.CheckTime))
                        .ToList();
                }





                var employeeRoutes = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                var routesInfo = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Active == true, new[] { "TransportationVehicle" }).ToList();

                foreach (var log in pharmaAttandance)
                {
                    DateTime logDateTime = SafeConvertToDateTime(log.CheckDate + " " + log.CheckTime);

                    if (logDateTime.Date == new DateTime(2026, 9, 3))
                    {
                    }
                    var userSubscribedRouteIds = employeeRoutes
                        .Where(x => x.HrUser.Email == log.EmployeeNumber.ToString())
                        .Select(x => x.TransportationVehicleRouteId).ToList();

                    if (!userSubscribedRouteIds.Any()) continue;
                    var potentialRoutes = routesInfo.Where(r => userSubscribedRouteIds.Contains(r.Id)).ToList();

                    if (log.Type == "I") // بصمة دخول
                    {
                        // 1. محاولة مطابقة دقيقة (60 دقيقة)
                        var matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 59);

                        //// 2. إذا لم يجد، محاولة مطابقة واسعة (120 دقيقة)
                        //if (matchedRoute == null)
                        //    matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 120);

                        // 3. إذا لم يجد أي أتوبيس قريب، يأخذ أول أتوبيس مسجل له (Fallback) لضمان عدم ضياع البصمة
                        var routeIdToUse = matchedRoute?.Id;

                        var existing = userAttandanceDB
                            .Where(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckIn.Value.Date == logDateTime.Date).FirstOrDefault();

                        if (existing == null)
                        {
                            var existingRecord = userAttandanceCheckOutExit
                             .FirstOrDefault(x =>
                                 x.Serial == log.EmployeeNumber.ToString() &&
                                 x.CheckOut.Date == logDateTime.Date // نفس اليوم فقط
                             );

                            if (existingRecord != null)
                            {
                                if (logDateTime.AddMinutes(30) >= existingRecord.CheckOut)
                                {
                                    continue;
                                }
                            }
                            var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckIn = logDateTime,
                                CheckInRouteId = routeIdToUse,
                                Type = log.BusType,
                                CreationDate = DateTime.Now,
                                CreationBy = 1
                            });
                            userAttandanceDB.Add(newAttendance);
                        }

                    }
                    else if (log.Type == "O") // بصمة خروج
                    {



                        // أولوية 1: البحث عن أي سجل مفتوح خلال 18 ساعة لإغلاقه
                        var openRecord = userAttandanceDB
                            .Where(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckOut == null && x.CheckIn >= logDateTime.AddHours(-14))
                            .OrderByDescending(x => x.CheckIn).FirstOrDefault();

                        // أولوية 2: تحديد أتوبيس الخروج (نطاق واسع 30 دقيقة)
                        var matchedOutRoute = potentialRoutes.FirstOrDefault(r => r.ToDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.ToDate.Value.TimeOfDay).TotalMinutes) <= 30);

                        if (openRecord != null)
                        {
                            openRecord.CheckOut = logDateTime;
                            // نضع أتوبيس الخروج المكتشف، وإذا لم نجد نضع نفس أتوبيس الدخول
                            openRecord.CheckOutRouteId = matchedOutRoute?.Id;
                            //_unitOfWork.TransprotationUserAttedances.Update(openRecord);
                        }
                        //else
                        //{
                        //    userAttandanceIsFound.Add(openRecord.Id);
                        //}
                        else
                        {
                            // إذا لم يجد دخول، ينشئ سجل خروج فقط لكي لا تضيع البصمة
                            var newAttendance = new AttendanceCheckOutVm
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckOut = logDateTime
                            };

                            userAttandanceCheckOutExit.Add(newAttendance);

                        }

                    }
                }


                // 1. تحديد قائمة الأيام التي وردت في البصمات لمعالجتها
                var datesToProcess = pharmaAttandance
                    .Select(x => SafeConvertToDateTime(x.CheckDate).Date)
                    .Distinct()
                    .ToList();

                foreach (var day in datesToProcess)
                {

                    foreach (var route in routesInfo)
                    {
                        if (route.Id > 154)
                        {
                        }
                        // سعة الأتوبيس
                        int capacity = route.TransportationVehicle?.Capacity ?? 0;
                        if (capacity == 0) continue; // تخطي إذا لم توجد سعة مسجلة

                        // 2. حساب عدد الموظفين الذين بصموا دخول في هذا المسار وهذا اليوم
                        var checkInCount = userAttandanceDB
                            .Where(x => x.CheckIn.HasValue && x.CheckIn.Value.Date == day && x.CheckInRouteId == route.Id)
                            .Count();

                        // 3. حساب عدد الموظفين الذين بصموا خروج في هذا المسار وهذا اليوم
                        var checkOutCount = userAttandanceDB
                            .Where(x => x.CheckOut.HasValue && x.CheckOut.Value.Date == day && x.CheckOutRouteId == route.Id)
                            .Count();

                        // 4. تطبيق قاعدة (أكثر من نصف السعة)
                        bool isBusInActive = checkInCount > (capacity / 2);
                        bool isBusOutActive = checkOutCount > (capacity / 2);

                        // 5. هنا يمكنك حفظ النتيجة في جدول (BusAttendance) أو اتخاذ إجراء
                        if (isBusInActive)
                        {
                            if ((bool)route.OneWay)
                            {
                                var exitOneWay = userAttandanceDB.Where(x => x.Type == "Bus" && x.OneWayDate.HasValue && x.OneWayDate?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                                if (exitOneWay == null)
                                {
                                    var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                        Type = "Bus",
                                        CreationDate = DateTime.Now,
                                        CreationBy = 1
                                    });
                                    userAttandanceDB.Add(newAttendance);

                                    _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                        TransportationVehicleRouteId = route.Id,
                                        SupplierId = (long)route.SupplierId
                                        //PriceRound = route.LineCost
                                    });

                                    var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == ((DateTime)newAttendance.OneWayDate).Month).FirstOrDefault();


                                    if (checkAccount == null)
                                    {
                                        //decimal CountOfround = 0;
                                        //if ((bool)route.OneWay)
                                        //{
                                        //    CountOfround = route.LineCost;
                                        //}
                                        //else
                                        //{
                                        //    CountOfround = (route.LineCost / 2);

                                        //}
                                        var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                        {
                                            TransportationVehicleRouteId = route.Id,
                                            Serial = route.Serial,
                                            DateOfMonth = (DateTime)newAttendance.OneWayDate,
                                            CountOfRounds = 1,
                                            // PriceOfRounds = CountOfround,
                                            CreationBy = 1,
                                            CreationDate = FullDateTime,
                                            ModifiedBy = 1,
                                            ModifiedDate = FullDateTime,
                                            SupplierId = (long)route.SupplierId

                                        });
                                        TransportionLineAccountDb.Add(newAccount);
                                    }
                                    else
                                    {
                                        //if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                        //{
                                        //    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                        //}
                                        //if ((bool)route.OneWay)
                                        //{
                                        //    checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + route.LineCost;
                                        //}
                                        //else
                                        //{
                                        //    checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + (route.LineCost / 2);
                                        //}
                                        checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                        checkAccount.ModifiedBy = 1;
                                        checkAccount.ModifiedDate = FullDateTime;
                                        // _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                                    }

                                    continue;
                                }
                            }

                            var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            if (exit == null)
                            {
                                var newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckIn = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = 1
                                });

                                userAttandanceDB.Add(newAttendance);

                                _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                {
                                    Serial = route.Serial,
                                    CheckInDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    TransportationVehicleRouteId = route.Id,
                                    SupplierId = (long)route.SupplierId

                                    // PriceRound = (route.LineCost / 2)

                                });

                                var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == ((DateTime)newAttendance.CheckIn).Month).FirstOrDefault();


                                if (checkAccount == null)
                                {
                                    //decimal CountOfround = 0;
                                    //if ((bool)route.OneWay)
                                    //{
                                    //    CountOfround = route.LineCost;
                                    //}
                                    //else
                                    //{
                                    //    CountOfround = (route.LineCost / 2);

                                    //}
                                    // update not add 
                                    var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                    {
                                        TransportationVehicleRouteId = route.Id,
                                        Serial = route.Serial,
                                        SupplierId = route.SupplierId ?? 0,
                                        DateOfMonth = (DateTime)newAttendance.CheckIn,
                                        CountOfRounds = 1,
                                        // PriceOfRounds = CountOfround,
                                        CreationBy = 1,
                                        CreationDate = FullDateTime,
                                        ModifiedBy = 1,
                                        ModifiedDate = FullDateTime,


                                    });
                                    TransportionLineAccountDb.Add(newAccount);
                                }
                                else
                                {
                                    //if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                    //{
                                    //    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                    //}
                                    //if ((bool)route.OneWay)
                                    //{
                                    //    checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + route.LineCost;
                                    //}
                                    //else
                                    //{
                                    //    checkAccount.PriceOfRounds = checkAccount.PriceOfRounds + (route.LineCost / 2);
                                    //}
                                    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                    checkAccount.ModifiedBy = 1;
                                    checkAccount.ModifiedDate = FullDateTime;
                                    //_unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                                }

                            }
                        }

                        if (isBusOutActive)
                        {
                            var recoredBefore = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckOut?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            if (recoredBefore != null)
                            {
                                continue;
                            }
                            if ((bool)route.OneWay)
                            {
                                var exitOneWay = userAttandanceDB.Where(x => x.Type == "Bus" && x.OneWayDate.HasValue && x.OneWayDate?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                                if (exitOneWay == null)
                                {
                                    var newAttendanceOneWay = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                        Type = "Bus",
                                        CreationDate = DateTime.Now,
                                        CreationBy = 1,
                                    });

                                    userAttandanceDB.Add(newAttendanceOneWay);

                                    _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                                    {
                                        Serial = route.Serial,
                                        OneWayDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                        TransportationVehicleRouteId = route.Id,
                                        SupplierId = (long)route.SupplierId

                                        // PriceRound = route.LineCost

                                    });

                                    var checkAccountOneWay = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == ((DateTime)newAttendanceOneWay.OneWayDate).Month).FirstOrDefault();


                                    if (checkAccountOneWay == null)
                                    {
                                        //decimal CountOfround = 0;
                                        //if ((bool)route.OneWay)
                                        //{
                                        //    CountOfround = route.LineCost;
                                        //}
                                        //else
                                        //{
                                        //    CountOfround = (route.LineCost / 2);

                                        //}
                                        var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                        {
                                            TransportationVehicleRouteId = route.Id,
                                            Serial = route.Serial,
                                            DateOfMonth = (DateTime)newAttendanceOneWay.OneWayDate,
                                            CountOfRounds = 1,
                                            // PriceOfRounds = CountOfround,
                                            CreationBy = 1,
                                            CreationDate = FullDateTime,
                                            ModifiedBy = 1,
                                            ModifiedDate = FullDateTime,
                                            SupplierId = route.SupplierId ?? 0
                                        });
                                        TransportionLineAccountDb.Add(newAccount);
                                    }
                                    else
                                    {
                                        //if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                        //{
                                        //    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                        //}
                                        //if ((bool)route.OneWay)
                                        //{
                                        //    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + route.LineCost;
                                        //}
                                        //else
                                        //{
                                        //    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + (route.LineCost / 2);
                                        //}
                                        checkAccountOneWay.CountOfRounds = checkAccountOneWay.CountOfRounds + 1;
                                        checkAccountOneWay.ModifiedBy = 1;
                                        checkAccountOneWay.ModifiedDate = FullDateTime;
                                        // _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccountOneWay);
                                    }

                                }

                                continue;
                            }


                            //var exitChechOut = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial).FirstOrDefault();
                            //if (exitChechOut == null)
                            //{
                            var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn >= day.AddHours(-14) && x.Serial == route.Serial).FirstOrDefault();

                            var newAttendance = new TransprotationUserAttedance();
                            if (exit != null)
                            {
                                exit.CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault();
                                // _unitOfWork.TransprotationUserAttedances.Update(exit);
                            }
                            else
                            {
                                newAttendance = _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = 1,
                                });
                                userAttandanceDB.Add(newAttendance);

                            }


                            _unitOfWork.TransportationRoundForRoutes.Add(new TransportationRoundForRoute
                            {
                                Serial = route.Serial,
                                CheckOutDate = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                TransportationVehicleRouteId = route.Id,
                                SupplierId = (long)route.SupplierId

                                //PriceRound = (route.LineCost / 2)
                            });

                            var checkAccount = TransportionLineAccountDb.Where(x => x.TransportationVehicleRouteId == route.Id && x.DateOfMonth.Month == ((DateTime)(newAttendance.CheckOut ?? exit.CheckOut)).Month).FirstOrDefault();


                            if (checkAccount == null)
                            {
                                //decimal CountOfround = 0;
                                //if ((bool)route.OneWay)
                                //{
                                //    CountOfround = route.LineCost;
                                //}
                                //else
                                //{
                                //    CountOfround = (route.LineCost / 2);

                                //}
                                var newAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                                {
                                    TransportationVehicleRouteId = route.Id,
                                    Serial = route.Serial,
                                    DateOfMonth = (DateTime)(newAttendance.CheckOut ?? exit.CheckOut),
                                    CountOfRounds = 1,
                                    //  PriceOfRounds = CountOfround,
                                    CreationBy = 1,
                                    CreationDate = FullDateTime,
                                    ModifiedBy = 1,
                                    ModifiedDate = FullDateTime,
                                    SupplierId = route.SupplierId ?? 0
                                });
                                TransportionLineAccountDb.Add(newAccount);
                            }
                            else
                            {
                                //if (checkAccount.DateOfMonth.Date != FullDateTime.Date)
                                //{
                                //    checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;

                                //}
                                //if ((bool)route.OneWay)
                                //{
                                //    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + route.LineCost;
                                //}
                                //else
                                //{
                                //    checkAccount.PriceOfRounds = (checkAccount.PriceOfRounds ?? 0) + (route.LineCost / 2);
                                //}
                                checkAccount.CountOfRounds = checkAccount.CountOfRounds + 1;
                                checkAccount.ModifiedBy = 1;
                                checkAccount.ModifiedDate = FullDateTime;
                                //  _unitOfWork.TransportationVehicleRouteAccounts.Update(checkAccount);
                            }


                        }
                    }
                }


                _unitOfWork.Complete();

                Response.Result = true;
            }
            catch (Exception ex) { Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message }); }
            return Response;
        }

        public BaseResponseWithData<DashBoardVM> DashBoard22(long RouteId, int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus)
        {
            var response = new BaseResponseWithData<DashBoardVM>
            {
                Data = new DashBoardVM(),
                Errors = new List<Error>(),
                Result = true
            };

            try
            {
                if (DateSerach == null)
                {
                    response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "يرجى إدخال التاريخ";
                    response.Errors.Add(error);
                    return response;
                }
                DateTime targetDate = DateSerach.Value.Date;
                var dashboard = new DashBoardVM();
                var attendance = _unitOfWork.TransprotationUserAttedances.FindAll(x =>
        (x.CheckIn.HasValue && x.CheckIn.Value.Date == targetDate) ||
        (x.CheckOut.HasValue && x.CheckOut.Value.Date == targetDate) ||
        (x.OneWayDate.HasValue && x.OneWayDate.Value.Date == targetDate), new[] { "CheckInRoute" }).DistinctBy(y => y.Serial);

                var allEmployees = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0).ToList();
                var vehicleRoutes = GetVehicleRoutes(RouteId, TransportionlineId, SupplierId, supplierContactPersonId, serialBus);

                if (!vehicleRoutes.Any())
                {
                    response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر";
                    response.Errors.Add(error);
                    return response;
                }
                var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0);

                dashboard.VehiclesNum = vehicleRoutes.Count();
                dashboard.TransportLinesNum = vehicleRoutes.Select(x => x.TransportationLineId).Distinct().Count();
                dashboard.VehiclesTypeNum = vehicleRoutes.Select(x => x.TransportationVehicle.VehicleTypeId).Distinct().Count();
                dashboard.Capacity = vehicleRoutes.Sum(x => x.TransportationVehicle.Capacity);
                dashboard.TwoWayVehiclesNum = vehicleRoutes.Where(x => x.OneWay == false).Count();
                dashboard.OneWayVehiclesNum = vehicleRoutes.Where(x => x.OneWay == true).Count();
                var transportationVehicleRouteId = vehicleRoutes.Select(t => t.Id);
                dashboard.AllHrUsersNum = transportationVehicleRouteEmployee.Where(x => x.Active == true).Select(x => x.HrUserId).Distinct().Count();

                dashboard.HrUsersNum = transportationVehicleRouteEmployee.Where(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUserId).Distinct().Count();
                dashboard.SuppliersNum = vehicleRoutes.Select(s => s.SupplierId).Distinct().Count();
                //dashboard.TransportLinesNum = 1;
                var AttedanceBus = attendance.Where(x => vehicleRoutes.Select(y => y.Serial).Contains(x.Serial) && x.Type == "Bus");
                var AttedanceUsers = attendance.Where(x => x.Type == "Person").Count();


                // All
                var AllAttedanceUserIds = attendance
                    .Where(x => x.Type == "Person" && (
            (x.CheckOutRouteId.HasValue &&
             transportationVehicleRouteId.Contains(x.CheckOutRouteId.Value))
            ||
            (x.CheckOutRouteId.HasValue &&
             transportationVehicleRouteId.Contains(x.CheckOutRouteId.Value))
        ))
                    .Select(y => long.Parse(y.Serial))
                    .ToList();
                var AllUsersForLine = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                var emailInHrUsers = _unitOfWork.HrUsers.FindAll(x => AllUsersForLine.Contains(x.Id)).Select(y => y.Email).ToList();
                var commonUserAttedanceAndLine = AllAttedanceUserIds.Intersect(emailInHrUsers.Select(long.Parse)).ToList();
                dashboard.AllHrUsersAttendanceNum = commonUserAttedanceAndLine.Count;


                // chechIn
                var AllAttedanceUserIdsCheckIn = attendance
                   .Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckInRouteId.HasValue && transportationVehicleRouteId.Contains((long)x.CheckInRouteId))
                   .Select(y => long.Parse(y.Serial))
                   .ToList();
                var AllUsersForLineCheckIn = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                var emailInHrUsersCheckIn = _unitOfWork.HrUsers.FindAll(x => AllUsersForLineCheckIn.Contains(x.Id)).Select(y => y.Email).ToList();
                var commonUserAttedanceAndLineCheckIn = AllAttedanceUserIdsCheckIn.Intersect(emailInHrUsersCheckIn.Select(long.Parse)).ToList();
                dashboard.HrUsersAttendanceNum = commonUserAttedanceAndLineCheckIn.Count;

                // chechOut

                var AllAttedanceUserIdsCheckOut = attendance
                   .Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOutRouteId.HasValue && transportationVehicleRouteId.Contains((long)x.CheckOutRouteId))
                   .Select(y => long.Parse(y.Serial))
                   .ToList();
                var AllUsersForLineCheckOut = transportationVehicleRouteEmployee.Where(x => transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(y => (long)y.HrUserId).ToList();
                var emailInHrUsersCheckOut = _unitOfWork.HrUsers.FindAll(x => AllUsersForLineCheckOut.Contains(x.Id)).Select(y => y.Email).ToList();
                var commonUserAttedanceAndLineCheckOut = AllAttedanceUserIdsCheckOut.Intersect(emailInHrUsersCheckOut.Select(long.Parse)).ToList();
                dashboard.HrUsersAttendanceNumCheckOut = commonUserAttedanceAndLineCheckOut.Count;




                dashboard.CheckInVehicleAttendanceNum = AttedanceBus.Where(x => x.CheckIn.HasValue && x.CheckIn?.Date == targetDate).Count();
                dashboard.CheckOutVehicleAttendanceNum = AttedanceBus.Where(x => x.CheckOut.HasValue && x.CheckOut?.Date == targetDate).Count();
                dashboard.OneWayVehicleAttendanceNum = AttedanceBus.Count(x =>
                                                                                     x.OneWayDate?.Date == targetDate &&
                                                                                     x.CheckInRoute?.OneWay == true &&
                                                                                     x.CheckInRoute?.FromoDate != null
                                                                                 );

                dashboard.OneWayVehiclePercentCheckOut = AttedanceBus.Count(x =>
                                                                                                x.OneWayDate?.Date == targetDate &&
                                                                                                x.CheckInRoute?.OneWay == true &&
                                                                                                x.CheckInRoute?.ToDate != null
                                                                                            );
                dashboard.CheckInVehiclePercent = dashboard.CheckInVehicleAttendanceNum == 0 ? 0 : ((decimal)dashboard.CheckInVehicleAttendanceNum / (decimal)dashboard.TwoWayVehiclesNum) * 100;
                dashboard.CheckOutVehiclePercent = dashboard.CheckOutVehicleAttendanceNum == 0 ? 0 : ((decimal)dashboard.CheckOutVehicleAttendanceNum / (decimal)dashboard.TwoWayVehiclesNum) * 100;
                dashboard.OneWayVehiclePercent = dashboard.OneWayVehicleAttendanceNum == 0 ? 0 : ((decimal)dashboard.OneWayVehicleAttendanceNum / (decimal)dashboard.OneWayVehiclesNum) * 100;
                dashboard.OneWayVehiclePercentCheckOut = dashboard.OneWayVehiclePercentCheckOut == 0 ? 0 : ((decimal)dashboard.OneWayVehiclePercentCheckOut / (decimal)dashboard.OneWayVehiclesNum) * 100;

                dashboard.HrUsersPercent = commonUserAttedanceAndLineCheckIn.Count() == 0 ? 0 : ((decimal)commonUserAttedanceAndLineCheckIn.Count() / (decimal)dashboard.HrUsersNum) * 100;
                dashboard.HrUsersOfCapacityPercent = dashboard.Capacity == 0 ? 0 : ((decimal)dashboard.HrUsersAttendanceNum / (decimal)dashboard.Capacity) * 100;

                response.Data = dashboard;
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

        public BaseResponseWithData<DashBoardVM> DashBoard(
    long RouteId,
    int TransportionlineId,
    long SupplierId,
    long supplierContactPersonId,
    DateTime? DateSerach,
    string serialBus)
        {
            var response = new BaseResponseWithData<DashBoardVM>
            {
                Data = new DashBoardVM(),
                Errors = new List<Error>(),
                Result = true
            };

            try
            {
                if (!DateSerach.HasValue)
                {
                    response.Result = false;
                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err10",
                        ErrorMSG = "يرجى إدخال التاريخ"
                    });

                    return response;
                }

                var targetDate = DateSerach.Value.Date;
                var dashboard = new DashBoardVM();

                // ---------------------------------------------------------
                // Vehicle Routes
                // ---------------------------------------------------------

                var vehicleRoutes = GetVehicleRoutes(
                    RouteId,
                    TransportionlineId,
                    SupplierId,
                    supplierContactPersonId,
                    serialBus
                ).ToList();

                if (vehicleRoutes.Count == 0)
                {
                    response.Result = false;

                    response.Errors.Add(new Error
                    {
                        ErrorCode = "Err10",
                        ErrorMSG = "لا يوجد مسارات نشطة لهذا الفلتر"
                    });

                    return response;
                }

                // HashSet gives much faster Contains()
                var vehicleRouteIds = vehicleRoutes
                    .Select(x => x.Id)
                    .ToHashSet();

                var vehicleSerials = vehicleRoutes
                    .Select(x => x.Serial)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToHashSet();

                // ---------------------------------------------------------
                // Employees
                // ---------------------------------------------------------

                var transportationEmployees =
                    _unitOfWork.TransportationVehicleRouteEmployees
                        .FindAllQueryable(x => x.Id > 0)
                        .ToList();

                var lineEmployees = transportationEmployees
                    .Where(x =>
                        x.Active == true &&
                        vehicleRouteIds.Contains(x.TransportationVehicleRouteId))
                    .Select(x => (long)x.HrUserId)
                    .Distinct()
                    .ToHashSet();

                var allActiveEmployees = transportationEmployees
                    .Where(x => x.Active == true)
                    .Select(x => (long)x.HrUserId)
                    .Distinct()
                    .Count();

                dashboard.AllHrUsersNum = allActiveEmployees;
                dashboard.HrUsersNum = lineEmployees.Count;

                // ---------------------------------------------------------
                // Dashboard Vehicle Information
                // ---------------------------------------------------------

                dashboard.VehiclesNum = vehicleRoutes.Count;

                dashboard.TransportLinesNum = vehicleRoutes
                    .Select(x => x.TransportationLineId)
                    .Distinct()
                    .Count();

                dashboard.VehiclesTypeNum = vehicleRoutes
                    .Select(x => x.TransportationVehicle.VehicleTypeId)
                    .Distinct()
                    .Count();

                dashboard.Capacity = vehicleRoutes
                    .Sum(x => x.TransportationVehicle.Capacity);

                dashboard.TwoWayVehiclesNum = vehicleRoutes
                    .Count(x => x.OneWay == false);

                dashboard.OneWayVehiclesNum = vehicleRoutes
                    .Count(x => x.OneWay == true);

                dashboard.SuppliersNum = vehicleRoutes
                    .Select(x => x.SupplierId)
                    .Distinct()
                    .Count();

                // ---------------------------------------------------------
                // Attendance
                // ---------------------------------------------------------

                var attendance = _unitOfWork.TransprotationUserAttedances.FindAll(
                    x =>
                        (x.CheckIn.HasValue &&
                         x.CheckIn.Value.Date == targetDate)
                        ||
                        (x.CheckOut.HasValue &&
                         x.CheckOut.Value.Date == targetDate)
                        ||
                        (x.OneWayDate.HasValue &&
                         x.OneWayDate.Value.Date == targetDate),
                    new[] { "CheckInRoute" }
                )
                .ToList();

                var attendanceBus = attendance
                    .Where(x =>
                        x.Type == "Bus" &&
                        !string.IsNullOrWhiteSpace(x.Serial) &&
                        vehicleSerials.Contains(x.Serial))
                    .ToList();

                var attendanceUsers = attendance
                    .Where(x => x.Type == "Person")
                    .ToList();

                // ---------------------------------------------------------
                // HR Users
                // ---------------------------------------------------------

                var hrUserIds = lineEmployees.ToList();

                var hrUserEmails = _unitOfWork.HrUsers
                    .FindAll(x => hrUserIds.Contains(x.Id))
                    .Select(x => x.Email)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                var hrUserEmailIds = new HashSet<long>();

                foreach (var email in hrUserEmails)
                {
                    if (long.TryParse(email, out var id))
                    {
                        hrUserEmailIds.Add(id);
                    }
                }

                // ---------------------------------------------------------
                // Person Attendance
                // ---------------------------------------------------------

                var allAttendanceUserIds = attendanceUsers
                    .Where(x =>
                        (x.CheckOutRouteId.HasValue &&
                         vehicleRouteIds.Contains(x.CheckOutRouteId.Value))
                        ||
                        (x.CheckInRouteId.HasValue &&
                         vehicleRouteIds.Contains(x.CheckInRouteId.Value))
                    )
                    .Select(x => long.TryParse(x.Serial, out var id) ? (long?)id : null)
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .ToHashSet();

                dashboard.AllHrUsersAttendanceNum =
                    allAttendanceUserIds.Intersect(hrUserEmailIds).Count();

                // ---------------------------------------------------------
                // Check In
                // ---------------------------------------------------------

                var checkInAttendanceUserIds = attendanceUsers
                    .Where(x =>
                        x.CheckIn.HasValue &&
                        x.CheckInRouteId.HasValue &&
                        vehicleRouteIds.Contains(x.CheckInRouteId.Value))
                    .Select(x => long.TryParse(x.Serial, out var id) ? (long?)id : null)
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .ToHashSet();

                dashboard.HrUsersAttendanceNum =
                    checkInAttendanceUserIds.Intersect(hrUserEmailIds).Count();

                // ---------------------------------------------------------
                // Check Out
                // ---------------------------------------------------------

                var checkOutAttendanceUserIds = attendanceUsers
                    .Where(x =>
                        x.CheckOut.HasValue &&
                        x.CheckOutRouteId.HasValue &&
                        vehicleRouteIds.Contains(x.CheckOutRouteId.Value))
                    .Select(x => long.TryParse(x.Serial, out var id) ? (long?)id : null)
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .ToHashSet();

                dashboard.HrUsersAttendanceNumCheckOut =
                    checkOutAttendanceUserIds.Intersect(hrUserEmailIds).Count();

                // ---------------------------------------------------------
                // Vehicle Attendance
                // ---------------------------------------------------------

                dashboard.CheckInVehicleAttendanceNum =
                    attendanceBus.Count(x =>
                        x.CheckIn?.Date == targetDate);

                dashboard.CheckOutVehicleAttendanceNum =
                    attendanceBus.Count(x =>
                        x.CheckOut?.Date == targetDate);

                dashboard.OneWayVehicleAttendanceNum =
                    attendanceBus.Count(x =>
                        x.OneWayDate?.Date == targetDate &&
                        x.CheckInRoute?.OneWay == true &&
                        x.CheckInRoute?.FromoDate != null);

                dashboard.OneWayVehiclePercentCheckOut =
                    attendanceBus.Count(x =>
                        x.OneWayDate?.Date == targetDate &&
                        x.CheckInRoute?.OneWay == true &&
                        x.CheckInRoute?.ToDate != null);

                // ---------------------------------------------------------
                // Percentages
                // ---------------------------------------------------------

                dashboard.CheckInVehiclePercent =
                    dashboard.TwoWayVehiclesNum == 0
                        ? 0
                        : (decimal)dashboard.CheckInVehicleAttendanceNum
                          / dashboard.TwoWayVehiclesNum * 100;

                dashboard.CheckOutVehiclePercent =
                    dashboard.TwoWayVehiclesNum == 0
                        ? 0
                        : (decimal)dashboard.CheckOutVehicleAttendanceNum
                          / dashboard.TwoWayVehiclesNum * 100;

                dashboard.OneWayVehiclePercent =
                    dashboard.OneWayVehiclesNum == 0
                        ? 0
                        : (decimal)dashboard.OneWayVehicleAttendanceNum
                          / dashboard.OneWayVehiclesNum * 100;

                dashboard.OneWayVehiclePercentCheckOut =
                    dashboard.OneWayVehiclesNum == 0
                        ? 0
                        : (decimal)dashboard.OneWayVehiclePercentCheckOut
                          / dashboard.OneWayVehiclesNum * 100;

                dashboard.HrUsersPercent =
                    dashboard.HrUsersNum == 0
                        ? 0
                        : (decimal)dashboard.HrUsersAttendanceNum
                          / dashboard.HrUsersNum * 100;

                dashboard.HrUsersOfCapacityPercent =
                    dashboard.Capacity == 0
                        ? 0
                        : (decimal)dashboard.HrUsersAttendanceNum
                          / dashboard.Capacity * 100;

                response.Data = dashboard;

                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;

                response.Errors.Add(new Error
                {
                    ErrorCode = "Err10",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });

                return response;
            }
        }

        public IQueryable<TransportationVehicleRoute> GetVehicleRoutes(long RouteId, int lineId, long supplierId, long contactPersonId, string serial)
        {
            var routeDB = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Active && x.Active == true, new[] { "TransportationLine", "TransportationVehicle" });

            if (lineId > 0)
                return routeDB.Where(x => x.TransportationLineId == lineId);
            if (RouteId > 0)
                return routeDB.Where(x => x.Id == RouteId);
            if (supplierId > 0)
                return routeDB.Where(x => x.SupplierId == supplierId);

            if (contactPersonId > 0)
                return routeDB.Where(x => x.SupplierContactPersonId == contactPersonId);

            if (!string.IsNullOrEmpty(serial))
                return routeDB.Where(x => x.Serial == serial);

            // Default: return all active routes
            return routeDB.Where(x => x.Active);
        }

        public BaseResponse UpdateDeduction(TransportationVehicleRouteDeduction dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;
            try
            {
                if (dto.Id <= 0)
                {
                    // Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter";
                    Response.Errors.Add(error);
                }
                var oldDeduction = _unitOfWork.TransportationVehicleRouteDeductions.FindAll(x => x.Id == dto.Id).FirstOrDefault();
                if (oldDeduction == null)
                {
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "enter correct Id";
                    Response.Errors.Add(error);
                }

                oldDeduction.SupplierId = dto.SupplierId;
                oldDeduction.TransportationVehicleRouteId = dto.TransportationVehicleRouteId;
                oldDeduction.Serial = dto.Serial;
                oldDeduction.DateOfDeduction = dto.DateOfDeduction;
                oldDeduction.Cause = dto.Cause;
                oldDeduction.DeductPerRound = dto.DeductPerRound;
                oldDeduction.TypeOfDedct = dto.TypeOfDedct;
                _unitOfWork.TransportationVehicleRouteDeductions.Update(oldDeduction);

                _unitOfWork.Complete();
                Response.Result = true;
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


        public async Task<BaseResponseWithData<string>> AttendanceExcell(int Year, int TransportionlineId, long RouteId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, DateTime? FromDate, DateTime? ToDate, bool? AttendaceFlag, long HrUser, string? Email, int PageNo, int NoOfItems)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                // 1. جلب بيانات العميل (Logo, Phone, etc.)
                var client = _unitOfWork.Clients.FindAll(x => x.OwnerCoProfile == true).FirstOrDefault();
                if (client == null)
                {
                    Response.Result = false;
                    Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = "client not found" });
                    return Response;
                }

                var clientPhone = _unitOfWork.ClientPhones.FindAll(x => x.ClientId == client.Id).Select(y => y.Phone).FirstOrDefault();
                var clientAddress = _unitOfWork.ClientAddresses.FindAll(x => x.ClientId == client.Id).FirstOrDefault();
                var createdUser = _unitOfWork.Users.FindAll(x => x.Id == client.CreatedBy).FirstOrDefault();

                // 2. جلب الخطوط المطلوبة
                var transportionLineDb = _unitOfWork.TransportationLines.FindAll(x => x.Id > 0);
                if (TransportionlineId > 0) transportionLineDb = transportionLineDb.Where(x => x.Id == TransportionlineId);
                if (SupplierId > 0)
                {
                    var lineIds = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.SupplierId == SupplierId).Select(s => s.TransportationLineId).ToList();
                    transportionLineDb = transportionLineDb.Where(x => lineIds.Contains(x.Id));
                }

                ExcelPackage excel = new ExcelPackage();
                int sheetCounter = 1;

                foreach (var item in transportionLineDb)
                {
                    // استدعاء الميثود الجديدة التي تعيد قائمة الموظفين وبداخلهم أيام الحضور
                    var hrUsers = DashBoardForAttendenceDuration(item.Id, RouteId, SupplierId, supplierContactPersonId, DateSerach, serialBus, FromDate, ToDate, AttendaceFlag, HrUser, Email, PageNo, NoOfItems).Data;

                    var sheet = excel.Workbook.Worksheets.Add($"{item.LineName}");
                    sheet.View.RightToLeft = true; // لضبط الاتجاه للعربية

                    // --- التنسيق العام والعناوين ---
                    sheet.Cells[5, 1].Value = $"تقرير الحضور والغياب التفصيلي للفترة";
                    sheet.Cells[5, 1, 5, 6].Merge = true;
                    sheet.Cells[5, 1].Style.Font.Bold = true;
                    sheet.Cells[5, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[6, 4].Value = $"خط: {item.LineName}";
                    sheet.Cells[6, 3].Value = "الفترة من:";
                    sheet.Cells[6, 2].Value = FromDate?.ToString("yyyy-MM-dd") ?? DateSerach?.ToString("yyyy-MM-dd");
                    sheet.Cells[6, 1].Value = $"إلى: {ToDate?.ToString("yyyy-MM-dd")}";

                    // إضافة بيانات العميل (الهيدر)
                    sheet.Cells[1, 3].Value = client.Name;
                    sheet.Cells[2, 3].Value = $"الهاتف: {clientPhone}";
                    sheet.Cells[1, 3, 4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // --- رؤوس الأعمدة (Row 8) ---
                    string[] headers = { "اسم الموظف", "التاريخ", "حاضر", "غائب", "وقت الحضور", "وقت الانصراف" };
                    for (int col = 0; col < headers.Length; col++)
                    {
                        var cell = sheet.Cells[8, col + 1];
                        cell.Value = headers[col];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // --- تعبئة البيانات (البحث في القائمة المتداخلة) ---
                    int y = 9;
                    if (hrUsers != null && hrUsers.Count > 0)
                    {
                        foreach (var user in hrUsers)
                        {
                            // الحلقة الداخلية للأيام
                            foreach (var day in user.AttendanceHistory)
                            {
                                sheet.Cells[y, 1].Value = $"{user.FirstName} {user.LastName}";
                                sheet.Cells[y, 2].Value = day.Date.ToString("yyyy-MM-dd");
                                sheet.Cells[y, 3].Value = day.ISAttendace ? "✔" : "";
                                sheet.Cells[y, 4].Value = !day.ISAttendace ? "✘" : "";
                                sheet.Cells[y, 5].Value = day.CheckIn?.ToString("HH:mm");
                                sheet.Cells[y, 6].Value = day.CheckOut?.ToString("HH:mm");

                                // تنسيق السطر
                                sheet.Row(y).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[y, 1, y, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                y++;
                            }
                        }
                    }

                    // ضبط عرض الأعمدة
                    sheet.Column(1).Width = 30; // الاسم
                    sheet.Column(2).Width = 15; // التاريخ
                    sheet.Cells.Style.Font.Size = 10;

                    // إضافة اللوجو (فقط في أول شيت)
                    if (sheetCounter == 1 && !string.IsNullOrEmpty(client.LogoUrl))
                    {
                        try
                        {
                            string imagePath = Path.Combine(_environment.WebRootPath, client.LogoUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));
                            if (File.Exists(imagePath))
                            {
                                var picture = sheet.Drawings.AddPicture("Logo", new FileInfo(imagePath));
                                picture.SetPosition(0, 0, 0, 0);
                                picture.SetSize(120, 60);
                            }
                        }
                        catch { /* تجاوز خطأ الصورة */ }
                    }

                    sheetCounter++;
                }

                // 3. حفظ الملف
                var path = "TransportLineExcel";
                var savedPath = Path.Combine(_environment.WebRootPath, path);
                if (!Directory.Exists(savedPath)) Directory.CreateDirectory(savedPath);

                var fileName = $"AttendanceReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var fullPath = Path.Combine(savedPath, fileName);
                excel.SaveAs(new FileInfo(fullPath));

                Response.Data = Globals.baseURL + "/" + path + "/" + fileName;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }


        public async Task<BaseResponseWithData<string>> InsertPharmaExcel(InsertHrUserExcelVM dto)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            var errorList = new List<string>();

            try
            {
                if (dto == null)
                    throw new Exception("Object is not received...");

                if (dto.ExcelSheet == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_host.WebRootPath, "ReceivedReports");

                // Get Data set
                DataSet competitionDayUserDs = Extension.CreateDsOfExcelFile(dto.ExcelSheet, dirPath);

                var allUsers = _unitOfWork.HrUsers.FindAll(x => x.Id > 0);
                var allEmails = new HashSet<string>(allUsers.Select(u => u.Email));
                var allPhoneNumbers = new HashSet<string>(allUsers.Select(u => u.Mobile));
                var allNames = new HashSet<string>(allUsers.Select(u => u.FirstName + " " + u.MiddleName + " " + u.LastName));
                var AllnewHrUser = new List<HrUser>();
                List<string> errorMessages = new List<string>();

                if (competitionDayUserDs != null && competitionDayUserDs.Tables.Count > 0)
                {
                    DataTable competitionDayUsers = competitionDayUserDs.Tables[0];



                    // Process each row in the Excel file
                    for (int i = 1; i < competitionDayUsers.Rows.Count; i++)
                    {
                        var row = competitionDayUsers.Rows[i];


                        var FirstName = row[1].ToString();
                        var MiddleName = row[2].ToString();
                        var LastName = row[3].ToString();
                        var Mobile = row[4].ToString();
                        var Serial = row[5].ToString();
                        var Gender = row[7].ToString();


                        // Validation
                        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(MiddleName)
                            || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Mobile)
                            || string.IsNullOrWhiteSpace(Serial))

                        {
                            errorMessages.Add($"This User {FirstName} {MiddleName} {LastName} Some Parameters is Empty");
                            continue;
                        }

                        if (allEmails.Contains(Serial))
                        {
                            errorMessages.Add($"This User {FirstName} {MiddleName} {LastName} Serial is already registered");
                            continue;
                        }

                        if (allPhoneNumbers.Contains(Mobile))
                        {
                            errorMessages.Add($"This User {FirstName} {MiddleName} {LastName} Mobile is already registered");
                            continue;
                        }

                        if (allNames.Contains(FirstName + " " + MiddleName + " " + LastName))
                        {
                            errorMessages.Add($"This User {FirstName} {MiddleName} {LastName} FullName is already registered");
                            continue;
                        }

                        HrUser newHrUser = new HrUser
                        {
                            FirstName = FirstName,
                            MiddleName = MiddleName,
                            LastName = LastName,
                            Mobile = Mobile,
                            Email = Serial,
                            Gender = Gender,
                            ArlastName = LastName,
                            CreatedById = validation.userID,
                            ModifiedById = validation.userID,
                            CreationDate = DateTime.Now,
                            Modified = DateTime.Now

                        };
                        AllnewHrUser.Add(newHrUser);


                    }

                    await _unitOfWork.HrUsers.AddRangeAsync(AllnewHrUser);

                }
                _unitOfWork.Complete();


                string LOGPath = null;
                if (errorMessages.Any())
                {
                    if (string.IsNullOrEmpty(_host.WebRootPath))
                        throw new InvalidOperationException("WebRootPath is null or empty.");

                    string logsDirectory = Path.Combine(_host.WebRootPath, "Logs");
                    string fileName = "PharmaErrorLog.txt";
                    string fullFilePath = Path.Combine(logsDirectory, fileName);

                    // ✅ Ensure the Logs directory exists (create if missing)
                    if (!Directory.Exists(logsDirectory))
                    {
                        Directory.CreateDirectory(logsDirectory);
                    }


                    using (StreamWriter writer = new StreamWriter(fullFilePath, append: false, encoding: Encoding.UTF8))
                    {
                        foreach (var error in errorMessages)
                        {
                            writer.WriteLine("Error message: " + error);
                        }
                    }

                    Response.Data = Globals.baseURL + "/Logs/" + fileName;

                }


                return (Response);
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
        public async Task<BaseResponse> AlterAddUsersAttendance()
        {
            var response = new BaseResponse
            {
                Errors = new List<Error>(),
                Result = false
            };

            try
            {
                var fullDateTime = GetCurrentEgyptTime();
                var pharmaAttendance = new List<VBusProgram>();

                var transportationRoutes = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Active, new[] { "TransportationLine" }).ToList();
                var routeAccounts = _unitOfWork.TransportationVehicleRouteAccounts.FindAll(x => true).ToList();

                // Get the maximum (latest) date from the data
                var lastDate = _unitOfWork.TransprotationUserAttedances
                    .GetMaxDateTime(x => x.CreationDate.Date);

                // Get all records that match that date
                var attendanceRecords = _unitOfWork.TransprotationUserAttedances
                   .FindAll(x => x.CreationDate.Date == lastDate).ToList() ?? null;




                //var attendanceRecords = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.CreationDate >= x.).OrderBy(y => y.CreationDate).ToList();

                // var lastAttendanceDate = attendanceRecords.LastOrDefault()?.CreationDate.Date ?? DateTime.MinValue;

                //if (attendanceRecords.LastOrDefault() != null)
                //{
                //    attendanceRecords = attendanceRecords.Where(x => x.CreationDate.Date >= lastDate?.Date).ToList();

                //}
                if (lastDate != null)
                {
                    pharmaAttendance = _unitOfWork.VBusProgram
                   .FindAll(x => SafeConvertToDateTime(x.CheckDate).Date >= ((DateTime)lastDate).Date)
                   .ToList();
                }
                else
                {
                    pharmaAttendance = _unitOfWork.VBusProgram
                   .FindAll(x => true)
                   .ToList();
                }

                foreach (var entry in pharmaAttendance)
                {
                    var checkTime = SafeConvertToDateTime(entry.CheckDate);
                    var attendance = attendanceRecords.FirstOrDefault(x =>
                        x.CreationDate.Date == checkTime.Date &&
                        x.Serial == entry.EmployeeNumber.ToString());

                    if (attendance != null)
                    {
                        UpdateExistingAttendance(attendance, entry, checkTime, fullDateTime, transportationRoutes, routeAccounts);
                    }
                    else
                    {
                        var newAttendance = CreateNewAttendance(entry, checkTime);
                        _unitOfWork.TransprotationUserAttedances.Add(newAttendance);

                        if (entry.BusType == "Bus")
                        {
                            UpdateOrCreateRouteAccount(attendance, entry, fullDateTime, checkTime, transportationRoutes, routeAccounts);
                        }

                        // Refresh in-memory list to reflect newly added record (optional)
                        attendanceRecords.Add(newAttendance);
                    }
                }
                await _unitOfWork.CompleteAsync();

                response.Result = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error
                {
                    ErrorCode = "Err10",
                    ErrorMSG = ex.InnerException?.Message ?? ex.Message
                });
            }

            return response;
        }


        public BaseResponse AddUsersAttedanceUpdated()
        {
            BaseResponse Response = new BaseResponse { Result = false, Errors = new List<Error>() };
            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                var FullDateTime = egyptTime;

                var userAttandanceDB = _unitOfWork.TransprotationUserAttedances.FindAll(x => x.Id > 0).OrderBy(y => y.CreationDate);

                var userAttandanceLast = new TransprotationUserAttedance();
                if (userAttandanceDB != null)
                {
                    userAttandanceLast = userAttandanceDB.LastOrDefault();
                }

                var pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true).ToList();
                var userAttandanceLastDate = new DateTime();
                if (userAttandanceLast != null)
                {
                    userAttandanceLastDate = userAttandanceLast.CreationDate.Date;
                    //pharmaAttandance = _ContextTransportation.v_Bus_Program.Where(x => SafeConvertToDateTime(x.CheckTime).Date >= userAttandanceLastDate).ToList();
                    pharmaAttandance = _unitOfWork.VBusProgram.FindAll(x => true)
                        .AsEnumerable()
                        .Where(x => SafeConvertToDateTime(x.CheckDate).Date >= userAttandanceLastDate)
                         .OrderBy(l => SafeConvertToDateTime(l.CheckDate + " " + l.CheckTime))
                        .ToList();
                }





                var employeeRoutes = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();
                var routesInfo = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Active == true, new[] { "TransportationVehicle" }).ToList();

                foreach (var log in pharmaAttandance)
                {
                    DateTime logDateTime = SafeConvertToDateTime(log.CheckDate + " " + log.CheckTime);

                    var userSubscribedRouteIds = employeeRoutes
                        .Where(x => x.HrUser.Email == log.EmployeeNumber.ToString())
                        .Select(x => x.TransportationVehicleRouteId).ToList();

                    if (!userSubscribedRouteIds.Any()) continue;
                    var potentialRoutes = routesInfo.Where(r => userSubscribedRouteIds.Contains(r.Id)).ToList();

                    if (log.Type == "I") // بصمة دخول
                    {
                        // 1. محاولة مطابقة دقيقة (60 دقيقة)
                        var matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 60);

                        // 2. إذا لم يجد، محاولة مطابقة واسعة (120 دقيقة)
                        if (matchedRoute == null)
                            matchedRoute = potentialRoutes.FirstOrDefault(r => r.FromoDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.FromoDate.Value.TimeOfDay).TotalMinutes) <= 120);

                        // 3. إذا لم يجد أي أتوبيس قريب، يأخذ أول أتوبيس مسجل له (Fallback) لضمان عدم ضياع البصمة
                        var routeIdToUse = matchedRoute?.Id;

                        var existing = _unitOfWork.TransprotationUserAttedances
                            .FindAll(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckIn.Value.Date == logDateTime.Date).FirstOrDefault();

                        if (existing == null)
                        {
                            _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckIn = logDateTime,
                                CheckInRouteId = routeIdToUse,
                                Type = log.BusType,
                                CreationDate = DateTime.Now,
                                CreationBy = validation.userID
                            });
                            _unitOfWork.Complete();
                        }
                    }
                    else if (log.Type == "O") // بصمة خروج
                    {
                        // أولوية 1: البحث عن أي سجل مفتوح خلال 18 ساعة لإغلاقه
                        var openRecord = _unitOfWork.TransprotationUserAttedances
                            .FindAll(x => x.Serial == log.EmployeeNumber.ToString() && x.CheckOut == null && x.CheckIn >= logDateTime.AddHours(-22))
                            .OrderByDescending(x => x.CheckIn).FirstOrDefault();

                        // أولوية 2: تحديد أتوبيس الخروج (نطاق واسع 120 دقيقة)
                        var matchedOutRoute = potentialRoutes.FirstOrDefault(r => r.ToDate.HasValue && Math.Abs((logDateTime.TimeOfDay - r.ToDate.Value.TimeOfDay).TotalMinutes) <= 30);

                        if (openRecord != null)
                        {
                            openRecord.CheckOut = logDateTime;
                            // نضع أتوبيس الخروج المكتشف، وإذا لم نجد نضع نفس أتوبيس الدخول
                            openRecord.CheckOutRouteId = matchedOutRoute?.Id;
                            _unitOfWork.TransprotationUserAttedances.Update(openRecord);
                        }
                        else
                        {
                            // إذا لم يجد دخول، ينشئ سجل خروج فقط لكي لا تضيع البصمة
                            _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                            {
                                Serial = log.EmployeeNumber.ToString(),
                                CheckOut = logDateTime,
                                CheckOutRouteId = matchedOutRoute?.Id,
                                Type = log.BusType,
                                CreationDate = DateTime.Now,
                                CreationBy = validation.userID

                            });
                        }
                        _unitOfWork.Complete();
                    }
                }

                foreach (var route in routesInfo)
                {
                    var busDateCheck = pharmaAttandance.FirstOrDefault(x => true).CheckDate;
                    var personCount = userAttandanceDB.Where(x => x.CheckIn >= userAttandanceLastDate.Date && x.Type == "Person" && x.CheckInRouteId == (long)route.Id).Count();
                    if (personCount > (route.TransportationVehicle.Capacity / 2))
                    {
                    }

                }


                // 1. تحديد قائمة الأيام التي وردت في البصمات لمعالجتها
                var datesToProcess = pharmaAttandance
                    .Select(x => SafeConvertToDateTime(x.CheckDate).Date)
                    .Distinct()
                    .ToList();

                foreach (var day in datesToProcess)
                {
                    foreach (var route in routesInfo)
                    {
                        // سعة الأتوبيس
                        int capacity = route.TransportationVehicle?.Capacity ?? 0;
                        if (capacity == 0) continue; // تخطي إذا لم توجد سعة مسجلة

                        // 2. حساب عدد الموظفين الذين بصموا دخول في هذا المسار وهذا اليوم
                        var checkInCount = _unitOfWork.TransprotationUserAttedances
                            .FindAll(x => x.CheckIn.HasValue && x.CheckIn.Value.Date == day && x.CheckInRouteId == route.Id)
                            .Count();

                        // 3. حساب عدد الموظفين الذين بصموا خروج في هذا المسار وهذا اليوم
                        var checkOutCount = _unitOfWork.TransprotationUserAttedances
                            .FindAll(x => x.CheckOut.HasValue && x.CheckOut.Value.Date == day && x.CheckOutRouteId == route.Id)
                            .Count();

                        // 4. تطبيق قاعدة (أكثر من نصف السعة)
                        bool isBusInActive = checkInCount > (capacity / 2);
                        bool isBusOutActive = checkOutCount > (capacity / 2);

                        // 5. هنا يمكنك حفظ النتيجة في جدول (BusAttendance) أو اتخاذ إجراء
                        if (isBusInActive)
                        {

                            var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.Serial == route.Serial);
                            if (exit == null)
                            {
                                _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckIn = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckIn.HasValue && x.CheckIn?.Date == day && x.CheckInRouteId == route.Id).Select(w => w.CheckIn).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = validation.userID
                                });
                                continue;
                            }
                        }

                        if (isBusOutActive)
                        {
                            var exitChechOut = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.Serial == route.Serial);
                            if (exitChechOut == null)
                            {
                                var exit = userAttandanceDB.Where(x => x.Type == "Bus" && x.CheckIn.HasValue && x.CheckIn >= day.AddHours(-22) && x.Serial == route.Serial).FirstOrDefault();
                                if (exit != null)
                                {
                                    exit.CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault();
                                    _unitOfWork.TransprotationUserAttedances.Update(exit);
                                }
                                _unitOfWork.TransprotationUserAttedances.Add(new TransprotationUserAttedance
                                {
                                    Serial = route.Serial,
                                    CheckOut = userAttandanceDB.Where(x => x.Type == "Person" && x.CheckOut.HasValue && x.CheckOut?.Date == day && x.CheckOutRouteId == route.Id).Select(w => w.CheckOut).FirstOrDefault(),
                                    Type = "Bus",
                                    CreationDate = DateTime.Now,
                                    CreationBy = validation.userID
                                });
                                continue;
                            }
                        }
                    }
                }




                Response.Result = true;
            }
            catch (Exception ex) { Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message }); }
            return Response;
        }

        public async Task<BaseResponse> CreateHrUserWithAllRoutes(CreateHrUserWithAllRoutesDto NewHrUser, long UserId, string CompanyName)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var HrUser = await HrUserService.CreateHrUser(NewHrUser.HrUserDto, UserId, CompanyName);
                if (HrUser.Errors.Count >= 1)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = HrUser.Errors[0].ErrorMSG;
                    Response.Errors.Add(error);
                    return Response;

                }

                _unitOfWork.Complete();

                if (HrUser.ID <= 0)
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "add parameter HrUser";
                    Response.Errors.Add(error);
                    return Response;

                }

                var AllTransportationEmployee = new List<TransportationVehicleRouteEmployee>();
                var RouteEmployeesDB = await _unitOfWork.TransportationVehicleRouteEmployees.FindAllAsync(x => x.Id > 0);

                if (NewHrUser.Data != null)
                {
                    foreach (var item in NewHrUser.Data)
                    {
                        var checkEmployeeFound = RouteEmployeesDB.Where(x => x.HrUserId == HrUser.ID && x.TransportationVehicleRouteId == item.RouteId).FirstOrDefault();
                        if (checkEmployeeFound != null)
                        {
                            //Response.Result = false;
                            //Error error = new Error();
                            //error.ErrorCode = "Err10";
                            //error.ErrorMSG = $" HrUserId = {HrUser} and  TransportationVehicleRouteId  = {item.RouteId} adding before ...remove it and Update";
                            //Response.Errors.Add(error);

                            continue;
                        }

                        var transportationEmployee = new TransportationVehicleRouteEmployee
                        {
                            TransportationVehicleRouteId = item.RouteId,
                            HrUserId = HrUser.ID,
                            Active = true,
                            CreationBy = validation.userID,
                            CreationDate = DateTime.Now,
                            TransportationVehicleRouteDirectionId = item.TransportationVehicleRouteDirectionId,
                            FromDate = DateTime.Now,
                            ToDate = item.ToDate,
                            DurationLatitude = item.DurationLatitude,
                            DurationLongtitud = item.DurationLongtitud,
                            Period = item.Period,
                        };
                        AllTransportationEmployee.Add(transportationEmployee);


                    }
                    _unitOfWork.TransportationVehicleRouteEmployees.AddRange(AllTransportationEmployee);

                }


                _unitOfWork.Complete();


                Response.Result = true;
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



        public BaseResponseWithData<string> downloadExcelUserActiveTemplete()
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();


            var filePath = System.IO.Path.Combine(this._environment.WebRootPath, "Attachments\\PharmaTemplate\\UserActive.xlsx");


            var HrUsersDb = _unitOfWork.HrUsers.FindAll(a => a.Active).ToList();

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                    //fill the list of InventoryItemCategory in Excel file to let the user choose from them

                    int row = 2;

                    foreach (var item in HrUsersDb)
                    {
                        sheet.Cells[row, 1].Value = item.Email;
                        sheet.Cells[row, 2].Value = item.FirstName;
                        sheet.Cells[row, 3].Value = item.Active;
                        row++;
                    }



                    var dirPath = System.IO.Path.Combine(this._environment.WebRootPath, $"Attachments\\PharmaTemplate\\resultTampletes\\");
                    Directory.CreateDirectory(dirPath);

                    var finalFilePath = dirPath + $"UserTemplete.xlsx";
                    package.SaveAs(finalFilePath);
                    package.Dispose();
                    var fixedPath = $"Attachments\\PharmaTemplate\\resultTampletes\\UserTemplete.xlsx";

                    response.Data = Globals.baseURL + "\\" + fixedPath;
                    return response;
                }
                else
                {
                    response.Result = false;
                    response.Errors = new List<Error>();
                    Error error = new Error();
                    error.ErrorCode = "Err11";
                    error.ErrorMSG = "The Templete File is not exsists";
                    response.Errors.Add(error);
                    return response;
                }
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

        public BaseResponseWithData<string> downloadExcelUserWithRoutesTemplete()
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();


            var filePath = System.IO.Path.Combine(this._environment.WebRootPath, "Attachments\\PharmaTemplate\\UserWithRoutes.xlsx");


            var HrUsersDb = _unitOfWork.HrUsers.FindAll(a => a.Active).ToList();
            var RoutesDb = _unitOfWork.TransportationVehicleRoutes.FindAll(a => a.Active).ToList();

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                    //fill the list of InventoryItemCategory in Excel file to let the user choose from them

                    int row = 2;

                    foreach (var item in HrUsersDb)
                    {
                        sheet.Cells[row, 1].Value = item.Email;
                        sheet.Cells[row, 2].Value = item.FirstName;
                        sheet.Cells[row, 3].Value = item.MaritalStatusId;
                        sheet.Cells[row, 4].Value = item.Mobile;
                        sheet.Cells[row, 5].Value = item.Gender;
                        row++;
                    }

                    row = 1000;

                    foreach (var item in RoutesDb)
                    {
                        sheet.Cells[row, 27].Value = item.NameOfRoute;
                        sheet.Cells[row, 28].Value = item.Id;
                        row++;
                    }





                    var dirPath = System.IO.Path.Combine(this._environment.WebRootPath, $"Attachments\\PharmaTemplate\\UserRoutes\\");
                    Directory.CreateDirectory(dirPath);

                    var finalFilePath = dirPath + $"UserWithRoutes.xlsx";
                    package.SaveAs(finalFilePath);
                    package.Dispose();
                    var fixedPath = $"Attachments\\PharmaTemplate\\UserRoutes\\UserWithRoutes.xlsx";

                    response.Data = Globals.baseURL + "\\" + fixedPath;
                    return response;
                }
                else
                {
                    response.Result = false;
                    response.Errors = new List<Error>();
                    Error error = new Error();
                    error.ErrorCode = "Err11";
                    error.ErrorMSG = "The Templete File is not exsists";
                    response.Errors.Add(error);
                    return response;
                }
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

        public BaseResponseWithData<string> downloadExcelRoutesTemplete()
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();


            var filePath = System.IO.Path.Combine(this._environment.WebRootPath, "Attachments\\PharmaTemplate\\RoutesTemplate.xlsx");


            var AllRoutes = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => x.Id > 0, new[] { "TransportationLine", "TransportationVehicle.VehicleType", "Supplier", "SupplierContactPerson" });


            var christianID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "C").Select(c => c.Id).FirstOrDefault();
            var MuslimID = _unitOfWork.MaritalStatus.FindAll(x => x.Id > 0 && x.Name == "M").Select(c => c.Id).FirstOrDefault();

            var RoutesEmployesDB = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.Active == true, new[] { "HrUser" }).ToList();


            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                    //fill the list of InventoryItemCategory in Excel file to let the user choose from them

                    int row = 7;


                    foreach (var item in AllRoutes)
                    {
                        sheet.Cells[row, 1].Value = item.Serial;
                        sheet.Cells[row, 2].Value = item.NameOfRoute;
                        sheet.Cells[row, 3].Value = item.LineCost;
                        sheet.Cells[row, 4].Value = item.OneWay;
                        sheet.Cells[row, 5].Value = item.FromoDate?.ToString("HH:mm") ?? string.Empty;
                        sheet.Cells[row, 6].Value = item.ToDate?.ToString("HH:mm") ?? string.Empty;
                        sheet.Cells[row, 7].Value = item.Supplier.Name;
                        sheet.Cells[row, 8].Value = item.SupplierContactPerson?.Name ?? string.Empty;
                        sheet.Cells[row, 9].Value = item.TransportationVehicle?.VehicleType?.Type ?? string.Empty;
                        sheet.Cells[row, 10].Value = item.TransportationVehicle.Capacity;
                        sheet.Cells[row, 11].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id).Count();
                        sheet.Cells[row, 12].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == christianID).Count();
                        sheet.Cells[row, 13].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == MuslimID).Count();
                        row++;
                    }



                    var dirPath = System.IO.Path.Combine(this._environment.WebRootPath, $"Attachments\\PharmaTemplate\\resultTampletes\\");
                    Directory.CreateDirectory(dirPath);

                    var finalFilePath = dirPath + $"RoutesTemplete.xlsx";
                    package.SaveAs(finalFilePath);
                    package.Dispose();
                    var fixedPath = $"Attachments\\PharmaTemplate\\resultTampletes\\RoutesTemplete.xlsx";

                    response.Data = Globals.baseURL + "\\" + fixedPath;
                    return response;
                }
                else
                {
                    response.Result = false;
                    response.Errors = new List<Error>();
                    Error error = new Error();
                    error.ErrorCode = "Err11";
                    error.ErrorMSG = "The Templete File is not exsists";
                    response.Errors.Add(error);
                    return response;
                }
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

        public async Task<BaseResponseWithData<string>> InsertUsersWithRoutesExcel(IFormFile ExcelSheet)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            var errorList = new List<string>();

            try
            {
                if (ExcelSheet == null)
                    throw new Exception("File is not received...");

                if (ExcelSheet == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_environment.WebRootPath, "ReceivedReports");

                // Get Data set
                DataSet competitionDayUserDs = Extension.CreateDsOfExcelFile(ExcelSheet, dirPath);

                var allUsers = _unitOfWork.HrUsers.FindAllQueryable(x => true);
                var allEmails = new HashSet<string>(allUsers.Select(u => u.Email));
                var allPhoneNumbers = new HashSet<string>(allUsers.Select(u => u.Mobile));
                var allNames = new HashSet<string>(allUsers.Select(u => u.FirstName));
                List<TransportationVehicleRouteEmployee> routeEmployeesToInsert = new List<TransportationVehicleRouteEmployee>();
                List<HrUser> usersToInsert = new List<HrUser>();
                List<string> errorMessages = new List<string>();

                if (competitionDayUserDs != null && competitionDayUserDs.Tables.Count > 0)
                {
                    DataTable competitionDayUsers = competitionDayUserDs.Tables[0];
                    List<HrUser> newUsers = new List<HrUser>();


                    Random random = new Random();

                    // Process each row in the Excel file
                    for (int i = 1; i < competitionDayUsers.Rows.Count; i++)
                    {
                        var row = competitionDayUsers.Rows[i];

                        var email = row[0].ToString().Trim();
                        var firstName = row[1].ToString().Trim();
                        var MaritalStatusId = row[3].ToString().Trim();
                        var Mobile = row[4].ToString().Trim();
                        var Gender = row[5].ToString().Trim();
                        var route_1 = row[7].ToString().Trim();
                        var route_2 = row[9].ToString().Trim();
                        var route_3 = row[11].ToString().Trim();
                        var route_4 = row[13].ToString().Trim();


                        // Validation
                        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(email)
                            || string.IsNullOrWhiteSpace(MaritalStatusId) || (string.IsNullOrWhiteSpace(route_1) &&
                                                                                string.IsNullOrWhiteSpace(route_2) &&
                                                                                string.IsNullOrWhiteSpace(route_3) &&
                                                                                string.IsNullOrWhiteSpace(route_4)))
                        {
                            break;
                        }

                        if (allEmails.Contains(email))
                        {
                            errorMessages.Add($"This User {firstName}  Id is already registered");
                            continue;
                        }

                        if (allPhoneNumbers.Contains(Mobile))
                        {
                            errorMessages.Add($"This User {firstName}  PhoneNumber is already registered");
                            continue;
                        }

                        if (allNames.Contains(firstName))
                        {
                            errorMessages.Add($"This User {firstName} FullName is already registered");
                            continue;
                        }
                        if ((string.IsNullOrWhiteSpace(route_1) && string.IsNullOrWhiteSpace(route_2) && string.IsNullOrWhiteSpace(route_3) && string.IsNullOrWhiteSpace(route_4)))
                        {
                            errorMessages.Add($"This User {firstName}  Routes are Empty");
                            continue;
                        }

                        HrUser newHrUser = new HrUser
                        {
                            FirstName = firstName,
                            ArfirstName = firstName,
                            MiddleName = "-",
                            LastName = "-",
                            ArlastName = "-",
                            Email = email,
                            Mobile = Mobile,
                            Gender = Gender,
                            MaritalStatusId = int.Parse(MaritalStatusId),
                            Active = true,
                            CreatedById = validation.userID,
                            CreationDate = DateTime.Now,
                            Modified = DateTime.Now,
                            ModifiedById = validation.userID,
                        };



                        usersToInsert.Add(newHrUser);

                        AddRoute(route_1);
                        AddRoute(route_2);
                        AddRoute(route_3);
                        AddRoute(route_4);


                        void AddRoute(string route)
                        {
                            if (!string.IsNullOrWhiteSpace(route))
                            {
                                routeEmployeesToInsert.Add(new TransportationVehicleRouteEmployee
                                {
                                    HrUser = newHrUser,
                                    TransportationVehicleRouteId = long.Parse(route),
                                    Active = true,
                                    CreationBy = validation.userID,
                                    CreationDate = DateTime.Now,
                                    FromDate = DateTime.Now
                                });
                            }
                        }


                        // Update HashSets علشان تمنع التكرار داخل نفس الملف
                        allEmails.Add(email);
                        allPhoneNumbers.Add(Mobile);
                        allNames.Add(firstName);

                    }

                    _unitOfWork.HrUsers.AddRange(usersToInsert);

                    _unitOfWork.TransportationVehicleRouteEmployees.AddRange(routeEmployeesToInsert);
                    _unitOfWork.Complete();
                }



                string LOGPath = null;
                if (errorMessages.Any())
                {
                    string logsPath = Path.Combine(_environment.WebRootPath, "Logs");


                    string FileName = $"UsersWithRoutesErrorLog.txt";
                    LOGPath = "Logs/";
                    string SavePath = Path.Combine(_environment.WebRootPath, LOGPath);
                    if (!System.IO.Directory.Exists(SavePath))
                    {
                        System.IO.Directory.CreateDirectory(SavePath);
                    }
                    SavePath = SavePath + FileName;

                    using (StreamWriter writer = new StreamWriter(SavePath, append: false, encoding: Encoding.UTF8))
                    {

                        foreach (var error in errorMessages)
                        {
                            writer.WriteLine($"Error message: " + error);
                        }

                    }
                    Response.Data = Globals.baseURL + "/Logs/" + FileName;

                }


                return (Response);
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

        public async Task<BaseResponseWithData<string>> InsertUserNotActiveExcel(IFormFile ExcelSheet)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            var errorList = new List<string>();

            try
            {
                if (ExcelSheet == null)
                    throw new Exception("File is not received...");

                if (ExcelSheet == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_environment.WebRootPath, "NotActiveReports");

                // Get Data set
                DataSet competitionDayUserDs = Extension.CreateDsOfExcelFile(ExcelSheet, dirPath);

                var allUsers = _unitOfWork.HrUsers.FindAllQueryable(x => true);
                var allEmails = new HashSet<string>(allUsers.Select(u => u.Email));
                var allPhoneNumbers = new HashSet<string>(allUsers.Select(u => u.Mobile));
                var allNames = new HashSet<string>(allUsers.Select(u => u.FirstName));
                var routeEmployeesDb = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => true);
                List<HrUser> usersToInsert = new List<HrUser>();
                List<string> errorMessages = new List<string>();

                if (competitionDayUserDs != null && competitionDayUserDs.Tables.Count > 0)
                {
                    DataTable competitionDayUsers = competitionDayUserDs.Tables[0];
                    List<HrUser> newUsers = new List<HrUser>();


                    Random random = new Random();

                    // Process each row in the Excel file
                    for (int i = 1; i < competitionDayUsers.Rows.Count; i++)
                    {
                        var row = competitionDayUsers.Rows[i];

                        var email = row[0].ToString().Trim();
                        var firstName = row[1].ToString().Trim();
                        var active = row[3].ToString().Trim();



                        // Validation
                        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(email)
                            || string.IsNullOrWhiteSpace(active))
                        {
                            errorMessages.Add($"This User {firstName}  Some Parameters is Empty");
                            continue;
                        }

                        var user = allUsers.Where(x => x.Email == email).FirstOrDefault();
                        if (user != null)
                        {
                            errorMessages.Add($"This User {firstName} Id Not Found");
                            continue;
                        }
                        else
                        {
                            user.Active = active.ToLower() == "true" ? true : false;

                            var routesEmployee = routeEmployeesDb.Where(x => x.HrUser.Email == email).ToList();
                            foreach (var item in routesEmployee)
                            {
                                item.Active = false;
                            }

                        }


                    }


                    _unitOfWork.Complete();
                }



                string LOGPath = null;
                if (errorMessages.Any())
                {
                    string logsPath = Path.Combine(_environment.WebRootPath, "Logs");


                    string FileName = $"UsersWithRoutesErrorLog.txt";
                    LOGPath = "Logs/";
                    string SavePath = Path.Combine(_environment.WebRootPath, LOGPath);
                    if (!System.IO.Directory.Exists(SavePath))
                    {
                        System.IO.Directory.CreateDirectory(SavePath);
                    }
                    SavePath = SavePath + FileName;

                    using (StreamWriter writer = new StreamWriter(SavePath, append: false, encoding: Encoding.UTF8))
                    {

                        foreach (var error in errorMessages)
                        {
                            writer.WriteLine($"Error message: " + error);
                        }

                    }
                    Response.Data = Globals.baseURL + "/Logs/" + FileName;

                }


                return (Response);
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

        public BaseResponseWithDataAndHeader<List<AccountDto>> Accounts(long RouteID, long SupplierID, int Month, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountDto>> Response = new BaseResponseWithDataAndHeader<List<AccountDto>>();
            Response.Data = new List<AccountDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var accountsDb = _unitOfWork.TransportationVehicleRouteAccounts.FindAllQueryable(x => x.DateOfMonth.Month == Month, new[] { "TransportationVehicleRoute", "Supplier" });
                var roundsDb = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => (x.CheckInDate.HasValue && ((DateTime)x.CheckInDate).Month == Month) ||
                                                                                         (x.CheckOutDate.HasValue && ((DateTime)x.CheckOutDate).AddHours(-16).Month == Month) ||
                                                                                         (x.OneWayDate.HasValue && ((DateTime)x.OneWayDate).AddHours(-16).Month == Month));
                var deductDb = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => ((DateTime)x.DateOfDeduction).Month == Month);
                var SupplierPaymentDb = _unitOfWork.TransportionLineSupplierPayment.FindAllQueryable(x => x.DatePayment.Month == Month);

                var DistributionSupplierPaymentDb = _unitOfWork.DistributionSupplierPayment.FindAllQueryable(x => x.MonthNum == Month);
                var TransportionLineSupplierPaymentDb = _unitOfWork.TransportionLineSupplierPayment.FindAllQueryable(x => true);

                if (SupplierID > 0)
                {
                    accountsDb = accountsDb.Where(x => x.SupplierId == SupplierID);
                }
                if (RouteID > 0)
                {
                    accountsDb = accountsDb.Where(x => x.TransportationVehicleRouteId == RouteID);
                }
                if (Month > 0)
                {
                    accountsDb = accountsDb.Where(x => x.DateOfMonth.Month == Month);
                }

                var accountsList = PagedList<TransportationVehicleRouteAccount>.Create(accountsDb, PageNo, NoOfItems);
                var accountsListItems = accountsList.Select(a =>
                {
                    // 1. Calculate the base values first
                    var advancePaymentIds = SupplierPaymentDb
                        .Where(x => x.SupplierId == a.SupplierId && x.TypeOfDebt == "Advance")
                        .Select(x => x.Id)
                        .ToList();

                    var totalDue = roundsDb.Where(x => x.SupplierId == a.SupplierId).Sum(x => x.PriceRound) ?? 0;
                    var totalDeduct = deductDb.Where(x => x.SupplierId == a.SupplierId).Sum(x => x.DeductPerRound) ?? 0;
                    var totalPaidNormal = SupplierPaymentDb.Where(x => x.SupplierId == a.SupplierId && x.TypeOfDebt == "Normal").Sum(x => x.Payment);

                    var totalPaidAdvance = DistributionSupplierPaymentDb
                        .Where(x => advancePaymentIds.Contains(x.SupplierPaymentId) && x.MonthNum == Month)
                        .Sum(x => x.Payment);

                    var CheckPaidAdvanceOtherMonth = DistributionSupplierPaymentDb
                        .Any(x => advancePaymentIds.Contains(x.SupplierPaymentId) && x.MonthNum > Month);

                    // 2. Map everything to the DTO
                    return new AccountDto
                    {
                        AccountId = a.Id,
                        SupplierId = a.SupplierId,
                        SupplierName = a.Supplier.Name,
                        RoutesNum = accountsDb.Count(x => x.SupplierId == a.SupplierId),
                        //  CountOfRounds = roundsDb.Count(x => x.SupplierId == a.SupplierId),

                        TotalDue = totalDue != null ? totalDue : 0,
                        TotalDeduct = totalDeduct != null ? totalDeduct : 0,
                        TotalPaidNormal = totalPaidNormal != null ? totalPaidNormal : 0,
                        TotalPaidadvance = totalPaidAdvance != null ? totalPaidAdvance : 0,

                        // 3. Now you can use the local variables for the final calculation!
                        TotalDueAfterPaid = totalDue - (totalDeduct + totalPaidNormal + totalPaidAdvance),
                        Note = CheckPaidAdvanceOtherMonth ? "There are advance payments for this supplier in other months, please check the details." : "Not advance payments for this supplier in other months"
                    };
                }).ToList();
                Response.Data = accountsListItems;
                Response.Result = true;
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

        public BaseResponseWithDataAndHeader<List<AccountDto>> AccountsAllMonths(int Year, long RouteID, long SupplierID, int Month, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountDto>> Response = new BaseResponseWithDataAndHeader<List<AccountDto>>();
            Response.Data = new List<AccountDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                // 1. استعلامات مفتوحة (بدون فلتر شهر ثابت في البداية)
                var accountsDb = _unitOfWork.TransportationVehicleRouteAccounts.FindAllQueryable(x => true, new[] { "TransportationVehicleRoute", "Supplier" });
                var roundsDb = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => true, new[] { "TransportationVehicleRoute" });
                var deductDb = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => true);
                var supplierPaymentDb = _unitOfWork.TransportionLineSupplierPayment.FindAllQueryable(x => true);
                var distributionDb = _unitOfWork.DistributionSupplierPayment.FindAllQueryable(x => true);

                // 2. تطبيق فلتر المورد (إذا تم اختياره)
                if (SupplierID > 0)
                {
                    accountsDb = accountsDb.Where(x => x.SupplierId == SupplierID);
                }

                // 3. تطبيق فلتر الشهر (إذا تم اختياره)
                // إذا كان Month = 0، سيتم جلب كل الشهور بناءً على فلتر المورد أعلاه
                if (Month > 0)
                {
                    accountsDb = accountsDb.Where(x => x.DateOfMonth.Month == Month);
                }

                // تطبيق Pagination
                var accountsList = PagedList<TransportationVehicleRouteAccount>.Create(accountsDb, PageNo, NoOfItems);

                // 4. بناء البيانات لكل سطر (شهر/مورد)
                var accountsListItems = accountsList.Select(a =>
                {
                    // تحديد الشهر الحالي للسطر (مهم في حالة عرض كل الشهور لمورد واحد)
                    int currentMonth = a.DateOfMonth.Month;

                    // حساب المستحقات (ذهاب + إياب + دورة كاملة كما في الرسم)
                    var currentRounds = roundsDb.Where(x => x.SupplierId == a.SupplierId &&
                                                       ((x.CheckInDate.HasValue && ((DateTime)x.CheckInDate).Month == currentMonth) ||
                                                        (x.OneWayDate.HasValue && ((DateTime)x.OneWayDate).AddHours(-16).Month == currentMonth) ||
                                                        (x.CheckOutDate.HasValue && ((DateTime)x.CheckOutDate).AddHours(-16).Month == currentMonth)));

                    var totalDue = currentRounds.Sum(x => x.PriceRound) ?? 0;
                    var totalDeduct = deductDb.Where(x => x.SupplierId == a.SupplierId && (x.DateOfDeduction).Month == currentMonth).Sum(x => x.DeductPerRound) ?? 0;

                    // المدفوع العادي (Normal) لشهر السطر
                    var totalPaidNormal = supplierPaymentDb.Where(x => x.SupplierId == a.SupplierId &&
                                                                       x.TypeOfDebt == "Normal" &&
                                                                       x.DatePayment.Month == currentMonth).Sum(x => x.Payment);

                    // المدفوع المقدم (Advance) من خلال جدول التوزيع لشهر السطر
                    // نأتي بكل الـ IDs الخاصة بالدفع المقدم لهذا المورد
                    var advancePaymentIds = supplierPaymentDb.Where(x => x.SupplierId == a.SupplierId && x.TypeOfDebt == "Advance").Select(x => x.Id).ToList();

                    // نجمع المبالغ الموزعة لهذا المورد في هذا الشهر تحديداً
                    var totalPaidAdvance = distributionDb.Where(x => advancePaymentIds.Contains(x.SupplierPaymentId) && x.MonthNum == currentMonth).Sum(x => x.Payment);

                    return new AccountDto
                    {
                        AccountId = a.Id,
                        SupplierId = a.SupplierId,
                        SupplierName = a.Supplier.Name,
                        MonthName = a.DateOfMonth.ToString("MMMM"),
                        MonthNum = a.DateOfMonth.Month,
                        RoutesNum = accountsDb.Count(x => x.SupplierId == a.SupplierId && x.DateOfMonth.Month == currentMonth),
                        // CountOfRounds = (currentRounds.Where(x=>x.TransportationVehicleRoute.OneWay == false).Count())/2,
                        CountOfHalfGoRounds = (currentRounds.Where(x => x.TransportationVehicleRoute.OneWay == true && x.TransportationVehicleRoute.FromoDate != null).Count()),
                        CountOfHalfReturnRounds = (currentRounds.Where(x => x.TransportationVehicleRoute.OneWay == true && x.TransportationVehicleRoute.ToDate != null).Count()),
                        TotalDue = totalDue,
                        TotalDeduct = totalDeduct,
                        TotalPaidNormal = totalPaidNormal,
                        TotalPaidadvance = totalPaidAdvance,
                        // الحسبة النهائية (المتبقي "باقي" كما في الرسم)
                        TotalDueAfterPaid = totalDue - (totalDeduct + totalPaidNormal + totalPaidAdvance)
                    };
                }).ToList();

                Response.Data = accountsListItems;
                Response.Result = true;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }

        public BaseResponseWithDataAndHeader<List<AccountDto>> AccountsAllMonths22(int Year, long SupplierID, long RouteID, int Month, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountDto>> Response = new BaseResponseWithDataAndHeader<List<AccountDto>>();
            Response.Data = new List<AccountDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                // 1. جلب الاستعلامات الأساسية
                var accountsQuery = _unitOfWork.TransportationVehicleRouteAccounts
                    .FindAllQueryable(x => x.DateOfMonth.Year == Year, new[] { "TransportationVehicleRoute", "Supplier" });

                // 2. تطبيق الفلاتر الاختيارية
                if (SupplierID > 0)
                {
                    accountsQuery = accountsQuery.Where(x => x.SupplierId == SupplierID);
                }

                if (Month > 0)
                {
                    accountsQuery = accountsQuery.Where(x => x.DateOfMonth.Month == Month);
                }

                // 3. التجميع (Grouping) لمنع التكرار
                var groupedAccounts = accountsQuery
                    .GroupBy(x => new { x.SupplierId, x.DateOfMonth.Month, x.DateOfMonth.Year })
                    .Select(g => new
                    {
                        SupplierId = g.Key.SupplierId,
                        Month = g.Key.Month,
                        Year = g.Key.Year,
                        SupplierName = g.FirstOrDefault().Supplier.Name,
                        // نأخذ تاريخاً ممثلاً للمجموعة
                        RepresentativeDate = g.FirstOrDefault().DateOfMonth,
                        // إجمالي عدد المسارات لهذا المورد في هذا الشهر
                        RoutesCount = g.Count()
                    });

                // 4. تطبيق الـ Pagination على المجموعات الفريدة
                var pagedGroups = PagedList<dynamic>.Create(groupedAccounts, PageNo, NoOfItems);

                // 5. جلب البيانات الخارجية (لتحسين الأداء، يفضل جلبها بناءً على الموردين والشهور الناتجة فقط)
                var roundsDb = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => true, new[] { "TransportationVehicleRoute" });
                var deductDb = _unitOfWork.TransportationVehicleRouteDeductions.FindAllQueryable(x => true);
                var supplierPaymentDb = _unitOfWork.TransportionLineSupplierPayment.FindAllQueryable(x => true);
                var distributionDb = _unitOfWork.DistributionSupplierPayment.FindAllQueryable(x => true);

                // 6. بناء قائمة الـ DTO
                var accountsListItems = pagedGroups.Select(g =>
                {
                    int currentMonth = g.Month;
                    long currentSupplierId = g.SupplierId;

                    // حساب المستحقات للشهر والمورد الحالي
                    var currentRounds = roundsDb.Where(x => x.SupplierId == currentSupplierId &&
                                                       ((x.CheckInDate.HasValue && x.CheckInDate.Value.Month == currentMonth && x.CheckInDate.Value.Year == Year) ||
                                                        (x.OneWayDate.HasValue && x.OneWayDate.Value.AddHours(-16).Month == currentMonth && x.OneWayDate.Value.AddHours(-16).Year == Year) ||
                                                        (x.CheckOutDate.HasValue && x.CheckOutDate.Value.AddHours(-16).Month == currentMonth && x.CheckOutDate.Value.AddHours(-16).Year == Year)));

                    var AllCompleteRounds = currentRounds.Where(x => x.TransportationVehicleRoute.OneWay == false);
                    var CountOfHalfGoRounds = currentRounds.Where(x => x.TransportationVehicleRoute.OneWay == true && x.TransportationVehicleRoute.FromoDate != null);
                    var CountOfHalfReturnRounds = currentRounds.Where(x => x.TransportationVehicleRoute.OneWay == true && x.TransportationVehicleRoute.ToDate != null);
                    var advancePaymentIds = supplierPaymentDb.Where(x => x.SupplierId == currentSupplierId && x.TypeOfDebt == "Advance").Select(x => x.Id).ToList();
                    var CheckPaidAdvanceOtherMonth = distributionDb.Any(x => advancePaymentIds.Contains(x.SupplierPaymentId) && x.MonthNum > currentMonth);

                    decimal totalCompleteRoundsDue = 0;
                    decimal totalHalfGoRoundsDue = 0;
                    decimal totalHalfReturnRoundsDue = 0;
                    if (AllCompleteRounds.Any())
                    {
                        foreach (var round in AllCompleteRounds)
                        {
                            totalCompleteRoundsDue = totalCompleteRoundsDue + CalculatePricePerRouteFromExceptionPrice((long)round.TransportationVehicleRouteId, (DateTime)(round.CheckInDate ?? round.CheckOutDate));
                        }
                    }
                    if (CountOfHalfGoRounds.Any())
                    {
                        foreach (var round in CountOfHalfGoRounds)
                        {
                            totalHalfGoRoundsDue = totalHalfGoRoundsDue + CalculatePricePerRouteFromExceptionPrice((long)round.TransportationVehicleRouteId, (DateTime)(round.OneWayDate));
                        }
                    }
                    if (CountOfHalfReturnRounds.Any())
                    {

                        foreach (var round in CountOfHalfReturnRounds)
                        {
                            totalHalfReturnRoundsDue = totalHalfReturnRoundsDue + CalculatePricePerRouteFromExceptionPrice((long)round.TransportationVehicleRouteId, (DateTime)(round.OneWayDate));
                        }
                    }




                    var totalDeduct = deductDb.Where(x => x.SupplierId == currentSupplierId &&
                                                        x.DateOfDeduction.Month == currentMonth &&
                                                        x.DateOfDeduction.Year == Year);

                    var totalPaidNormal = supplierPaymentDb.Where(x => x.SupplierId == currentSupplierId &&
                                                                       x.TypeOfDebt == "Normal" &&
                                                                       x.DatePayment.Month == currentMonth &&
                                                                       x.DatePayment.Year == Year).Sum(x => x.Payment);

                    var totalPaidAdvance = distributionDb.Where(x => advancePaymentIds.Contains(x.SupplierPaymentId) && x.MonthNum == currentMonth && x.YearNum == Year).Sum(x => x.Payment);
                    var totalDeductAmount = totalDeduct.Sum(x => x.DeductPerRound) ?? 0;
                    return new AccountDto
                    {
                        SupplierId = currentSupplierId,
                        SupplierName = g.SupplierName,
                        MonthName = g.RepresentativeDate.ToString("MMMM"),
                        MonthNum = currentMonth,
                        RoutesNum = g.RoutesCount,
                        CountOfcompleteRounds = AllCompleteRounds.Count() / 2,
                        CountOfHalfGoRounds = CountOfHalfGoRounds.Count(),
                        CountOfHalfReturnRounds = CountOfHalfReturnRounds.Count(),
                        TotalDue = totalCompleteRoundsDue + totalHalfGoRoundsDue + totalHalfReturnRoundsDue,
                        TotalDeduct = totalDeductAmount,
                        TotalNormalDeduct = totalDeduct.Where(x => x.TypeOfDedct == "Normal").Sum(x => x.DeductPerRound) ?? 0,
                        TotalTaxesDeduct = totalDeduct.Where(x => x.TypeOfDedct == "Taxes").Sum(x => x.DeductPerRound) ?? 0,
                        TotalPaidNormal = totalPaidNormal,
                        TotalPaidadvance = totalPaidAdvance,
                        TotalDueAfterPaid = (totalCompleteRoundsDue + totalHalfGoRoundsDue + totalHalfReturnRoundsDue) - (totalDeductAmount + totalPaidNormal + totalPaidAdvance),
                        TotalDuecompleteRound = totalCompleteRoundsDue,
                        TotalDueHalfGoRound = totalHalfGoRoundsDue,
                        TotalDueHalfReturnRound = totalHalfReturnRoundsDue,
                        Note = CheckPaidAdvanceOtherMonth ? "هناك دفعات مقدمة لهذا المورد في أشهر أخرى، يرجى التحقق من التفاصيل" : null

                    };
                }).ToList();

                Response.Data = accountsListItems;
                Response.Result = true;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }

        public BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>> AccountsAllRoundsForSupplier(int Year, long SupplierId, long RouteID, int Month, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>> Response = new BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>>();
            Response.Data = new List<AccountsAllRoundsDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var roundsDb = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => x.SupplierId == SupplierId &&
                                                                                        ((x.CheckInDate.HasValue && ((DateTime)x.CheckInDate).Month == Month) ||
                                                                                         (x.CheckOutDate.HasValue && ((DateTime)x.CheckOutDate).AddHours(-16).Month == Month) ||
                                                                                         (x.OneWayDate.HasValue && ((DateTime)x.OneWayDate).AddHours(-16).Month == Month)), new[] { "TransportationVehicleRoute" });

                if (RouteID > 0)
                {
                    roundsDb = roundsDb.Where(x => x.TransportationVehicleRouteId == (long)RouteID);
                }




                var exceptionPriceDb = _unitOfWork.TransportionLineExceptionPrice.FindAllQueryable(x => x.FromDate.Month == Month);


                var accountsList = PagedList<TransportationRoundForRoute>.Create(roundsDb, PageNo, NoOfItems);
                decimal totalPrice = 0;
                var accountsListItems = accountsList.Select(a =>
                {

                    var test = 1;
                    return new AccountsAllRoundsDto
                    {
                        NameOfRoute = a.TransportationVehicleRoute.NameOfRoute,
                        Serial = a.TransportationVehicleRoute.Serial,
                        DateOfRound = a.CheckInDate?.ToString("yyyy-MM-dd")
                                       ?? a.CheckOutDate?.ToString("yyyy-MM-dd")
                                       ?? a.OneWayDate?.ToString("yyyy-MM-dd"),
                        DateOfCheckIn = a.CheckInDate,
                        DateOfCheckOut = a.CheckOutDate,
                        DateOfOneWay = a.OneWayDate,
                        RoundsNum = ((bool)a.TransportationVehicleRoute.OneWay && a.OneWayDate != null) ? 1 : (a.CheckInDate != null && a.CheckOutDate != null) ? 1 :
                                                                                   (a.CheckInDate == null && a.CheckOutDate != null) ? 0.5 :
                                                                                    (a.CheckInDate != null && a.CheckOutDate == null) ? 0.5 : 0,


                        TotalPriceOfDay = CalculatePricePerRouteFromExceptionPrice((long)a.TransportationVehicleRouteId, (DateTime)(a.CheckInDate ?? a.CheckOutDate ?? a.OneWayDate))
                    };
                }).ToList();
                Response.Data = accountsListItems;
                Response.Result = true;
                return Response;

            }
            //catch (Exception ex)
            //{
            //    Response.Result = false;
            //    Error error = new Error();
            //    error.ErrorCode = "Err10";
            //    //error.ErrorMSG = ex.InnerException.Message;
            //    error.ErrorMSG = ex.InnerException?.Message ?? ex.Message;

            //    Response.Errors.Add(error);
            //    return Response;
            //}

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public BaseResponseWithDataAndHeader<List<SupplierPaymentDto>> getAllSupplierPayment(long SupplierId, DateTime? FromDate, DateTime? ToDate, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<SupplierPaymentDto>> Response = new BaseResponseWithDataAndHeader<List<SupplierPaymentDto>>();
            Response.Data = new List<SupplierPaymentDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var SupplierPaymentDB = _unitOfWork.TransportionLineSupplierPayment.FindAllQueryable(x => x.Id > 0, new[] { "Supplier" });
                var DistributionSupplierPaymentDb = _unitOfWork.DistributionSupplierPayment.FindAllQueryable(x => x.Id > 0);

                if (SupplierId > 0)
                {
                    SupplierPaymentDB = SupplierPaymentDB.Where(x => x.SupplierId == SupplierId);

                }


                if (FromDate != null)
                {
                    SupplierPaymentDB = SupplierPaymentDB.Where(x => x.DatePayment.Date >= ((DateTime)FromDate).Date);


                }
                if (ToDate != null)
                {
                    SupplierPaymentDB = SupplierPaymentDB.Where(x => x.DatePayment.Date <= ((DateTime)ToDate).Date);

                }
                SupplierPaymentDB = SupplierPaymentDB.OrderBy(x => x.DatePayment);
                var transportPagedList = PagedList<TransportionLineSupplierPayment>.Create(SupplierPaymentDB, PageNo, NoOfItems);
                Response.Data = transportPagedList.Select(a => new SupplierPaymentDto
                {

                    SupplierName = a.Supplier.Name,
                    DatePayment = a.DatePayment,
                    Payment = a.Payment,
                    StartDate = a.StartDate,
                    NumberOfMonths = a.NumberOfMonths,
                    TypeOfDebt = a.TypeOfDebt,
                    DistributionSupplierPayments = DistributionSupplierPaymentDb
                        .Where(d => d.SupplierPaymentId == a.Id)
                        .Select(d => new DistributionSupplierPaymentDo
                        {
                            Payment = d.Payment,
                            MonthNum = d.MonthNum,
                            YearNum = d.YearNum
                        }).ToList()


                }).ToList();
                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = transportPagedList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = transportPagedList.TotalCount
                };
                Response.Result = true;
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

        public BaseResponse AddSupplierPayment(TransportionLineSupplierPayment dto)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                if (dto.StartDate != null && dto.NumberOfMonths > 0 && dto.TypeOfDebt == "Advance")
                {
                    var supplierPayment = new TransportionLineSupplierPayment
                    {
                        SupplierId = dto.SupplierId,
                        Payment = dto.Payment,
                        DatePayment = dto.DatePayment,
                        StartDate = dto.StartDate,
                        NumberOfMonths = dto.NumberOfMonths,
                        TypeOfDebt = dto.TypeOfDebt,
                    };

                    _unitOfWork.TransportionLineSupplierPayment.Add(supplierPayment);


                    for (int month = 1; month <= dto.NumberOfMonths; month++)
                    {
                        var targetDate = ((DateTime)dto.StartDate).AddMonths(month - 1);

                        _unitOfWork.DistributionSupplierPayment.Add(new DistributionSupplierPayment
                        {
                            SupplierId = dto.SupplierId,

                            SupplierPayment = supplierPayment,

                            MonthNum = targetDate.Month,

                            YearNum = targetDate.Year,

                            Payment = dto.Payment / dto.NumberOfMonths
                        });
                    }
                }
                else if (dto.StartDate == null && dto.NumberOfMonths == null && dto.TypeOfDebt == "Normal")
                {
                    _unitOfWork.TransportionLineSupplierPayment.Add(new TransportionLineSupplierPayment
                    {
                        SupplierId = dto.SupplierId,
                        Payment = dto.Payment,
                        DatePayment = dto.DatePayment,
                        StartDate = dto.StartDate,
                        NumberOfMonths = dto.NumberOfMonths,
                        TypeOfDebt = dto.TypeOfDebt,
                    });
                }
                else
                {
                    Response.Result = false;
                    Error error = new Error();
                    error.ErrorCode = "Err10";
                    error.ErrorMSG = "تاكد من تاريخ البدء وعدد الشهور لدفعات المقدمة والغائهم في الدفعات العادية ";
                    Response.Errors.Add(error);
                    return Response;
                }

                _unitOfWork.Complete();
                Response.Result = true;
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
        public BaseResponse UpdateRoutePrice()
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors = new List<Error>();
            Response.Result = false;

            try
            {
                var routesDb = _unitOfWork.TransportationVehicleRoutes.FindAllQueryable(x => true);
                var exceptionPrices = _unitOfWork.TransportionLineExceptionPrice.FindAllQueryable(x => true);
                foreach (var item in routesDb)
                {
                    var exceptionPrice = exceptionPrices.Where(x => x.RouteId == item.Id && x.FromDate <= DateTime.Now).OrderBy(w => w.FromDate).FirstOrDefault();
                    if (exceptionPrice != null)
                    {
                        item.LineCost = exceptionPrice.Price;
                        _unitOfWork.TransportationVehicleRoutes.Update(item);
                    }

                }
                _unitOfWork.Complete();
                Response.Result = true;
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

        public async Task<BaseResponseWithData<string>> RoutesWithUsersExcell(long? RouteId, long? SupplierId)
        {
            BaseResponseWithData<string> Response = new BaseResponseWithData<string>();
            Response.Result = true;
            Response.Errors = new List<Error>();
            try
            {
                // 1. جلب بيانات العميل (Logo, Phone, etc.)
                var client = _unitOfWork.Clients.FindAll(x => x.OwnerCoProfile == true).FirstOrDefault();
                if (client == null)
                {
                    Response.Result = false;
                    Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = "client not found" });
                    return Response;
                }

                var clientPhone = _unitOfWork.ClientPhones.FindAll(x => x.ClientId == client.Id).Select(y => y.Phone).FirstOrDefault();
                var clientAddress = _unitOfWork.ClientAddresses.FindAll(x => x.ClientId == client.Id).FirstOrDefault();
                var createdUser = _unitOfWork.Users.FindAll(x => x.Id == client.CreatedBy).FirstOrDefault();

                // 2. جلب الخطوط المطلوبة
                var transportionLineRoutesDb = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id > 0);
                if (RouteId > 0) transportionLineRoutesDb = transportionLineRoutesDb.Where(x => x.Id == RouteId);
                if (SupplierId > 0)
                {
                    var lineIds = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.SupplierId == SupplierId).Select(s => s.Id).ToList();
                    transportionLineRoutesDb = transportionLineRoutesDb.Where(x => lineIds.Contains(x.Id));
                }

                ExcelPackage excel = new ExcelPackage();
                int sheetCounter = 1;

                foreach (var item in transportionLineRoutesDb)
                {
                    // استدعاء الميثود الجديدة التي تعيد قائمة الموظفين وبداخلهم أيام الحضور
                    var hrUsers = _unitOfWork.TransportationVehicleRouteEmployees.FindAll(x => x.TransportationVehicleRouteId == item.Id && x.Active == true, new[] { "HrUser" }).Select(y => y.HrUser).ToList();
                    var sheet = excel.Workbook.Worksheets.Add($"{item.NameOfRoute}");
                    sheet.View.RightToLeft = true; // لضبط الاتجاه للعربية

                    // --- التنسيق العام والعناوين ---
                    sheet.Cells[5, 1].Value = $"تقرير الموظفين المقيدين في {item.NameOfRoute}";
                    sheet.Cells[5, 1, 5, 6].Merge = true;
                    sheet.Cells[5, 1].Style.Font.Bold = true;
                    sheet.Cells[5, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.Cells[6, 4].Value = $"Date :";
                    sheet.Cells[6, 3].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    sheet.Cells[7, 4].Value = $"Current Capacity :";
                    sheet.Cells[7, 3].Value = hrUsers.Count();
                    //sheet.Cells[6, 3].Value = "الفترة من:";
                    //sheet.Cells[6, 2].Value = FromDate?.ToString("yyyy-MM-dd") ?? DateSerach?.ToString("yyyy-MM-dd");
                    //sheet.Cells[6, 1].Value = $"إلى: {ToDate?.ToString("yyyy-MM-dd")}";

                    // إضافة بيانات العميل (الهيدر)
                    sheet.Cells[1, 3].Value = client.Name;
                    sheet.Cells[2, 3].Value = $"الهاتف: {clientPhone}";
                    sheet.Cells[1, 3, 4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // --- رؤوس الأعمدة (Row 8) ---
                    string[] headers = { "اسم الموظف", "الرقم التعريفي" };
                    for (int col = 0; col < headers.Length; col++)
                    {
                        var cell = sheet.Cells[8, col + 1];
                        cell.Value = headers[col];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 174, 81));
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // --- تعبئة البيانات (البحث في القائمة المتداخلة) ---
                    int y = 9;
                    if (hrUsers != null && hrUsers.Count > 0)
                    {
                        foreach (var user in hrUsers)
                        {

                            sheet.Cells[y, 1].Value = $"{user.FirstName} ";
                            sheet.Cells[y, 2].Value = user.Email;


                            // تنسيق السطر
                            sheet.Row(y).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[y, 1, y, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            y++;

                        }
                    }

                    // ضبط عرض الأعمدة
                    sheet.Column(1).Width = 30; // الاسم
                    sheet.Column(2).Width = 15; // التاريخ
                    sheet.Cells.Style.Font.Size = 10;

                    // إضافة اللوجو (فقط في أول شيت)
                    if (sheetCounter == 1 && !string.IsNullOrEmpty(client.LogoUrl))
                    {
                        try
                        {
                            string imagePath = Path.Combine(_environment.WebRootPath, client.LogoUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));
                            if (File.Exists(imagePath))
                            {
                                var picture = sheet.Drawings.AddPicture("Logo", new FileInfo(imagePath));
                                picture.SetPosition(0, 0, 0, 0);
                                picture.SetSize(120, 60);
                            }
                        }
                        catch { /* تجاوز خطأ الصورة */ }
                    }

                    sheetCounter++;
                }

                // 3. حفظ الملف
                var path = "TransportLineExcel";
                var savedPath = Path.Combine(_environment.WebRootPath, path);
                if (!Directory.Exists(savedPath)) Directory.CreateDirectory(savedPath);

                var fileName = $"AttendanceReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var fullPath = Path.Combine(savedPath, fileName);
                excel.SaveAs(new FileInfo(fullPath));

                Response.Data = Globals.baseURL + "/" + path + "/" + fileName;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.Errors.Add(new Error { ErrorCode = "Err10", ErrorMSG = ex.Message });
                return Response;
            }
        }

        public async Task<BaseResponseWithData<string>> getAllDeductionExcel(int Id, DateTime? FromDate, DateTime? ToDate, long SupplierId, long RouteId, string? Name, int PageNo, int NoOfItems)
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();


            var filePath = System.IO.Path.Combine(this._environment.WebRootPath, "Attachments\\PharmaTemplate\\getAllDeductionExcel.xlsx");




            var allDeductions = getAllDeduction(Id, FromDate, ToDate, SupplierId, RouteId, Name, PageNo, NoOfItems);


            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                    //fill the list of InventoryItemCategory in Excel file to let the user choose from them

                    int row = 7;


                    foreach (var item in allDeductions.Data)
                    {
                        sheet.Cells[row, 1].Value = item.Serial;
                        sheet.Cells[row, 2].Value = item.TransportationVehicleRouteName;
                        sheet.Cells[row, 3].Value = item.SupplierName;
                        sheet.Cells[row, 4].Value = item.SupplierContentPersonIdName;
                        sheet.Cells[row, 5].Value = item.DeductPerRound;
                        sheet.Cells[row, 6].Value = item.DateOfDeduction;
                        sheet.Cells[row, 7].Value = item.typeOfDeduction;
                        sheet.Cells[row, 8].Value = item.Cause;
                        //sheet.Cells[row, 9].Value = item.TransportationVehicle?.VehicleType?.Type ?? string.Empty;
                        //sheet.Cells[row, 10].Value = item.TransportationVehicle.Capacity;
                        //sheet.Cells[row, 11].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id).Count();
                        //sheet.Cells[row, 12].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == christianID).Count();
                        //sheet.Cells[row, 13].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == MuslimID).Count();
                        row++;
                    }



                    var dirPath = System.IO.Path.Combine(this._environment.WebRootPath, $"Attachments\\PharmaTemplate\\resultTampletes\\");
                    Directory.CreateDirectory(dirPath);

                    var finalFilePath = dirPath + $"getAllDeductionExcel.xlsx";
                    package.SaveAs(finalFilePath);
                    package.Dispose();
                    var fixedPath = $"Attachments\\PharmaTemplate\\resultTampletes\\getAllDeductionExcel.xlsx";

                    response.Data = Globals.baseURL + "\\" + fixedPath;
                    return response;
                }
                else
                {
                    response.Result = false;
                    response.Errors = new List<Error>();
                    Error error = new Error();
                    error.ErrorCode = "Err11";
                    error.ErrorMSG = "The Templete File is not exsists";
                    response.Errors.Add(error);
                    return response;
                }
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
        public async Task<BaseResponseWithData<string>> AccountsAllMonthsForSupplierExcel(int Year, long SupplierID, long RouteID, int Month, int PageNo, int NoOfItems)
        {
            var response = new BaseResponseWithData<string>();
            response.Result = true;
            response.Errors = new List<Error>();


            var filePath = System.IO.Path.Combine(this._environment.WebRootPath, "Attachments\\PharmaTemplate\\AccountsAllMonthsForSupplierExcel.xlsx");




            var allDeductions = AccountsAllMonths22(Year, SupplierID, RouteID, Month, PageNo, NoOfItems);


            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                    //fill the list of InventoryItemCategory in Excel file to let the user choose from them

                    int row = 7;


                    foreach (var item in allDeductions.Data)
                    {
                        sheet.Cells[row, 1].Value = item.MonthName;
                        sheet.Cells[row, 2].Value = item.SupplierName;
                        sheet.Cells[row, 3].Value = item.RoutesNum;
                        sheet.Cells[row, 4].Value = item.CountOfcompleteRounds;
                        sheet.Cells[row, 5].Value = item.CountOfHalfGoRounds;
                        sheet.Cells[row, 6].Value = item.CountOfHalfReturnRounds;
                        sheet.Cells[row, 7].Value = item.TotalDue;
                        sheet.Cells[row, 8].Value = item.TotalNormalDeduct;
                        sheet.Cells[row, 8].Value = item.TotalTaxesDeduct;
                        sheet.Cells[row, 8].Value = item.TotalPaidNormal;
                        sheet.Cells[row, 8].Value = item.TotalPaidadvance;
                        sheet.Cells[row, 8].Value = item.TotalDueAfterPaid;
                        sheet.Cells[row, 8].Value = item.TotalDuecompleteRound;
                        sheet.Cells[row, 8].Value = item.TotalDueHalfGoRound;
                        sheet.Cells[row, 8].Value = item.TotalDueHalfReturnRound;
                        sheet.Cells[row, 8].Value = item.Note;
                        //sheet.Cells[row, 9].Value = item.TransportationVehicle?.VehicleType?.Type ?? string.Empty;
                        //sheet.Cells[row, 10].Value = item.TransportationVehicle.Capacity;
                        //sheet.Cells[row, 11].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id).Count();
                        //sheet.Cells[row, 12].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == christianID).Count();
                        //sheet.Cells[row, 13].Value = RoutesEmployesDB.Where(x => x.TransportationVehicleRouteId == item.Id && x.HrUser != null && x.HrUser.MaritalStatusId == MuslimID).Count();
                        row++;
                    }



                    var dirPath = System.IO.Path.Combine(this._environment.WebRootPath, $"Attachments\\PharmaTemplate\\resultTampletes\\");
                    Directory.CreateDirectory(dirPath);

                    var finalFilePath = dirPath + $"AccountsAllMonthsForSupplierExcel.xlsx";
                    package.SaveAs(finalFilePath);
                    package.Dispose();
                    var fixedPath = $"Attachments\\PharmaTemplate\\resultTampletes\\AccountsAllMonthsForSupplierExcel.xlsx";

                    response.Data = Globals.baseURL + "\\" + fixedPath;
                    return response;
                }
                else
                {
                    response.Result = false;
                    response.Errors = new List<Error>();
                    Error error = new Error();
                    error.ErrorCode = "Err11";
                    error.ErrorMSG = "The Templete File is not exsists";
                    response.Errors.Add(error);
                    return response;
                }
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




        public async Task<BaseResponseWithDataAndHeader<List<BusAttendanceLineVm>>>
BusAttendance(
  DateTime? dateSearch,
  DateTime? FromDate,
  DateTime? ToDate,
  long SupplierId,
  long supplierContactPersonId,
  long RouteId,
  string? Name,
  string? serialBus,
  int PageNo,
  int NoOfItems)
        {
            var response =
                new BaseResponseWithDataAndHeader<List<BusAttendanceLineVm>>
                {
                    Result = true,
                    Errors = new List<Error>()
                };

            try
            {
                // ============================================================
                // 1. تحديد الفترة
                // ============================================================

                DateTime startDate;
                DateTime endDate;

                if (FromDate.HasValue && ToDate.HasValue)
                {
                    startDate = FromDate.Value.Date;
                    endDate = ToDate.Value.Date;
                }
                else if (dateSearch.HasValue)
                {
                    startDate = new DateTime(
                        dateSearch.Value.Year,
                        dateSearch.Value.Month,
                        1);

                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    // إذا لم يتم إرسال تاريخ
                    // نستخدم تاريخ اليوم

                    startDate = DateTime.Today;
                    endDate = DateTime.Today;
                }

                // ============================================================
                // 2. جلب Transportation Vehicle Routes
                // ============================================================

                var vehicleRouteQuery =
                    _unitOfWork.TransportationVehicleRoutes
                        .FindAllQueryable(
                            x => x.Active,
                            new[] { "TransportationLine" });

                // ============================================================
                // 3. Supplier
                // ============================================================

                if (SupplierId > 0)
                {
                    vehicleRouteQuery =
                        vehicleRouteQuery.Where(
                            x => x.SupplierId == SupplierId);
                }

                // ============================================================
                // 4. Supplier Contact Person
                // ============================================================

                if (supplierContactPersonId > 0)
                {
                    vehicleRouteQuery =
                        vehicleRouteQuery.Where(
                            x =>
                                x.SupplierContactPersonId ==
                                supplierContactPersonId);
                }

                // ============================================================
                // 5. Route
                // ============================================================

                if (RouteId > 0)
                {
                    vehicleRouteQuery =
                        vehicleRouteQuery.Where(
                            x => x.Id == RouteId);
                }

                // ============================================================
                // 6. Serial Bus
                // ============================================================

                if (!string.IsNullOrWhiteSpace(serialBus))
                {
                    serialBus =
                        HttpUtility.UrlDecode(serialBus);

                    var searchSerial =
                        serialBus.Trim();

                    vehicleRouteQuery =
                        vehicleRouteQuery.Where(
                            x =>
                                x.Serial != null &&
                                x.Serial.Trim() == searchSerial);
                }

                // ============================================================
                // 7. Name
                // ============================================================

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    Name =
                        HttpUtility.UrlDecode(Name);

                    var searchName =
                        Name.Trim();

                    vehicleRouteQuery =
                        vehicleRouteQuery.Where(
                            x =>
                                (
                                    x.NameOfRoute != null &&
                                    x.NameOfRoute
                                        .Trim()
                                        .Contains(searchName)
                                )
                                ||
                                (
                                    x.Serial != null &&
                                    x.Serial
                                        .Trim()
                                        .Contains(searchName)
                                ));
                }

                // ============================================================
                // 8. جلب الأتوبيسات
                // ============================================================

                var vehicleRoutes =
                    await vehicleRouteQuery.ToListAsync();

                if (vehicleRoutes == null ||
                    vehicleRoutes.Count == 0)
                {
                    response.Data =
                        new List<BusAttendanceLineVm>();

                    return response;
                }

                // ============================================================
                // 9. جلب Serials
                // ============================================================

                var busSerials =
                    vehicleRoutes
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.Serial))
                        .Select(x =>
                            x.Serial!.Trim())
                        .Distinct()
                        .ToList();

                // ============================================================
                // 10. جلب Attendance للفترة فقط
                // ============================================================

                var attendanceQuery =
                    _unitOfWork.TransprotationUserAttedances
                        .FindAllQueryable(
                            x =>
                                x.Id > 0 &&
                                x.Serial != null &&
                                busSerials.Contains(
                                    x.Serial.Trim()) &&
                                x.CreationDate >= startDate &&
                                x.CreationDate <
                                    endDate.AddDays(1),
                            new[] { "CheckInRoute" });

                var attendanceList =
                    await attendanceQuery.Where(x => x.Type == "BUS")
                        .OrderBy(x => x.CreationDate)
                        .ThenBy(x => x.Serial)
                        .ToListAsync();

                var attendanceUsers =
                    _unitOfWork.TransprotationUserAttedances
                        .FindAllQueryable(x => x.Type == "Person" &&
                                x.CreationDate >= startDate &&
                                x.CreationDate <
                                    endDate.AddDays(1))
                        .OrderBy(x => x.CreationDate)
                        .ThenBy(x => x.Serial);

                // ============================================================
                // 11. عمل Dictionary للـ Attendance
                //
                // المفتاح:
                // Serial + Date
                //
                // حتى نستطيع معرفة هل الأتوبيس حضر في هذا اليوم أم لا
                // ============================================================

                var attendanceDictionary =
                    attendanceList
                        .GroupBy(x => new
                        {
                            Serial = x.Serial!.Trim(),
                            Date = x.CreationDate.Date
                        })
                        .ToDictionary(
                            g => (
                                g.Key.Serial,
                                g.Key.Date),
                            g => g.First());

                // ============================================================
                // 12. إنشاء قائمة الأيام
                // ============================================================

                var days = new List<DateTime>();

                for (
                    var date = startDate.Date;
                    date <= endDate.Date;
                    date = date.AddDays(1))
                {
                    days.Add(date);
                }

                // ============================================================
                // 13. إنشاء البيانات
                //
                // كل Bus × كل Day
                // ============================================================

                var allData =
                    new List<BusAttendanceApiVm>();

                foreach (var vehicleRoute in vehicleRoutes)
                {
                    if (string.IsNullOrWhiteSpace(
                        vehicleRoute.Serial))
                    {
                        continue;
                    }

                    var serial =
                        vehicleRoute.Serial.Trim();

                    var lineName =
                        vehicleRoute
                            .TransportationLine?
                            .LineName
                        ?? "غير محدد";

                    // ========================================================
                    // كل يوم
                    // ========================================================

                    foreach (var day in days)
                    {
                        attendanceDictionary.TryGetValue(
                            (serial, day.Date),
                            out var attendance);

                        // ====================================================
                        // إذا وجد Attendance
                        // ====================================================

                        if (attendance != null)
                        {
                            allData.Add(
                                new BusAttendanceApiVm
                                {
                                    LineName =
                                        lineName,

                                    RouteName =
                                        attendance
                                            .CheckInRoute?
                                            .NameOfRoute
                                        ??
                                        vehicleRoute
                                            .NameOfRoute
                                        ??
                                        "",

                                    Serial =
                                        serial,

                                    Date =
                                        day,

                                    CheckIn =
                                        attendance.CheckIn != null,

                                    CheckOut =
                                        attendance.CheckOut != null,

                                    OneWay =
                                        attendance.OneWayDate != null,

                                    CheckInUsersCount = attendanceUsers
                                          .Where(x =>
                                              x.CheckInRouteId == vehicleRoute.Id &&
                                              x.CreationDate.Date == day.Date)
                                          .Select(x => x.Serial)
                                          .Distinct()
                                          .Count(),
                                    CheckOutUsersCount = attendanceUsers
                                          .Where(x =>
                                              x.CheckOutRouteId == vehicleRoute.Id &&
                                              x.CreationDate.Date == day.Date)
                                          .Select(x => x.Serial)
                                          .Distinct()
                                          .Count(),

                                    // OneWayUsersCount = attendanceUsers.Where(x => x.CheckInRouteId == attendance.CheckInRouteId && x.CreationDate.Date == day.Date).Count(),

                                });
                        }
                        else
                        {
                            // =================================================
                            // لا يوجد Attendance
                            //
                            // نظهر اليوم ولكن القيم = false / 0
                            // =================================================

                            allData.Add(
                                new BusAttendanceApiVm
                                {
                                    LineName =
                                        lineName,

                                    RouteName =
                                        vehicleRoute
                                            .NameOfRoute
                                        ?? "",

                                    Serial =
                                        serial,

                                    Date =
                                        day,

                                    CheckIn = false,

                                    CheckOut = false,

                                    OneWay = false,

                                    CheckInUsersCount = 0,

                                    CheckOutUsersCount = 0,

                                    OneWayUsersCount = 0
                                });
                        }
                    }
                }

                // ============================================================
                // 14. Group By TransportationLine
                // ============================================================

                var grouped =
                    vehicleRoutes
                        .GroupBy(x =>
                            new
                            {

                                RouteName =
                string.IsNullOrWhiteSpace(x.NameOfRoute)
                    ? "غير محدد"
                    : x.NameOfRoute.Trim()
                            })
                        .Select(g =>
                        {
                            var lineData =
                                allData
                                    .Where(x =>
                                        x.RouteName ==
                                        g.Key.RouteName)
                                    .OrderBy(x => x.Date)
                                    .ThenBy(x => x.Serial)
                                    .ToList();

                            return new BusAttendanceLineVm
                            {


                                RouteName =
                                    g.Key.RouteName,



                                Attendance =
                                    lineData
                            };
                        })
                        .OrderBy(x => x.RouteName)
                        .ToList();

                // ============================================================
                // 15. Pagination
                // ============================================================

                var pagedData =
      PagedList<BusAttendanceLineVm>
          .Create(grouped.AsQueryable(), PageNo, NoOfItems);

                response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = pagedData.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = pagedData.TotalCount
                };

                response.Data = pagedData.ToList();

                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;

                response.Errors.Add(
                    new Error
                    {
                        ErrorCode = "Err10",

                        ErrorMSG =
                            ex.InnerException?.Message
                            ?? ex.Message
                    });

                return response;
            }
        }

        public async Task<BaseResponseWithData<string>> BusAttendanceExcel(
     DateTime? dateSearch,
     DateTime? FromDate,
     DateTime? ToDate,
     long SupplierId,
     long supplierContactPersonId,
     long RouteId,
     string? Name,
     string? serialBus,
     int PageNo,
     int NoOfItems)
        {
            var response = new BaseResponseWithData<string>
            {
                Result = true,
                Errors = new List<Error>()
            };

            try
            {
                // ============================================================
                // 1. استدعاء نفس Method الخاصة بالـ API
                //
                // ForExcel = true
                // حتى نحصل على جميع الخطوط وجميع الأيام
                // ============================================================

                var attendanceResponse =
                    await BusAttendance(
                        dateSearch,
                        FromDate,
                        ToDate,
                        SupplierId,
                        supplierContactPersonId,
                        RouteId,
                        Name,
                        serialBus,
                        1,
                        NoOfItems
                        );

                if (!attendanceResponse.Result)
                {
                    response.Result = false;
                    response.Errors = attendanceResponse.Errors;

                    return response;
                }

                var Routes =
                    attendanceResponse.Data
                    ?? new List<BusAttendanceLineVm>();

                // ============================================================
                // 2. إنشاء Excel جديد
                // ============================================================

                using var package = new ExcelPackage();

                // ============================================================
                // 3. لا توجد بيانات
                // ============================================================

                if (Routes.Count == 0)
                {
                    var emptySheet =
                        package.Workbook.Worksheets.Add(
                            "لا توجد بيانات");

                    emptySheet.View.RightToLeft = true;

                    emptySheet.Cells[1, 1].Value =
                        "لا توجد بيانات حسب الفلاتر المحددة.";

                    emptySheet.Cells[1, 1].Style.Font.Bold = true;
                }

                // ============================================================
                // 4. إنشاء Tab لكل TransportationLine
                // ============================================================

                foreach (var line in Routes)
                {
                    var RouteName =
                        string.IsNullOrWhiteSpace(line.RouteName)
                            ? "غير محدد"
                            : line.RouteName.Trim();

                    // ========================================================
                    // تنظيف اسم الـ Sheet
                    // ========================================================

                    var sheetName = RouteName
                        .Replace("/", "-")
                        .Replace("\\", "-")
                        .Replace("?", "")
                        .Replace("*", "")
                        .Replace("[", "(")
                        .Replace("]", ")")
                        .Replace(":", "-");

                    if (string.IsNullOrWhiteSpace(sheetName))
                    {
                        sheetName = "غير محدد";
                    }

                    // Excel maximum sheet name = 31
                    if (sheetName.Length > 31)
                    {
                        sheetName =
                            sheetName.Substring(0, 31);
                    }

                    // ========================================================
                    // منع تكرار أسماء Tabs
                    // ========================================================

                    var originalSheetName = sheetName;

                    int sheetIndex = 1;

                    while (
                        package.Workbook.Worksheets[sheetName] != null)
                    {
                        var suffix =
                            $"_{sheetIndex}";

                        var maxLength =
                            31 - suffix.Length;

                        var baseName =
                            originalSheetName.Length > maxLength
                                ? originalSheetName.Substring(
                                    0,
                                    maxLength)
                                : originalSheetName;

                        sheetName =
                            baseName + suffix;

                        sheetIndex++;
                    }

                    // ========================================================
                    // إنشاء Sheet
                    // ========================================================

                    var sheet =
                        package.Workbook.Worksheets.Add(
                            sheetName);

                    sheet.View.RightToLeft = true;

                    // ========================================================
                    // العنوان
                    // ========================================================

                    sheet.Cells[1, 1].Value =
                        "تقرير حضور الأتوبيسات";

                    sheet.Cells[1, 1, 1, 10].Merge = true;

                    sheet.Cells[1, 1].Style.Font.Bold = true;

                    sheet.Cells[1, 1].Style.Font.Size = 16;

                    sheet.Cells[1, 1]
                        .Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Center;

                    // ========================================================
                    // اسم الخط
                    // ========================================================

                    sheet.Cells[2, 1].Value =
                        $"الخط: {RouteName}";

                    sheet.Cells[2, 1, 2, 10].Merge = true;

                    sheet.Cells[2, 1].Style.Font.Bold = true;

                    sheet.Cells[2, 1]
                        .Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Center;

                    // ========================================================
                    // الفترة
                    // ========================================================

                    if (FromDate.HasValue ||
                        ToDate.HasValue)
                    {
                        var fromText =
                            FromDate.HasValue
                                ? FromDate.Value
                                    .ToString("yyyy-MM-dd")
                                : "";

                        var toText =
                            ToDate.HasValue
                                ? ToDate.Value
                                    .ToString("yyyy-MM-dd")
                                : "";

                        sheet.Cells[3, 1].Value =
                            $"الفترة من {fromText} إلى {toText}";

                        sheet.Cells[3, 1, 3, 10].Merge = true;

                        sheet.Cells[3, 1]
                            .Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.Center;
                    }
                    else if (dateSearch.HasValue)
                    {
                        sheet.Cells[3, 1].Value =
                            $"الشهر: {dateSearch.Value:yyyy-MM}";

                        sheet.Cells[3, 1, 3, 10].Merge = true;

                        sheet.Cells[3, 1]
                            .Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.Center;
                    }

                    // ========================================================
                    // Headers
                    // ========================================================

                    string[] headers =
                    {
            "اسم الطريق",
            "اسم الخط",
            "رقم الأتوبيس",
            "التاريخ",
            "حضور",
            "انصراف",
            "One Way",
            "عدد الحضور",
            "عدد الانصراف"

        };

                    int headerRow = 5;

                    for (int col = 0;
                         col < headers.Length;
                         col++)
                    {
                        var cell =
                            sheet.Cells[
                                headerRow,
                                col + 1];

                        cell.Value =
                            headers[col];

                        cell.Style.Font.Bold = true;

                        cell.Style.Fill.PatternType =
                            ExcelFillStyle.Solid;

                        cell.Style.Fill.BackgroundColor
                            .SetColor(
                                System.Drawing.Color.FromArgb(
                                    255,
                                    174,
                                    81));

                        cell.Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.Center;

                        cell.Style.VerticalAlignment =
                            ExcelVerticalAlignment.Center;

                        cell.Style.Border.BorderAround(
                            ExcelBorderStyle.Thin);
                    }

                    // ========================================================
                    // البيانات
                    // ========================================================

                    int row = 6;

                    foreach (var item in line.Attendance
                        .OrderBy(x => x.Date)
                        .ThenBy(x => x.Serial))
                    {
                        sheet.Cells[row, 1].Value =
                            item.RouteName;

                        sheet.Cells[row, 2].Value =
                            item.LineName;

                        sheet.Cells[row, 3].Value =
                            item.Serial;

                        sheet.Cells[row, 4].Value =
                            item.Date.ToString(
                                "yyyy-MM-dd");

                        // CheckIn
                        sheet.Cells[row, 5].Value = item.CheckIn ? "✔" : "✘";

                        if (item.CheckIn)
                        {
                            sheet.Cells[row, 5].Style.Font.Color
                                .SetColor(System.Drawing.Color.Green);

                            sheet.Cells[row, 5].Style.Font.Bold = true;
                            sheet.Cells[row, 5].Style.Font.Size = 16;
                        }

                        // CheckOut
                        sheet.Cells[row, 6].Value = item.CheckOut ? "✔" : "✘";

                        if (item.CheckOut)
                        {
                            sheet.Cells[row, 6].Style.Font.Color
                                .SetColor(System.Drawing.Color.Green);

                            sheet.Cells[row, 6].Style.Font.Bold = true;
                            sheet.Cells[row, 6].Style.Font.Size = 16;
                        }

                        // OneWay
                        sheet.Cells[row, 7].Value = item.OneWay ? "✔" : "✘";

                        if (item.OneWay)
                        {
                            sheet.Cells[row, 7].Style.Font.Color
                                .SetColor(System.Drawing.Color.Green);

                            sheet.Cells[row, 7].Style.Font.Bold = true;
                            sheet.Cells[row, 7].Style.Font.Size = 16;
                        }

                        sheet.Cells[row, 8].Value =
                            item.CheckInUsersCount;

                        sheet.Cells[row, 9].Value =
                            item.CheckOutUsersCount;



                        // ====================================================
                        // تنسيق
                        // ====================================================

                        sheet.Cells[
                            row,
                            1,
                            row,
                            9]
                            .Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.Center;

                        sheet.Cells[
                            row,
                            1,
                            row,
                            9]
                            .Style.VerticalAlignment =
                            ExcelVerticalAlignment.Center;

                        sheet.Cells[
                            row,
                            1,
                            row,
                            9]
                            .Style.Border.BorderAround(
                                ExcelBorderStyle.Thin);

                        row++;
                    }

                    // ========================================================
                    // عرض الأعمدة
                    // ========================================================

                    sheet.Column(1).Width = 25;
                    sheet.Column(2).Width = 30;
                    sheet.Column(3).Width = 18;
                    sheet.Column(4).Width = 15;
                    sheet.Column(5).Width = 12;
                    sheet.Column(6).Width = 12;
                    sheet.Column(7).Width = 12;
                    sheet.Column(8).Width = 18;
                    sheet.Column(9).Width = 18;

                    sheet.Cells.Style.Font.Size = 10;

                    // تثبيت الـ Header
                    sheet.View.FreezePanes(6, 1);
                }

                // ============================================================
                // 5. حفظ الملف
                // ============================================================

                var dirPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "Attachments",
                        "PharmaTemplate",
                        "resultTampletes");

                Directory.CreateDirectory(dirPath);

                var fileName =
                    $"BusAttendance_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                var finalFilePath =
                    Path.Combine(
                        dirPath,
                        fileName);

                package.SaveAs(
                    new FileInfo(finalFilePath));

                // ============================================================
                // 6. URL
                // ============================================================

                var relativePath =
                    $"Attachments/PharmaTemplate/resultTampletes/{fileName}";

                response.Data =
                    $"{Globals.baseURL}/{relativePath}";

                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;

                response.Errors.Add(
                    new Error
                    {
                        ErrorCode = "Err10",

                        ErrorMSG =
                            ex.InnerException?.Message
                            ?? ex.Message
                    });

                return response;
            }
        }



        public BaseResponseWithDataAndHeader<List<HrUserCardDto>> getAllUsers(string userName, string Email, long RouteId, int TransportionlineId, long SupplierId, long supplierContactPersonId, DateTime? DateSerach, string serialBus, int PageNo, int NoOfItems)
        {
            BaseResponseWithDataAndHeader<List<HrUserCardDto>> Response = new BaseResponseWithDataAndHeader<List<HrUserCardDto>>();
            Response.Data = new List<HrUserCardDto>();
            Response.Errors = new List<Error>();
            Response.Result = false;    

            try
            {

                Response.Result = true;

                var users = _unitOfWork.HrUsers.FindAll(x => x.Id > 0).ToList();

               
                var vehicleRoutes = GetVehicleRoutes(RouteId, TransportionlineId, SupplierId, supplierContactPersonId, serialBus);
                var transportationVehicleRouteEmployee = _unitOfWork.TransportationVehicleRouteEmployees.FindAllQueryable(x => x.Id > 0, new[] { "HrUser" } );
                var transportationVehicleRouteId = vehicleRoutes.Select(t => t.Id);
                var AllHrUsers = transportationVehicleRouteEmployee.Where(x => x.Active == true && transportationVehicleRouteId.Contains(x.TransportationVehicleRouteId)).Select(x => x.HrUser).Distinct();
               // AllHrUsers = AllHrUsers.OrderBy(x => x.ArfirstName);

                if (!string.IsNullOrEmpty(userName))
                {
                    AllHrUsers = AllHrUsers.Where(a => (a.FirstName + a.MiddleName + a.LastName).Contains(userName.Replace(" ", "")) || (a.Email).Contains(userName) || (a.Mobile).Contains(userName));
                }
                if (Email != null)
                {
                    AllHrUsers = AllHrUsers.Where(a => a.Email == Email).AsQueryable();
                }
                var AllHrUsersList = PagedList<HrUser>.Create(AllHrUsers, PageNo, NoOfItems);

                Response.Data = AllHrUsersList.Select(x => new HrUserCardDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    IsUser = x.IsUser,
                    ImgPath = x.ImgPath
                }).ToList();

                Response.PaginationHeader = new PaginationHeader
                {
                    CurrentPage = PageNo,
                    TotalPages = AllHrUsersList.TotalPages,
                    ItemsPerPage = NoOfItems,
                    TotalItems = AllHrUsersList.TotalCount
                };
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


        private decimal CalculatePercentage(int part, int total)
        {
            return total == 0 ? 0 : ((decimal)part / total) * 100;
        }


        private DateTime GetCurrentEgyptTime()
        {
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
        }
        private TransprotationUserAttedance CreateNewAttendance(dynamic entry, DateTime checkTime)
        {
            return new TransprotationUserAttedance
            {
                Serial = entry.EmployeeNumber,
                CheckIn = entry.TYPE == "I" ? checkTime : null,
                CheckOut = entry.TYPE == "O" ? checkTime : null,
                CreationBy = validation.userID,
                CreationDate = checkTime,
                Type = entry.BusType
            };
        }
        private void UpdateExistingAttendance(
              TransprotationUserAttedance attendance,
              dynamic entry,
              DateTime checkTime,
              DateTime fullDateTime,
              List<TransportationVehicleRoute> transportationRoutes,
              List<TransportationVehicleRouteAccount> routeAccounts)
        {

            UpdateCheckInOut(attendance, entry, checkTime, fullDateTime, transportationRoutes, routeAccounts);

            //else if (entry.BusType == "Bus")
            //{
            //    UpdateCheckInOut(attendance, entry, checkTime, fullDateTime, transportationRoutes, routeAccounts);
            //    UpdateOrCreateRouteAccount(entry, fullDateTime, transportationRoutes, routeAccounts);
            //}

            // _unitOfWork.TransprotationUserAttedances.Update(attendance);
        }

        private void UpdateCheckInOut(TransprotationUserAttedance attendance, dynamic entry, DateTime checkTime, DateTime fullDateTime,
            List<TransportationVehicleRoute> transportationRoutes,
            List<TransportationVehicleRouteAccount> routeAccounts)
        {
            if (entry.TYPE == "I" && attendance.CheckIn == null)
            {
                if (entry.BusType == "Bus")
                {
                    UpdateOrCreateRouteAccount(attendance, entry, fullDateTime, checkTime, transportationRoutes, routeAccounts);
                }
                attendance.CheckIn = checkTime;
            }

            if (entry.TYPE == "O" && attendance.CheckOut == null)
            {
                if (entry.BusType == "Bus")
                {
                    UpdateOrCreateRouteAccount(attendance, entry, fullDateTime, checkTime, transportationRoutes, routeAccounts);
                }
                attendance.CheckOut = checkTime;
            }

            attendance.CreationBy = attendance.CreationBy == null ? validation.userID : attendance.CreationBy;
            attendance.CreationDate = attendance.CreationDate == default ? checkTime : attendance.CreationDate;
            attendance.Type = entry.BusType;
        }

        private void UpdateOrCreateRouteAccount(TransprotationUserAttedance attendance,
            dynamic entry,
            DateTime fullDateTime,
            DateTime checkTime,
            List<TransportationVehicleRoute> transportationRoutes,
            List<TransportationVehicleRouteAccount> routeAccounts)
        {
            long routeId = transportationRoutes.FirstOrDefault(x => x.Serial == entry.EmployeeNumber)?.Id ?? 0;
            if (routeId == 0) return;

            var existingAccount = routeAccounts.FirstOrDefault(x =>
                x.TransportationVehicleRouteId == routeId &&
                x.DateOfMonth.Month == checkTime.Month);

            if (existingAccount == null)
            {
                var newRouteAccount = _unitOfWork.TransportationVehicleRouteAccounts.Add(new TransportationVehicleRouteAccount
                {
                    TransportationVehicleRouteId = routeId,
                    Serial = entry.EmployeeNumber,
                    DateOfMonth = checkTime,
                    CountOfRounds = 1,
                    // مش صح
                    PriceOfRounds = 1,
                    CreationBy = validation.userID,
                    CreationDate = checkTime,
                    ModifiedBy = validation.userID,
                    ModifiedDate = fullDateTime
                });
                routeAccounts.Add(newRouteAccount);
            }
            else
            {
                if (attendance == null)
                {
                    if (existingAccount.DateOfMonth.Date != fullDateTime.Date)
                    {
                        existingAccount.CountOfRounds += 1;
                    }
                    existingAccount.CountOfRounds += 1;
                    existingAccount.ModifiedBy = validation.userID;
                    existingAccount.ModifiedDate = fullDateTime;
                }
                else if ((entry.TYPE == "I" && attendance.CheckIn == null) || (entry.TYPE == "O" && attendance.CheckOut == null))
                {
                    if (existingAccount.DateOfMonth.Date != fullDateTime.Date)
                    {
                        existingAccount.CountOfRounds += 1;
                    }
                    existingAccount.CountOfRounds += 1;
                    existingAccount.ModifiedBy = validation.userID;
                    existingAccount.ModifiedDate = fullDateTime;
                }




                // _unitOfWork.TransportationVehicleRouteAccounts.Update(existingAccount);
            }
        }
        private decimal CustomApproximateToFive(decimal number)
        {
            decimal remainder = number % 5;
            if (remainder >= (decimal)2.50)
                return number + (5 - remainder); // Round up
            else
                return number - remainder;       // Round down
        }
        private priceRoundDto CalculatePricePerRoute(long RouteId, int Mounth)
        {
            var route = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == RouteId, new[] { "TransportationLine" }).FirstOrDefault();
            var allRoudsPerRoute = _unitOfWork.TransportationRoundForRoutes.FindAllQueryable(x => x.TransportationVehicleRouteId == RouteId && x.CheckInDate.Value.Month == Mounth).AsQueryable();
            var excetionPrices = _unitOfWork.TransportionLineExceptionPrice.FindAllQueryable(x => x.Id == route.Id).AsQueryable();
            var data = new priceRoundDto();
            foreach (var round in allRoudsPerRoute)
            {
                var exceptionPrice = excetionPrices.Where(x => x.FromDate <= round.CheckInDate || x.FromDate <= round.CheckOutDate.Value.AddHours(-14)
                                                            || x.FromDate <= round.OneWayDate).OrderByDescending(x => x.FromDate).FirstOrDefault();
                if (exceptionPrice != null)
                {
                    data.TotalPrice += exceptionPrice.Price;
                }
                else
                {
                    data.TotalPrice += route.LineCost;
                }
                data.TotalRounds = allRoudsPerRoute.Count();
            }

            return data;
        }

        private decimal CalculatePricePerRouteFromExceptionPrice(long RouteId, DateTime DateOfRoute)
        {
            var exceptionPriceDb = _unitOfWork.TransportionLineExceptionPrice.FindAllQueryable(x => x.RouteId == RouteId && x.FromDate <= DateOfRoute).OrderByDescending(x => x.FromDate).FirstOrDefault();
            var route = _unitOfWork.TransportationVehicleRoutes.FindAll(x => x.Id == RouteId).FirstOrDefault();

            if (exceptionPriceDb != null)
            {
                if (route.OneWay == true)
                {
                    return exceptionPriceDb.Price;
                }
                else
                {
                    return exceptionPriceDb.Price / 2;
                }
            }

            else
            {
                if (route.OneWay == true)
                {
                    return route.LineCost;
                }
                else
                {
                    return route.LineCost / 2;
                }
            }

        }


        private decimal CalculatePricePerRouteFromExceptionPrice22(long RouteId, DateTime DateOfRoute)
        {
            var exceptionPriceDb = _unitOfWork.TransportionLineExceptionPrice.FindAllQueryable(x => x.RouteId == RouteId && x.FromDate <= DateOfRoute).OrderByDescending(x => x.FromDate).FirstOrDefault();

            return exceptionPriceDb.Price;
        }

        //    public static DateTime SafeConvertToDateTime(string dateString)
        //    {
        //        if (string.IsNullOrWhiteSpace(dateString))
        //            return new DateTime(1753, 1, 1); // 🛡️ قيمة آمنة لـ SQL Server

        //        // 🛠️ قائمة بالتنسيقات المحتملة (تاريخ فقط أو تاريخ ووقت)
        //        string[] formats = {
        //    //"d/M/yyyy",
        //    //"MM/dd/yyyy",
        //    //"d/M/yyyy HH:mm",
        //    //"d/M/yyyy HH:mm:ss",
        //    "dd/MM/yyyy HH:mm",
        //    //"yyyy-MM-dd",
        //    //"MM/dd/yyyy" 
        //};

        //        if (DateTime.TryParseExact(dateString.Trim(), formats,
        //            System.Globalization.CultureInfo.InvariantCulture,
        //            System.Globalization.DateTimeStyles.None,
        //            out DateTime date))
        //        {
        //            // نتحقق أن التاريخ الناتج أكبر من الحد الأدنى لـ SQL
        //            return date < new DateTime(1753, 1, 1) ? new DateTime(1753, 1, 1) : date;
        //        }

        //        // محاولة أخيرة بالتحويل العادي
        //        if (DateTime.TryParse(dateString, out DateTime fallbackDate))
        //        {
        //            return fallbackDate < new DateTime(1753, 1, 1) ? new DateTime(1753, 1, 1) : fallbackDate;
        //        }

        //        return new DateTime(1753, 1, 1); // 🛡️ لا ترجع MinValue أبداً
        //    }

        //public static DateTime SafeConvertToDateTime(string dateString)
        //{


        //    if (DateTime.TryParse(dateString, out DateTime date))
        //    {
        //        return date;
        //    }
        //    return date;

        //}


        public static DateTime SafeConvertToDateTime(string dateString)
        {
            if (DateTime.TryParseExact(
                dateString,
                new[] { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy H:mm:ss", "dd/MM/yyyy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date))
            {
                return date;
            }

            return DateTime.Now.Date; // ✅ بدل MinValue
        }
    }



}
