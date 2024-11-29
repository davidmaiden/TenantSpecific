using Microsoft.Extensions.Primitives;
using TenantSpecific.Interfaces;
using TenantSpecific.Tenants;

namespace TenantSpecific.Application;

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


