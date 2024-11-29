using TenantSpecific.Tenants;

namespace TenantSpecific.Extensions;

public static class HttpContextExtensions
{
    private const string TenantContextKey = "TenantContext";

    public static void SetTenantContext<TTenant>(this HttpContext context, TenantContext<TTenant> tenantContext)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(tenantContext, nameof(tenantContext));

        context.Items[TenantContextKey] = tenantContext;
    }

    public static TenantContext<TTenant>? GetTenantContext<TTenant>(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        object? tenantContext;
        if (context.Items.TryGetValue(TenantContextKey, out tenantContext))
        {
            return tenantContext as TenantContext<TTenant>;
        }

        return null;
    }

    public static TTenant? GetTenant<TTenant>(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var tenantContext = GetTenantContext<TTenant>(context);

        if (tenantContext != null)
        {
            return tenantContext.Tenant;
        }

        return default;
    }
}
