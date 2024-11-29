using TenantSpecific.Tenants;

namespace TenantSpecific.Interfaces;

public interface ITenantResolver<TTenant>
{
    Task<TenantContext<TTenant>?> ResolveAsync(HttpContext context);
}
