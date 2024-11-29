namespace TenantSpecific.Interfaces;

public interface ITenant<out TTenant>
{
    TTenant Value { get; }
}
