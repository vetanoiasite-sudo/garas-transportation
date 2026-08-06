using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Deductions;

// Base path /api/Transportation (§6.10). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class DeductionsController : ApiControllerBase
{
    private readonly DeductionsService _service;

    public DeductionsController(DeductionsService service) => _service = service;

    private DeductionFilters Filters() => new()
    {
        SupplierId = HeaderIntOrNull("SupplierId"),
        RouteId = HeaderIntOrNull("RouteId"),
        FromDate = Header("FromDate"),
        ToDate = Header("ToDate"),
        Name = HeaderDecoded("Name"),
    };

    [HttpGet("getAllDeduction")]
    public Task<object> GetAll() => _service.GetAll(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), Filters());

    [HttpGet("getAllDeductionExcel")]
    public Task<object> Excel() => _service.Excel(Filters());

    [HttpPost("AddDeduction")]
    public Task<object> Add([FromBody] DeductionBody body) => _service.Add(body ?? new(), CurrentUser.UserId);

    [HttpPost("UpdateDeduction")]
    public Task<object> Update([FromBody] DeductionBody body) => _service.Update(body?.Id ?? 0, body ?? new());
}
