namespace FeatureTestApplication.TestFeatures.InterceptorInjection.SingletonFeature
{
    public class TestServiceOneSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceOneSingleton()
        {
            Name = "Intercepted " + nameof(TestServiceOneSingleton);
        }
    }
}