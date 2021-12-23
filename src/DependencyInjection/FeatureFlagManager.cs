namespace NOW.FeatureFlagExtensions
{
    public abstract class FeatureFlagManager : IFeatureFlagManager
    {
        public abstract bool IsEnabled(string feature);

        public abstract Task<bool> IsEnabledAsync(string feature);
    }
}