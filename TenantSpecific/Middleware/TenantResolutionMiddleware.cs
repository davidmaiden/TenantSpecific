using TenantSpecific.Extensions;
using TenantSpecific.Interfaces;

namespace TenantSpecific.Middleware;

public class TenantResolutionMiddleware<TTenant>
{
    private readonly RequestDelegate next;
    private readonly ILogger log;

    public TenantResolutionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(next, nameof(next));
        ArgumentNullException.ThrowIfNull(loggerFactory, nameof(loggerFactory));

        this.next = next;
        this.log = loggerFactory.CreateLogger<TenantResolutionMiddleware<TTenant>>();
    }

    public async Task Invoke(HttpContext context, ITenantResolver<TTenant> tenantResolver)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(tenantResolver, nameof(tenantResolver));

        log.LogDebug("Resolving TenantContext using {loggerType}.", tenantResolver.GetType().Name);

        var tenantContext = await tenantResolver.ResolveAsync(context);

        if (tenantContext != null)
        {
            log.LogDebug("TenantContext Resolved. Adding to HttpContext.");
            context.SetTenantContext(tenantContext);
        }
        else
        {
            log.LogDebug("TenantContext Not Resolved.");
        }

        await next.Invoke(context);
    }
}
