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
                .SingleOrDefault(t => t.Key.IsAssignableFrom(implementationType));
        }

        public void AddOrUpdate(Type serviceType, Type implementationType, string feature)
        {
            var addedStuff = _serviceImplementationTypes.AddOrUpdate(
                serviceType,
                new List<(Type, string)> { (implementationType, feature) },
                (key, existingImplementationTypes) =>
                {
                    var newImplementationTypes = existingImplementationTypes.ToList();
                    var hasExistingImplementationType = existingImplementationTypes.Any(t =>
                        t.ImplementationType == implementationType &&
                        t.Feature == feature
                    );

                    if (!hasExistingImplementationType)
                    {
                        // Make sure the last service added is the first one found.
                        newImplementationTypes.Insert(0, (implementationType, feature));
                    }

                    return newImplementationTypes;
                }
            );
        }

        public bool TryRemove(Type key)
        {
            return _serviceImplementationTypes.TryRemove(key, out _);
        }
    }
}