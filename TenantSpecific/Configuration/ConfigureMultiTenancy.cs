using Microsoft.Extensions.DependencyInjection.Extensions;
using TenantSpecific.Extensions;
using TenantSpecific.Interfaces;
using TenantSpecific.Resolvers;
using TenantSpecific.Tenants;

namespace TenantSpecific.Configuration;

public static class ConfigureMultiTenancy
{
    public static IServiceCollection AddMultiTenancy<TTenant, TResolver>(this IServiceCollection services) 
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class
    {
        services.AddScoped<ITenantResolver<TTenant>, TResolver>();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());
        services.AddScoped(sp => sp.GetService<TenantContext<TTenant>>()?.Tenant);

        // Make ITenant injectable for handling null injection, similar to IOptions
        services.AddScoped<ITenant<TTenant>>(sp => new TenantWrapper<TTenant>(sp.GetService<TTenant>()!));

        // Ensure caching is available for caching resolvers
        var resolverType = typeof(TResolver);
        if (typeof(MemoryCacheTenantResolver<TTenant>).IsAssignableFrom(resolverType))
        {
            services.AddMemoryCache();
        }

        return services;
    }

}
