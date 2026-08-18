using NewGaras.Domain.DTO.HrUser;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Models;
using NewGaras.Infrastructure.Models.TransportationLine;
using NewGaras.Infrastructure.Models.TransportationLineModel;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces ;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine
{
    public interface ITransportationLineService
    {
        public HearderVaidatorOutput Validation { get; set; }
        public BaseResponseWithData<List<VBusProgram>> Bus_Program();

        public BaseResponseWithData<List<VehicleType>> getAllVehicleType();
        public BaseResponse AddVehicleType(VehicleTypeVM dto);
        public BaseResponse UpdateVehicleType(VehicleTypeVM dto);
        public BaseResponse DeleteVehicleType([FromHeader] int Id);
        public BaseResponseWithDataAndHeader<List<getTransportationVehicleDto>> getAllTransportationVehicle(long RouteId , int transportationLineId , long supplierId , long supplierContactPersonId , DateTime? dateSearch , string serialBus , int PageNo , int NoOfItems);
        public BaseResponse AddTransportationVehicle(TransportationVehicleVM dto);
        public BaseResponse UpdateTransportationVehicle(TransportationVehicleVM dto);
        public BaseResponse DeleteTransportationVehicle(int Id);
        public BaseResponseWithDataAndHeader<List<TransportationLineVM>> getAllRoutes(long RouteId , string Name , int transportationLineId , long supplierId , long supplierContactPersonId , string serialBus , int PageNo , int NoOfItems , bool Active);
        public BaseResponse AddTransportationLine(TransportationLineVM dto);
        public BaseResponse UpdateTransportationLine(TransportationLineVM dto);
        public BaseResponse DeleteTransportationLine(int Id);
        public BaseResponse ApproveRoute(long Id , bool Approve);
        public BaseResponse ApproveTransportationVehicle(int Id , bool Approve);
        public Task<BaseResponse> AddTransportationRoute(TransportationVehicleRouteVM dto);
        public BaseResponse UpdateTransportationRoute(TransportationRouteUpdateVM dto);
        public BaseResponse DeleteTransportationRoute(long Id);
        public BaseResponse AddTransportationDirection(TransportationDirectionDto dto , long TransportationVehicleRouteId);
        public BaseResponse UpdateTransportationDirection(UpdateTransportationDirectionVM dto);
        public BaseResponse DeleteTransportationDirection(long Id);
        public BaseResponse AddTransportationEmployee(TransportationEmployeeDto dto , long TransportationVehicleRouteId);

        public BaseResponse UpdateTransportationEmployee(UpdateTransportationEmployeeVM dto);
        public BaseResponse DeleteTransportationEmployee(long Id);
        public BaseResponse UpdatePriceOfTransportationLine(int Id);
        public BaseResponse RejectUpdatePrice(int Id);
        public Task<BaseResponse> ModifyPriceOfTransportationLine(ModifyTransportationLinePriceVM dto);
        public BaseResponseWithDataAndHeader<List<RouteDeductionVM>> getAllDeduction(int Id , DateTime? FromDate , DateTime? ToDate , long SupplierId , long RouteId , string? Name , int PageNo , int NoOfItems);
        public BaseResponse AddDeduction(TransportationVehicleRouteDeduction dto);
        public BaseResponse AddUsersAttedance(TransprotationUserAttedanceVM dto);
        public BaseResponseWithData<List<transportionLineNameVn>> getAllTransportationLine(string? Name);


        public BaseResponseWithDataAndHeader<List<getAllModifyPriceVM>> getAllModifyPriceOfTransportationLine(int PageNo , int NoOfItems , bool? Approve);

        //public BaseResponseWithData<DashBoardVM> DashBoard(int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus);
        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence(int TransportionlineId , long RouteId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , bool? AttendaceFlag , long HrUser , int PageNo , int NoOfItems);

        public Task<BaseResponseWithData<string>> AttendanceExcellDur(int Year , long RouteId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , bool? AttendaceFlag , long HrUser , int PageNo , int NoOfItems);
        public BaseResponseWithDataAndHeader<List<CostOfTransportLineVM>> CostOfLine(int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? date , DateTime? dateFrom , DateTime? dateTo , string serialBus , int PageNo , int NoOfItems);
        public BaseResponseWithData<string> ReportCostOfLinesExcell(int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? date , DateTime? dateFrom , DateTime? dateTo , string serialBus , int PageNo , int NoOfItems);
        public BaseResponseWithDataAndHeader<List<TransportationRouteVM>> getAllTransportationRoute(long RouteId , string Name , int transportationLineId , long supplierId , long supplierContactPersonId , string serialBus , int PageNo , int NoOfItems , bool Active);
        public BaseResponseWithData<TransportationRouteVM> getTransportationRoute(long RouteId);
        public BaseResponseWithData<List<TransportationDirectionByRouteIdVm>> GetTransportationDirection(long routeId);
        public BaseResponseWithData<List<EmployeeBasicInfoVM>> GetTransportationEmployeeList(long RouteId);
        public BaseResponseWithData<List<TransportationRouteVM>> getTransportationRouteByHrUser(long HrUserId);
        public BaseResponseWithDataAndHeader<List<EmployeeExceptionVM>> GetEmployeeException(long? HrUserId , long? Id , int PageNo , int NoOfItems);
        public BaseResponse AddException(TransportationVehicleRouteEmployeeException dto);
        public BaseResponse UpdateException(TransportationVehicleRouteEmployeeException dto);
        public BaseResponseWithData<CapacityTransportionNumbersVM> GetCapacityNumbers(long RouteID , string Period , DateTime? DateSearch);
        public BaseResponseWithData<List<EmployeeBasicInfoVM>> GetTransportationEmployees(long RouteID , string Period , DateTime? DateSearch);
        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence22(int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , long HrUser , int PageNo , int NoOfItems);

        public BaseResponse UpdateDeduction(TransportationVehicleRouteDeduction dto);
        public IQueryable<TransportationVehicleRoute> GetVehicleRoutes(long RouteId , int lineId , long supplierId , long contactPersonId , string serial);
        public BaseResponse AddUsersAttedance44();
        public Task<BaseResponse> AlterAddUsersAttendance();
        public Task<BaseResponseWithData<string>> InsertPharmaExcel(InsertHrUserExcelVM dto);
        public BaseResponse AddRoutesForEmployee(AddRoutesForHrUserDto dto , long HrUserId);
        public BaseResponseWithData<TransportationRouteDetailsVM> getTransportationRouteDetails(long RouteId , DateTime FromDate , DateTime ToDate);
        public BaseResponseWithData<List<getTransportationRoutesForHrUserVm>> getTransportationRoutesForHrUser(long HrUserId);
        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsers>> DashBoardForAttendence88(int TransportionlineId , long RouteId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , bool? AttendaceFlag , long HrUser , int PageNo , int NoOfItems);
        public BaseResponseWithDataAndHeader<List<DashBoardForHrUsersFromDuration>> DashBoardForAttendenceDuration(int TransportionlineId , long RouteId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , bool? AttendaceFlag , long HrUser , string? Email , int PageNo , int NoOfItems);
        public BaseResponse DeleteRouteEmployee(int Id);
        public BaseResponse UpdateRouteEmployee(UpdateRouteEmployeeVM dto);
        public Task<BaseResponseWithData<string>> AttendanceExcell(int Year , int TransportionlineId , long RouteId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , DateTime? FromDate , DateTime? ToDate , bool? AttendaceFlag , long HrUser , string? Email , int PageNo , int NoOfItems);
        public BaseResponse AddUsersAttedanceUpdated();
        public BaseResponseWithData<DashBoardVM> DashBoard22(long RouteId , int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus);
        public BaseResponseWithData<DashBoardVM> DashBoard(long RouteId , int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus);
        public Task<BaseResponse> CreateHrUserWithAllRoutes(CreateHrUserWithAllRoutesDto NewHrUser , long UserId , string CompanyName);
        public BaseResponseWithData<string> downloadExcelUserActiveTemplete();
        public BaseResponseWithData<string> downloadExcelUserWithRoutesTemplete();
        public Task<BaseResponseWithData<string>> InsertUsersWithRoutesExcel(IFormFile ExcelSheet);
        public Task<BaseResponseWithData<string>> InsertUserNotActiveExcel(IFormFile ExcelSheet);
        public BaseResponse AddUsersAttedance66();

        public BaseResponseWithDataAndHeader<List<AccountDto>> AccountsAllMonths(int Year , long RouteID , long SupplierID , int Month , int PageNo , int NoOfItems);
        public BaseResponseWithDataAndHeader<List<SupplierPaymentDto>> getAllSupplierPayment(long SupplierId , DateTime? FromDate , DateTime? ToDate , int PageNo , int NoOfItems);
        public BaseResponse AddSupplierPayment(TransportionLineSupplierPayment dto);
        public BaseResponseWithDataAndHeader<List<AccountsAllRoundsDto>> AccountsAllRoundsForSupplier(int Year , long SupplierId , long RouteID , int Month , int PageNo , int NoOfItems);

        public BaseResponseWithDataAndHeader<List<AccountDto>> AccountsAllMonths22(int Year , long SupplierID , long RouteID , int Month , int PageNo , int NoOfItems);
        public BaseResponse UpdateRoutePrice();
        public BaseResponseWithData<string> downloadExcelRoutesTemplete();
        public Task<BaseResponseWithData<string>> RoutesWithUsersExcell(long? RouteId , long? SupplierId);
        public Task<BaseResponseWithData<string>> getAllDeductionExcel(int Id , DateTime? FromDate , DateTime? ToDate , long SupplierId , long RouteId , string? Name , int PageNo , int NoOfItems);

        public Task<BaseResponseWithData<string>> AccountsAllMonthsForSupplierExcel(int Year , long SupplierID , long RouteID , int Month , int PageNo , int NoOfItems);

        public Task<BaseResponseWithDataAndHeader<List<BusAttendanceLineVm>>>
    BusAttendance(
        DateTime? dateSearch ,
        DateTime? FromDate ,
        DateTime? ToDate ,
        long SupplierId ,
        long supplierContactPersonId ,
        long RouteId ,
        string? Name ,
        string? serialBus ,
        int PageNo ,
        int NoOfItems);



        public Task<BaseResponseWithData<string>> BusAttendanceExcel(
    DateTime? dateSearch ,
    DateTime? FromDate ,
    DateTime? ToDate ,
    long SupplierId ,
    long supplierContactPersonId ,
    long RouteId ,
    string? Name ,
    string? serialBus ,
    int PageNo ,
    int NoOfItems);



        public BaseResponseWithDataAndHeader<List<HrUserCardDto>> getAllUsers(string userName , string Email , long RouteId , int TransportionlineId , long SupplierId , long supplierContactPersonId , DateTime? DateSerach , string serialBus , int PageNo , int NoOfItems);


    }
}
