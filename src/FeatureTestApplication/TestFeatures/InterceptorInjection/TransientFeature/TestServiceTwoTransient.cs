namespace FeatureTestApplication.TestFeatures.InterceptorInjection.TransientFeature
{
    public class TestServiceTwoTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceTwoTransient()
        {
            Name = "Intercepted " + nameof(TestServiceTwoTransient);
        }
    }
}