using TenantSpecific.Factories;
using TenantSpecific.Interfaces;
using TenantSpecific.Services;

namespace TenantSpecific.Configuration;

public static class ConfigureTenantSpecificServices
{
    public static void AddTenantSpecificServices(this IServiceCollection services)
    {
        services.AddScoped<IKeyedServiceFactory<IWeatherForecastService>, KeyedServiceFactory<IWeatherForecastService>>();
        services.AddKeyedScoped<IWeatherForecastService, WeatherForecastService>("default");
        services.AddKeyedScoped<IWeatherForecastService, Tenant1WeatherForecastService>("TenantA");
        services.AddKeyedScoped<IWeatherForecastService, Tenant2WeatherForecastService>("TenantB");
    }
}
