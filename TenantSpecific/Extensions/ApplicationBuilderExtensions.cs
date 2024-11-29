using TenantSpecific.Middleware;

namespace TenantSpecific.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMultiTenancy<TTenant>(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        return app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();
    }
}
