namespace TestFeatures.DefaultInjection.ScopedFeature
{
    public class TestServiceThreeScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceThreeScoped()
        {
            Name = "Default " + nameof(TestServiceThreeScoped);
        }
    }
}