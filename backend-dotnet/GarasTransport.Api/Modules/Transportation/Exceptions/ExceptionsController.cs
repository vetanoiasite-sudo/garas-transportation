using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Exceptions;

// Base path /api/Transportation (§6.8). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class ExceptionsController : ApiControllerBase
{
    private readonly ExceptionsService _service;

    public ExceptionsController(ExceptionsService service) => _service = service;

    [HttpGet("GetEmployeeException")]
    public Task<object> GetAll() =>
        _service.GetAll(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), HeaderIntOrNull("HrUserId"), HeaderIntOrNull("Id"));

    [HttpPost("AddException")]
    public Task<object> Add([FromBody] ExceptionBody body) => _service.Add(body ?? new(), CurrentUser.UserId);

    [HttpPost("UpdateException")]
    public Task<object> Update([FromBody] ExceptionBody body) => _service.Update(body?.id ?? 0, body ?? new(), CurrentUser.UserId);

    [HttpGet("GetCapacityNumbers")]
    public Task<object> GetCapacity() =>
        _service.GetCapacity(HeaderInt("RouteId", 0), Header("Period"), Header("DateSerach"));
}
