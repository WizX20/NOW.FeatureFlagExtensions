namespace FeatureTestApplication
{
    /// <summary>
    /// Available feature flags/toggles, https://www.martinfowler.com/articles/feature-toggles.html
    /// </summary>
    public static class FeatureFlags
    {
        /// <summary>
        /// Experiment Toggles are used to perform multivariate or A/B testing.
        /// </summary>
        public static class ExperimentFlags
        {
        }

        /// <summary>
        /// Ops Toggles are used to control operational aspects of our system's behavior.
        /// </summary>
        public static class OpsFlags
        {
            public static class MyExample
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
        public static class PermissionFlags
        {
        }

        /// <summary>
        /// Release Toggles allow incomplete and untested code-paths to be shipped to production,
        /// as latent code which may never be turned on.
        /// </summary>
        public static class ReleaseFlags
        {
            public static class TestFeatures
            {
                public static class DefaultInjection
                {
                    public const string TestDefaultServiceScopedTwo = nameof(TestDefaultServiceScopedTwo);
                    public const string TestDefaultServiceScopedThree = nameof(TestDefaultServiceScopedThree);

                    public const string TestDefaultServiceTransientTwo = nameof(TestDefaultServiceTransientTwo);
                    public const string TestDefaultServiceTransientThree = nameof(TestDefaultServiceTransientThree);
                }

                public static class InterceptorInjection
                {
                    public const string TestInterceptedServiceScopedTwo = nameof(TestInterceptedServiceScopedTwo);
                    public const string TestInterceptedServiceScopedThree = nameof(TestInterceptedServiceScopedThree);

                    public const string TestInterceptedServiceSingletonTwo = nameof(TestInterceptedServiceSingletonTwo);
                    public const string TestInterceptedServiceSingletonThree = nameof(TestInterceptedServiceSingletonThree);

                    public const string TestInterceptedServiceTransientTwo = nameof(TestInterceptedServiceTransientTwo);
                    public const string TestInterceptedServiceTransientThree = nameof(TestInterceptedServiceTransientThree);
                }
            }
        }
    }
}