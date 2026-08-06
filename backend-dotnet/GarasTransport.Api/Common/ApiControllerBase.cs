using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Common;

// Base for all API controllers. Exposes the validated identity (populated by
// HeaderAuthFilter) and convenient access to raw request headers.
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>The validated { UserId, CompanyName } — only set on [HeaderAuth] actions.</summary>
    protected Validation CurrentUser =>
        HttpContext.Items["validation"] as Validation ?? new Validation();

    protected string? Header(string name) => HeaderUtil.Get(Request, name);
    protected int HeaderInt(string name, int fallback) => HeaderUtil.GetInt(Request, name, fallback);
    protected int? HeaderIntOrNull(string name) => HeaderUtil.GetIntOrNull(Request, name);
    protected string? HeaderDecoded(string name) => HeaderUtil.DecodeHeader(HeaderUtil.Get(Request, name));
}
