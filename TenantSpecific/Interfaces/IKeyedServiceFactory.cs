namespace TenantSpecific.Interfaces;

public interface IKeyedServiceFactory<TService>
    where TService : class, IServiceDefinition
{
    TService GetTenantServiceOrDefault(string key);
}
