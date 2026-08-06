using GarasTransport.Application.IServices.Passengers;
using GarasTransport.Application.Services.Passengers;

namespace GarasTransport.Api.Extensions;

/// <summary>
/// Application DI (doc §13.3). Clean-architecture services register here
/// interface → implementation; one line per migrated module.
/// </summary>
public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPassengerService, PassengerService>();

        return services;
    }
}
