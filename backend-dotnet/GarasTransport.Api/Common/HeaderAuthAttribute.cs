using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Common;

/// <summary>Validated identity resolved from the request headers.</summary>
public class Validation
{
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}

// Per-request validation — replicates the old ValidateHeader:
//   1. CompanyName header must be an allowed tenant code (single-tenant: one DB).
//   2. UserToken header decrypts to a session id; the session must be Active and
//      not expired. On failure returns the documented Err-P200 / Err-P2 envelope
//      at HTTP 200 (the Flutter/Next clients string-match these codes).
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HeaderAuthAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider services) =>
        ActivatorUtilities.CreateInstance<HeaderAuthFilter>(services);
}

public class HeaderAuthFilter : IAsyncActionFilter
{
    private readonly AppDbContext _db;
    private readonly TokenCrypto _crypto;
    private readonly GarasTransport.Infrastructure.Tenancy.TenantRegistry _tenants;

    public HeaderAuthFilter(AppDbContext db, TokenCrypto crypto, GarasTransport.Infrastructure.Tenancy.TenantRegistry tenants)
    {
        _db = db;
        _crypto = crypto;
        _tenants = tenants;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var req = context.HttpContext.Request;
        var companyName = HeaderUtil.Get(req, "CompanyName") ?? string.Empty;
        var userToken = HeaderUtil.Get(req, "UserToken") ?? string.Empty;

        // The company must be a configured tenant. The request's DbContext is
        // already targeting this company's own database (see InfrastructureServices).
        if (!_tenants.IsAllowed(companyName))
        {
            context.Result = new OkObjectResult(Envelope.Fail("Err-P200", "Invalid or unknown CompanyName"));
            return;
        }
        if (string.IsNullOrEmpty(userToken))
        {
            context.Result = new OkObjectResult(Envelope.Fail("Err-P2", "Invalid or expired UserToken"));
            return;
        }

        // Token payload is "sessionId|company" — the company inside the token
        // must match the header, otherwise a token from tenant A could hit a
        // same-numbered session in tenant B's database.
        if (!_crypto.TryDecryptSession(Uri.UnescapeDataString(userToken), out var sessionId, out var tokenCompany)
            || sessionId == 0
            || !string.Equals(tokenCompany, companyName, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new OkObjectResult(Envelope.Fail("Err-P2", "Invalid or expired UserToken"));
            return;
        }

        var session = await _db.UserSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.Active && s.EndDate > DateTime.UtcNow);
        if (session == null)
        {
            context.Result = new OkObjectResult(Envelope.Fail("Err-P2", "Invalid or expired UserToken"));
            return;
        }

        context.HttpContext.Items["validation"] = new Validation { UserId = session.UserId, CompanyName = companyName };
        await next();
    }
}
