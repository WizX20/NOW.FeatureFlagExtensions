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
            public struct TestFeatures
            {
                public struct DefaultInjection
                {
                    public const string TestDefaultServiceTwoScoped = nameof(TestDefaultServiceTwoScoped);
                    public const string TestDefaultServiceThreeScoped = nameof(TestDefaultServiceThreeScoped);

                    public const string TestDefaultServiceTwoTransient = nameof(TestDefaultServiceTwoTransient);
                    public const string TestDefaultServiceThreeTransient = nameof(TestDefaultServiceThreeTransient);
                }

                public struct InterceptorInjection
                {
                    public const string TestInterceptedServiceTwoScoped = nameof(TestInterceptedServiceTwoScoped);
                    public const string TestInterceptedServiceThreeScoped = nameof(TestInterceptedServiceThreeScoped);

                    public const string TestInterceptedServiceTwoSingleton = nameof(TestInterceptedServiceTwoSingleton);
                    public const string TestInterceptedServiceThreeSingleton = nameof(TestInterceptedServiceThreeSingleton);

                    public const string TestInterceptedServiceTwoTransient = nameof(TestInterceptedServiceTwoTransient);
                    public const string TestInterceptedServiceThreeTransient = nameof(TestInterceptedServiceThreeTransient);
                }
            }
        }
    }
}