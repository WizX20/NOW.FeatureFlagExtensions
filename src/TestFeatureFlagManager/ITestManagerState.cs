namespace TestFeatureFlagManager
{
    public interface ITestManagerState
    {
        void Add(string feature, bool isEnabled);

        (string Feature, bool IsEnabled) Get(string feature);

        void Remove(string feature, out bool removedFeatureFlag);

        void Update(string feature, bool isEnabled);
    }
}