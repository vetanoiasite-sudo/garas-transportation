using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Shifts;

// Base path /HR/BranchSchedule (§6.7 — shifts live under the HR prefix, NOT
// /api/Transportation). Every action requires the CompanyName + UserToken headers.
[Route("HR/BranchSchedule")]
[HeaderAuth]
public class ShiftsController : ApiControllerBase
{
    private readonly ShiftsService _service;

    public ShiftsController(ShiftsService service) => _service = service;

    [HttpGet("GetShifts")]
    public Task<object> GetShifts() => _service.GetShifts(HeaderIntOrNull("BranchId"));

    [HttpPost("AddListOfShifts")]
    public Task<object> AddListOfShifts([FromBody] ShiftsBody body) =>
        _service.AddListOfShifts(body?.Shifts, CurrentUser.UserId);

    [HttpPost("UpdateShift")]
    public Task<object> UpdateShift([FromBody] ShiftsBody body) =>
        _service.UpdateShift(body?.Shifts, CurrentUser.UserId);
}

public class ShiftsBody
{
    public int? Id { get; set; }
    public List<ShiftInput>? Shifts { get; set; }
}
