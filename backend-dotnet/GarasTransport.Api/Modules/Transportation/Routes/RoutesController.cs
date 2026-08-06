using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Routes;

// Base path /api/Transportation (§6.4). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class RoutesController : ApiControllerBase
{
    private readonly RoutesService _service;

    public RoutesController(RoutesService service) => _service = service;

    [HttpGet("getAllTransportationRoute")]
    public Task<object> GetAll() =>
        _service.GetAll(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), new RouteFilters
        {
            LineId = HeaderIntOrNull("PTransportionLineId"),
            SupplierId = HeaderIntOrNull("SupplierId"),
            SerialBus = Header("SerialBus"),
            Name = HeaderDecoded("Name"),
        });

    [HttpGet("getTransportationRoute")]
    public Task<object> GetOne() => _service.GetOne(HeaderInt("RouteId", 0));

    [HttpGet("getTransportationRouteByHrUser")]
    public Task<object> GetByHrUser() => _service.GetByHrUser(HeaderInt("HrUserId", 0));

    [HttpGet("RoutesWithUsersExcel")]
    public Task<object> RoutesWithUsersExcel() => _service.RoutesWithUsersExcel();

    [HttpPost("AddTransportationRoute")]
    public Task<object> Add([FromBody] RouteBody body) => _service.Add(body ?? new(), CurrentUser.UserId);

    [HttpPost("UpdateTransportationRoute")]
    public Task<object> Update([FromBody] RouteBody body) =>
        _service.Update(body?.Id ?? 0, body ?? new(), CurrentUser.UserId);

    [HttpPost("DeleteTransportationRoute")]
    public Task<object> Remove() => _service.Remove(HeaderInt("Id", 0));

    [HttpPost("ApproveRoute")]
    public Task<object> Approve() =>
        _service.Approve(HeaderInt("Id", 0), string.Equals(Header("Approve"), "true", StringComparison.OrdinalIgnoreCase), CurrentUser.UserId);
}
