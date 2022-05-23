namespace TestFeatures.DefaultInjection.ScopedFeature
{
    public class TestServiceOneScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceOneScoped()
        {
            Name = "Default " + nameof(TestServiceOneScoped);
        }
    }
}