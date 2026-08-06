using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Dashboard;

// Base path /api/Transportation (§6.9). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class DashboardController : ApiControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(DashboardService service) => _service = service;

    [HttpGet("DashBoard")]
    public Task<object> Dashboard() =>
        _service.Dashboard(Header("DateSerach"), new DashboardFilters
        {
            LineId = HeaderIntOrNull("TransportionLineId"),
            SupplierId = HeaderIntOrNull("SupplierId"),
            SerialBus = Header("SerialBus"),
            RouteId = HeaderIntOrNull("RouteId"),
        });

    private AttendanceFilters AttendanceFilters() => new()
    {
        FromDate = Header("FromDate"),
        ToDate = Header("ToDate"),
        LineId = HeaderIntOrNull("TransportionLineId"),
        SupplierId = HeaderIntOrNull("SupplierId"),
        SerialBus = Header("SerialBus"),
        RouteId = HeaderIntOrNull("RouteId"),
        AttendanceFlag = Header("AttendaceFlag"),
        EmployeeId = Header("EmployeeId"),
    };

    [HttpGet("DashBoardForAttendenceDuration")]
    public Task<object> DashboardAttendance() =>
        _service.DashboardAttendance(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), AttendanceFilters());

    [HttpGet("AttendanceExcell")]
    public Task<object> AttendanceExcel() => _service.AttendanceExcel(AttendanceFilters());

    [HttpPost("AddUsersAttedance")]
    public Task<object> AddUsersAttendance([FromBody] AddAttendanceBody body) =>
        _service.AddUsersAttendance(body ?? new(), CurrentUser.UserId);

    [HttpPost("AddUsersAttedance44")]
    public Task<object> AddUsersAttendance44() => _service.AddUsersAttendance44();
}
