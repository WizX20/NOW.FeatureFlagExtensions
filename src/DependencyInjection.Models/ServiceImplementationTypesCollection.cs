using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Models
{
    public class ServiceImplementationTypesCollection : IServiceImplementationTypesCollection
    {
        /// <summary>
        /// The key is the type of the <see cref="{TService}"/> registered, and the value, the type of the implementation and assigned feature-flag.
        /// </summary>
        private readonly ConcurrentDictionary<Type, IEnumerable<(Type ImplementationType, string Feature)>> _serviceImplementationTypes = new();

        public bool ContainsKey(Type key)
        {
            return _serviceImplementationTypes.ContainsKey(key);
        }

        public IEnumerable<(Type ImplementationType, string Feature)> Get(Type key)
        {
            return _serviceImplementationTypes
                .Where(t => t.Key == key)
                .SelectMany(t => t.Value);
        }

        public IReadOnlyList<KeyValuePair<Type, IEnumerable<(Type ImplementationType, string Feature)>>> GetAll()
        {
            return _serviceImplementationTypes.ToImmutableList();
        }

        public KeyValuePair<Type, IEnumerable<(Type ImplementationType, string Feature)>> GetServiceTypeAssignableFrom(Type implementationType)
        {
            return _serviceImplementationTypes
                .Where(t => t.Key.IsAssignableFrom(implementationType))
                .SingleOrDefault();
        }

        public bool TryAdd(Type serviceType, Type implementationType, string feature)
        {
            // Add new.
            if (!_serviceImplementationTypes.ContainsKey(serviceType))
            {
                return _serviceImplementationTypes.TryAdd(
                    serviceType,
                    new List<(Type, string)> { (implementationType, feature) }
                );
            }

            // Update existing.
            var existingImplementationTypes = Get(serviceType);
            var hasExistingImplementationType = existingImplementationTypes.Any(t =>
                t.ImplementationType == implementationType &&
                t.Feature == feature
            );

            if (!hasExistingImplementationType)
            {
                var newImplementationTypes = existingImplementationTypes.ToList();
                newImplementationTypes.Add((implementationType, feature));

                return _serviceImplementationTypes.TryUpdate(serviceType, newImplementationTypes, existingImplementationTypes);
            }

            // Combination of types already exists.
            return true;
        }

        public bool TryRemove(Type key)
        {
            return _serviceImplementationTypes.TryRemove(key, out _);
        }
    }
}