namespace FeatureTestApplication.TestFeatures.InterceptorInjection.ScopedFeature
{
    public class TestServiceOneScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceOneScoped()
        {
            Name = "Intercepted " + nameof(TestServiceOneScoped);
        }
    }
}