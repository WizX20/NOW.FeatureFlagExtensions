namespace NOW.FeatureFlagExtensions.DependencyInjection.Managers
{
    public interface IFeatureFlagManager
    {
        bool IsEnabled(string feature);

        Task<bool> IsEnabledAsync(string feature);
    }
}