namespace FeatureTestApplication
{
    /// <summary>
    /// Available feature flags/toggles, https://www.martinfowler.com/articles/feature-toggles.html
    /// </summary>
    public struct FeatureFlags
    {
        /// <summary>
        /// Experiment Toggles are used to perform multivariate or A/B testing.
        /// </summary>
        public struct ExperimentFlags
        {
        }

        /// <summary>
        /// Ops Toggles are used to control operational aspects of our system's behavior.
        /// </summary>
        public struct OpsFlags
        {
            public struct MyExample
            {
                /// <summary>
                /// Fake responses for all requests.
                /// </summary>
                public const string FakeResponses = nameof(FakeResponses);

                /// <summary>
                /// Simulate test exceptions.
                /// </summary>
                public const string SimulateTestExceptions = nameof(SimulateTestExceptions);
            }
        }

        /// <summary>
        /// Permission Toggles are used to change the features or product experience that
        /// certain users receive.
        /// </summary>
        public struct PermissionFlags
        {
        }

        /// <summary>
        /// Release Toggles allow incomplete and untested code-paths to be shipped to production,
        /// as latent code which may never be turned on.
        /// </summary>
        public struct ReleaseFlags
        {
            public struct TestFeature
            {
                public const string TestServiceTwo = nameof(TestServiceTwo);
                public const string TestServiceThree = nameof(TestServiceThree);

                public const string TestConfigurationTwo = nameof(TestConfigurationTwo);
                public const string TestConfigurationThree = nameof(TestConfigurationThree);
            }
        }
    }
}
