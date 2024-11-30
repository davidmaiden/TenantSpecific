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
        return serviceProvider.GetKeyedService<T>(key) ?? serviceProvider.GetRequiredKeyedService<T>("default");
    }
}
