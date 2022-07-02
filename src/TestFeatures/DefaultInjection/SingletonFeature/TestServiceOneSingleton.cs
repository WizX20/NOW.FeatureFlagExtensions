namespace TestFeatures.DefaultInjection.SingletonFeature
{
    public class TestServiceOneSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceOneSingleton()
        {
            Name = "Default " + nameof(TestServiceOneSingleton);
        }
    }
}