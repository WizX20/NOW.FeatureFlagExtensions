namespace FeatureTestApplication.TestFeatures.DefaultInjection.TransientFeature
{
    public class TestServiceTwoTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceTwoTransient()
        {
            Name = "Default " + nameof(TestServiceTwoTransient);
        }
    }
}