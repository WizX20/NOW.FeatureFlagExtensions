using NOW.FeatureFlagExtensions.DependencyInjection.Managers;

namespace TestFeatureFlagManager
{
    public class TestManager : FeatureFlagManager
    {
        private readonly ITestManagerState _testManagerState;

        public TestManager(ITestManagerState testManagerState)
        {
            _testManagerState = testManagerState ?? throw new ArgumentNullException(nameof(testManagerState));
        }

        public override bool IsEnabled(string feature)
        {
            var featureFlag = _testManagerState.Get(feature);

            return featureFlag.IsEnabled;
        }

        public override Task<bool> IsEnabledAsync(string feature)
        {
            return Task.FromResult(IsEnabled(feature));
        }
    }
}