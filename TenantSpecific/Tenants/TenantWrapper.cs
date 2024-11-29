using TenantSpecific.Interfaces;

namespace TenantSpecific.Tenants;

public class TenantWrapper<TTenant> : ITenant<TTenant>
{
    /// <summary>
    /// Initializes the wrapper with the tenant instance to return.
    /// </summary>
    /// <param name="tenant">The tenant instance to return.</param>		
    public TenantWrapper(TTenant tenant)
    {
        Value = tenant;
    }

    /// <summary>
    /// The tenant instance.
    /// </summary>
    public TTenant Value { get; }
}
