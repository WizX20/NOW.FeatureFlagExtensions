namespace NOW.FeatureFlagExtensions.DependencyInjection.Models
{
    public interface IServiceImplementationTypesCollection
    {
        bool ContainsKey(Type key);

        IEnumerable<(Type ImplementationType, string Feature)> Get(Type key);

        IReadOnlyList<KeyValuePair<Type, IEnumerable<(Type ImplementationType, string Feature)>>> GetAll();

        KeyValuePair<Type, IEnumerable<(Type ImplementationType, string Feature)>> GetServiceTypeAssignableFrom(Type implementationType);

        void AddOrUpdate(Type serviceType, Type implementationType, string feature);

        bool TryRemove(Type key);
    }
}