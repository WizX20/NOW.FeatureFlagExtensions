using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.DependencyInjection.Managers;

namespace NOW.FeatureFlagExtensions.DependencyInjection.FeatureManagement
{
    public class FeatureFlagFeatureManagementManager : FeatureFlagManager
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagFeatureManagementManager(
            IFeatureManager featureManager)
        {
            if (featureManager == null)
            {
                throw new ArgumentNullException(nameof(featureManager));
            }

            _featureManager = featureManager;
        }

        public override bool IsEnabled(string feature)
        {
            return IsEnabledAsync(feature).GetAwaiter().GetResult();
        }

        public override Task<bool> IsEnabledAsync(string feature)
        {
            return _featureManager.IsEnabledAsync(feature);
        }
    }
}