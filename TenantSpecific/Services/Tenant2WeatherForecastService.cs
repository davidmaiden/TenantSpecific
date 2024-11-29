using TenantSpecific.Interfaces;
using TenantSpecific.Models;

namespace TenantSpecific.Services;

public class Tenant2WeatherForecastService : IWeatherForecastService
{
    private string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    WeatherForecast[] IWeatherForecastService.GetWeatherForecast()
    {
        var forecast = Enumerable.Range(1, 2).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

        return forecast;
    }
};
