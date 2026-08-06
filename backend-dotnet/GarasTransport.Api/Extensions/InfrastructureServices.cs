using GarasTransport.Domain.IRepository;
using GarasTransport.Infrastructure.Persistence;
using GarasTransport.Infrastructure.Persistence.Repositories;
using GarasTransport.Infrastructure.Security;
using GarasTransport.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Extensions;

/// <summary>Infrastructure DI (doc §13.3): DbContext, UnitOfWork, tenancy, security helpers.</summary>
public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<TenantRegistry>();

        // Database-per-company: each request's DbContext connects to the DB of the
        // CompanyName header (validated later by HeaderAuthFilter). Requests without
        // the header (login, design-time tools) start on the default connection —
        // login re-targets explicitly after reading the company from its body.
        services.AddDbContext<AppDbContext>((sp, opt) =>
        {
            var registry = sp.GetRequiredService<TenantRegistry>();
            var company = sp.GetService<IHttpContextAccessor>()?.HttpContext?.Request.Headers["CompanyName"].ToString();
            opt.UseSqlServer(registry.ResolveOrDefault(company));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<TokenCrypto>();

        return services;
    }
}
