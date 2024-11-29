using TenantSpecific.Models;

namespace TenantSpecific.Interfaces;

public interface IWeatherForecastService : IServiceDefinition
{
    public WeatherForecast[] GetWeatherForecast();

}
