using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Lines;

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the filter. Filters/pagination
// ride in HTTP headers (PageNo/NoOfItems), and deletes/approvals are POSTs with
// an Id header — exactly as documented.
[Route("api/Transportation")]
[HeaderAuth]
public class LinesController : ApiControllerBase
{
    private readonly LinesService _service;

    public LinesController(LinesService service) => _service = service;

    [HttpGet("getAllTransportationLine")]
    public Task<object> GetAll() =>
        _service.GetAll(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), HeaderDecoded("Name"));

    [HttpPost("AddTransportationLine")]
    public Task<object> Add([FromBody] LineBody body) => _service.Add(body?.LineName, CurrentUser.UserId);

    [HttpPost("UpdateTransportationLine")]
    public Task<object> Update([FromBody] LineBody body) =>
        _service.Update(body?.Id ?? 0, body?.LineName, CurrentUser.UserId);

    [HttpPost("DeleteTransportationLine")]
    public Task<object> Remove() => _service.Remove(HeaderInt("Id", 0));

    [HttpPost("ApproveTransportationLine")]
    public Task<object> Approve() =>
        _service.Approve(HeaderInt("Id", 0), string.Equals(Header("Approve"), "true", StringComparison.OrdinalIgnoreCase), CurrentUser.UserId);
}

public class LineBody
{
    public int? Id { get; set; }
    public string? LineName { get; set; }
}
