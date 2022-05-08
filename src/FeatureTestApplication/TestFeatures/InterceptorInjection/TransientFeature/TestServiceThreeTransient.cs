namespace FeatureTestApplication.TestFeatures.InterceptorInjection.TransientFeature
{
    public class TestServiceThreeTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceThreeTransient()
        {
            Name = "Intercepted " + nameof(TestServiceThreeTransient);
        }
    }
}