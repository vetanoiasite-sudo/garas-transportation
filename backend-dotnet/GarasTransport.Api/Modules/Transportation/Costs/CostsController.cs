using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Costs;

// Base path /api/Transportation (§6.11). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class CostsController : ApiControllerBase
{
    private readonly CostsService _service;

    public CostsController(CostsService service) => _service = service;

    private CostFilters Filters() => new()
    {
        SupplierId = HeaderIntOrNull("SupplierId"),
        LineId = HeaderIntOrNull("TransportionLineId"),
        SerialBus = Header("SerialBus"),
        DateFrom = Header("DateFrom"),
        DateTo = Header("DateTo"),
    };

    [HttpGet("ReportCostOfLines")]
    public Task<object> ReportCostOfLines() =>
        _service.ReportCostOfLines(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), Filters());

    [HttpGet("ReportCostOfLinesExcell")]
    public Task<object> ReportCostOfLinesExcel() => _service.ReportCostOfLinesExcel(Filters());
}
