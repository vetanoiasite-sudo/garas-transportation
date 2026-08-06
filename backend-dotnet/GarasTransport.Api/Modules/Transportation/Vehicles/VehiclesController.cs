using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Vehicles;

// Base path /api/Transportation (§6.1 — vehicles & vehicle types).
[Route("api/Transportation")]
[HeaderAuth]
public class VehiclesController : ApiControllerBase
{
    private readonly VehiclesService _service;

    public VehiclesController(VehiclesService service) => _service = service;

    [HttpGet("getAllVehicleType")]
    public Task<object> GetAllVehicleType() => _service.GetAllVehicleType();

    [HttpPost("AddVehicleType")]
    public Task<object> AddVehicleType([FromBody] VehicleTypeBody body) =>
        _service.AddVehicleType(body?.Type, body?.Active);

    [HttpPost("UpdateVehicleType")]
    public Task<object> UpdateVehicleType([FromBody] VehicleTypeBody body) =>
        _service.UpdateVehicleType(body?.Id ?? 0, body?.Type, body?.Active);

    [HttpGet("getAllTransportationVehicle")]
    public Task<object> GetAllVehicles() =>
        _service.GetAllVehicles(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20));

    [HttpPost("AddTransportationVehicle")]
    public Task<object> AddVehicle([FromBody] VehicleBody body) =>
        _service.AddVehicle(body?.VehicleTypeId ?? 0, body?.Capacity ?? 0, body?.Active, CurrentUser.UserId);

    [HttpPost("UpdateTransportationVehicle")]
    public Task<object> UpdateVehicle([FromBody] VehicleBody body) =>
        _service.UpdateVehicle(body?.Id ?? 0, body?.VehicleTypeId ?? 0, body?.Capacity ?? 0, body?.Active, CurrentUser.UserId);

    [HttpPost("DeleteTransportationVehicle")]
    public Task<object> RemoveVehicle() => _service.RemoveVehicle(HeaderInt("Id", 0));

    [HttpPost("ApproveTransportationVehicle")]
    public Task<object> ApproveVehicle() =>
        _service.ApproveVehicle(HeaderInt("Id", 0), string.Equals(Header("Approve"), "true", StringComparison.OrdinalIgnoreCase), CurrentUser.UserId);
}

public class VehicleTypeBody
{
    public int? Id { get; set; }
    public string? Type { get; set; }
    public bool? Active { get; set; }
}

public class VehicleBody
{
    public int? Id { get; set; }
    public int? VehicleTypeId { get; set; }
    public int? Capacity { get; set; }
    public bool? Active { get; set; }
}
