namespace TestFeatures.DefaultInjection.SingletonFeature
{
    public class TestServiceTwoSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceTwoSingleton()
        {
            Name = "Default " + nameof(TestServiceTwoSingleton);
        }
    }
}