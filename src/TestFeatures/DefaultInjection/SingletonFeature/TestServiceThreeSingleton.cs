namespace TestFeatures.DefaultInjection.SingletonFeature
{
    public class TestServiceThreeSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceThreeSingleton()
        {
            Name = "Default " + nameof(TestServiceThreeSingleton);
        }
    }
}