using System.Collections.Concurrent;

namespace TestFeatureFlagManager
{
    public class TestManagerState : ITestManagerState
    {
        private readonly ConcurrentDictionary<string, bool> _featureFlags = new();

        public (string Feature, bool IsEnabled) Get(string feature)
        {
            if (!_featureFlags.TryGetValue(feature, out var isEnabled))
            {
                return (feature, false);
            }

            return (feature, isEnabled);
        }

        public void Add(string feature, bool isEnabled)
        {
            if (!_featureFlags.TryAdd(feature, isEnabled))
            {
                // Throwing for testing purposes.
                throw new InvalidOperationException($"Unable to add {nameof(feature)} `{feature}` with {nameof(isEnabled)} `{isEnabled}`");
            }
        }

        public void Update(string feature, bool isEnabled)
            => _featureFlags[feature] = isEnabled;

        public void Remove(string feature, out bool removedFeatureFlag)
        {
            if (!_featureFlags.TryRemove(feature, out removedFeatureFlag))
            {
                // Throwing for testing purposes.
                throw new InvalidOperationException($"Unable to remove {nameof(feature)} `{feature}`");
            }
        }
    }
}