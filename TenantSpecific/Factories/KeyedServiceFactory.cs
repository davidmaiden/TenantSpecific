using TenantSpecific.Interfaces;

namespace TenantSpecific.Factories;

public class KeyedServiceFactory<T> : IKeyedServiceFactory<T>
        where T : class, IServiceDefinition
{
    private readonly IServiceProvider serviceProvider;

    public KeyedServiceFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public T GetTenantServiceOrDefault(string key)
    {
        try
        {
            return serviceProvider.GetRequiredKeyedService<T>(key);
        }
        catch (InvalidOperationException)
        {
            return serviceProvider.GetRequiredKeyedService<T>("default");
        }
    }
}
