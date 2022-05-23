namespace TestFeatures.DefaultInjection.ScopedFeature
{
    public class TestServiceTwoScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceTwoScoped()
        {
            Name = "Default " + nameof(TestServiceTwoScoped);
        }
    }
}