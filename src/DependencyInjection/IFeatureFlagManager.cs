namespace NOW.FeatureFlagExtensions
{
    public interface IFeatureFlagManager
    {
        bool IsEnabled(string feature);

        Task<bool> IsEnabledAsync(string feature);
    }
}