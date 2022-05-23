namespace TestFeatures.DefaultInjection.TransientFeature
{
    public class TestServiceOneTransient : TestServiceTransientBase
    {
        public override string Name { get; }

        public TestServiceOneTransient()
        {
            Name = "Default " + nameof(TestServiceOneTransient);
        }
    }
}