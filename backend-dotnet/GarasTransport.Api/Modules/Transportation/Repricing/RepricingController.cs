using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Repricing;

// Base path /api/Transportation (§6.3). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class RepricingController : ApiControllerBase
{
    private readonly RepricingService _service;

    public RepricingController(RepricingService service) => _service = service;

    [HttpPost("ModifyPriceOfTransportationLine")]
    public Task<object> Modify([FromBody] RepriceBody body) => _service.Modify(body ?? new(), CurrentUser.UserId);

    [HttpGet("getAllModifyPriceOfTransportationLine")]
    public Task<object> GetAll()
    {
        var raw = Header("Approve");
        bool? approveFilter = string.IsNullOrEmpty(raw) ? null : string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase);
        return _service.GetAll(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), approveFilter);
    }

    [HttpPost("UpdatePriceOfTransportationLine")]
    public Task<object> ApprovePrice() => _service.ApprovePrice(HeaderInt("Id", 0), CurrentUser.UserId);

    [HttpPost("RejectUpdatePrice")]
    public Task<object> RejectPrice() => _service.RejectPrice(HeaderInt("Id", 0), CurrentUser.UserId);
}
