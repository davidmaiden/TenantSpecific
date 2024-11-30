This repo is to demonstrate how it is possible to introduce Tenant-Specific business logic into a Multi-Tenant application. To illustrate this, the out-of-the-box Weather Forecast for Minimal API template has been utilized on the premise that the default business logic delivers an array of 5 weather forecasts. The supposed tenant specific logic is that Tenant A only requires a single weather forecast in the returned array, Tenant B requires two weather forecasts and Tanant C is happy to take the default full 5 weather forecasts.
This achieved by adding a Factory for a WeatherForecastService so that Tenant Specific WeatherForecastServices can be added to the factory when developed. This is shown below.

```public static class ConfigureTenantSpecificServices
{
    public static void AddTenantSpecificServices(this IServiceCollection services)
    {
        services.AddScoped<IKeyedServiceFactory<IWeatherForecastService>, KeyedServiceFactory<IWeatherForecastService>>();
        services.AddKeyedScoped<IWeatherForecastService, WeatherForecastService>("default");
        services.AddKeyedScoped<IWeatherForecastService, Tenant1WeatherForecastService>("TenantA");
        services.AddKeyedScoped<IWeatherForecastService, Tenant2WeatherForecastService>("TenantB");
    }
}
```
This method is called when building the application and adding the services during bootstrapping the application thus:
```
builder.Services.AddTenantSpecificServices();
```
The second key part of this is the identification of tenants which is done in the HTTP Pipeline, in this case for ease of demonstration it is looking for a tenantId in a custom header of x-tenant-id. This could be done of course via the use of host headers, or even a tenantId embedded in a JWT token from the authentication server for instance. The majority of the code for this was leveraged from a repo in GitHub SaaSkit.Multitenancy. The tenant is resolved per request through the pipeline in the AppTenantResolver, obviously this could be cached but didn't feel this was needed for demonstration purposes.
```
public class AppTenantResolver : ITenantResolver<AppTenant>
{
    private readonly Dictionary<int, string> mappings = new Dictionary<int, string>()
        {
            { 100, "TenantA"},
            { 200, "TenantB"},
            { 300, "TenantC"}
        };

    public Task<TenantContext<AppTenant>?> ResolveAsync(HttpContext context)
    {
        string? tenantName;
        TenantContext<AppTenant>? tenantContext = null;

        if (context.Request.Headers["x-tenant-id"] != StringValues.Empty)
        {
            int tenantId = int.Parse(context.Request.Headers["x-tenant-id"]!);


            if (mappings.TryGetValue(tenantId, out tenantName))
            {
                tenantContext = new TenantContext<AppTenant>(new AppTenant { Name = tenantName, TenantId = tenantId });
                tenantContext.Properties.Add("Created", DateTime.UtcNow);
            }
        }

        return Task.FromResult(tenantContext);
    }
}
```

This is then added to the application during bootstrapping.
```
builder.Services.AddMultiTenancy<AppTenant, AppTenantResolver>();
var app = builder.Build();
app.UseMultiTenancy<AppTenant>();
```
Once the tenant and weather forecast factory have been added to the Services Collection they can be resolved via Dependency Injection in the delegate (in this case) for the Minimal API endpoint. The factory is called to deliver a WeatherForecastService for the tenant - if a tenant specific service is not in the Keyed Services Collection, as is the case for Tenant C, then the default WeatherForecastService is returned and used.
```
app.MapGet("/weatherforecast", (ITenant<AppTenant> tenant, IKeyedServiceFactory<IWeatherForecastService> serviceFactory ) =>
{
    var weatherForecastService = serviceFactory.GetTenantServiceOrDefault(tenant.Value.Name!);   

    return weatherForecastService.GetWeatherForecast();
});
```





