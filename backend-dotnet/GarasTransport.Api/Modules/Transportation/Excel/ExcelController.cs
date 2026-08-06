using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Excel;

// Base path /api/Transportation (§6.13). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class ExcelController : ApiControllerBase
{
    private readonly ExcelService _service;

    public ExcelController(ExcelService service) => _service = service;

    [HttpGet("downloadExcelUserWithRoutesTemplete")]
    public Task<object> DownloadUserWithRoutesTemplate() => _service.DownloadUserWithRoutesTemplate();

    [HttpGet("downloadExcelUserActiveTemplete")]
    public Task<object> DownloadUserActiveTemplate() => _service.DownloadUserActiveTemplate();

    [HttpGet("downloadExcelUsersList")]
    public Task<object> UsersListExcel() => _service.UsersListExcel();

    [HttpPost("InsertUsersWithRoutesExcel")]
    public Task<object> InsertUsersWithRoutesExcel([FromBody] FileBody body) =>
        _service.InsertUsersWithRoutesExcel(body?.File);

    [HttpPost("InsertUserNotActiveExcel")]
    public Task<object> InsertUserNotActiveExcel([FromBody] FileBody body) =>
        _service.InsertUserNotActiveExcel(body?.File);
}

public class FileBody
{
    public string? File { get; set; }
}
