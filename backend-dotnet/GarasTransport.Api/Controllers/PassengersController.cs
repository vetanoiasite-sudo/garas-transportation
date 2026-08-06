using GarasTransport.Api.Common;
using GarasTransport.Application.DTOs.Passenger;
using GarasTransport.Application.IServices.Passengers;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Controllers;

// Base path /api/Transportation (§6.6 — Passengers / route employees). Every
// action requires the CompanyName + UserToken headers via the filter. Routes,
// header parameter names, and response envelopes are all FROZEN — the controller
// only extracts inputs, calls IPassengerService, and maps the result.
[Route("api/Transportation")]
[HeaderAuth]
public class PassengersController : ApiControllerBase
{
    private readonly IPassengerService _service;

    public PassengersController(IPassengerService service) => _service = service;

    [HttpGet("getAllHrUsers")]
    public async Task<object> GetAllHrUsers()
    {
        var raw = Header("Active");
        bool? activeFilter = string.IsNullOrEmpty(raw) ? null : string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase);
        return (await _service.GetAllHrUsersAsync(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), HeaderDecoded("Name"), activeFilter)).ToEnvelope();
    }

    [HttpGet("getHrUser")]
    public async Task<object> GetHrUser() => (await _service.GetHrUserAsync(HeaderInt("Id", 0))).ToEnvelope();

    [HttpGet("GetMaritalStatus")]
    public async Task<object> GetMaritalStatus() => (await _service.GetMaritalStatusAsync()).ToEnvelope();

    [HttpPost("AddHrUser")]
    public async Task<object> AddHrUser([FromBody] HrUserDto body) => (await _service.AddHrUserAsync(body ?? new())).ToEnvelope();

    [HttpPost("UpdateHrUser")]
    public async Task<object> UpdateHrUser([FromBody] HrUserDto body) => (await _service.UpdateHrUserAsync(body?.Id ?? 0, body ?? new())).ToEnvelope();

    [HttpGet("GetTransportationEmployeeList")]
    public async Task<object> GetEmployeeList() => (await _service.GetEmployeeListAsync(HeaderInt("RouteId", 0))).ToEnvelope();

    [HttpGet("GetTransportationEmployees")]
    public async Task<object> GetEmployeesForMob() =>
        (await _service.GetEmployeesForMobAsync(HeaderInt("RouteId", 0), Header("Period"), Header("DateSerach"))).ToEnvelope();

    [HttpPost("AddTransportationEmployee")]
    public async Task<object> AddEmployee([FromBody] EmployeeDataBody body) =>
        (await _service.AddEmployeeAsync(HeaderInt("TransportationVehicleRouteId", 0), body?.Data ?? new())).ToEnvelope();

    [HttpPost("UpdateTransportationEmployee")]
    public async Task<object> UpdateEmployee([FromBody] UpdateEmployeeDto body) => (await _service.UpdateEmployeeAsync(body ?? new())).ToEnvelope();

    [HttpPost("DeleteTransportationEmployee")]
    public async Task<object> DeleteEmployee() => (await _service.DeleteEmployeeAsync(HeaderInt("Id", 0))).ToEnvelope();

    [HttpGet("getTransportationRoutesForHrUser")]
    public async Task<object> GetRoutesForHrUser() => (await _service.GetRoutesForHrUserAsync(HeaderInt("HrUserId", 0))).ToEnvelope();

    [HttpPost("AddRoutesForEmployee")]
    public async Task<object> AddRoutesForEmployee([FromBody] RoutesDataBody body) =>
        (await _service.AddRoutesForEmployeeAsync(HeaderInt("HrUserId", 0), body?.Data ?? new())).ToEnvelope();

    [HttpPost("UpdateRouteEmployee")]
    public async Task<object> UpdateRouteEmployee([FromBody] UpdateRouteEmployeeDto body) => (await _service.UpdateRouteEmployeeAsync(body ?? new())).ToEnvelope();

    [HttpPost("DeleteRouteEmployee")]
    public async Task<object> DeleteRouteEmployee() => (await _service.DeleteRouteEmployeeAsync(HeaderInt("Id", 0))).ToEnvelope();
}

public class EmployeeDataBody
{
    public List<EmployeeInputDto>? Data { get; set; }
}

public class RoutesDataBody
{
    public List<RouteForEmployeeInputDto>? Data { get; set; }
}
