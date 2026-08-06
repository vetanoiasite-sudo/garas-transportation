using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Stations;

[Route("api/Transportation")]
[HeaderAuth]
public class StationsController : ApiControllerBase
{
    private readonly StationsService _service;

    public StationsController(StationsService service) => _service = service;

    [HttpGet("GetTransportationDirection")]
    public Task<object> List() => _service.List(HeaderInt("RouteId", 0));

    [HttpPost("AddTransportationDirection")]
    public Task<object> Add([FromBody] StationAddBody body) =>
        _service.Add(HeaderInt("TransportationVehicleRouteId", 0), body?.Data ?? new());

    [HttpPost("UpdateTransportationDirection")]
    public Task<object> Update([FromBody] StationInput body) => _service.Update(body?.Id ?? 0, body ?? new());

    [HttpPost("DeleteTransportationDirection")]
    public Task<object> Remove() => _service.Remove(HeaderInt("Id", 0));
}

public class StationAddBody
{
    public List<StationInput>? Data { get; set; }
}
