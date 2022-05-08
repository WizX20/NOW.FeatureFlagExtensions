namespace FeatureTestApplication.TestFeatures.InterceptorInjection.TransientFeature
{
    public class TestServiceOneTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceOneTransient()
        {
            Name = "Intercepted " + nameof(TestServiceOneTransient);
        }
    }
}