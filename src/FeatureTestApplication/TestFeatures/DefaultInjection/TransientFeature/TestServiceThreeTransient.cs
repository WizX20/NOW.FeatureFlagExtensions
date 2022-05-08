namespace FeatureTestApplication.TestFeatures.DefaultInjection.TransientFeature
{
    public class TestServiceThreeTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceThreeTransient()
        {
            Name = "Default " + nameof(TestServiceThreeTransient);
        }
    }
}