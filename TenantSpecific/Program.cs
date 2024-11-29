using TenantSpecific.Application;
using TenantSpecific.Configuration;
using TenantSpecific.Extensions;
using TenantSpecific.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddMultiTenancy<AppTenant, AppTenantResolver>();

builder.Services.AddTenantSpecificServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMultiTenancy<AppTenant>();

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", (ITenant<AppTenant> tenant, IKeyedServiceFactory<IWeatherForecastService> serviceFactory ) =>
{
    var weatherForecastService = serviceFactory.GetTenantServiceOrDefault(tenant.Value.Name!);   

    return weatherForecastService.GetWeatherForecast();
});

app.Run();

