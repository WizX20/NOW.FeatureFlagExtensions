namespace FeatureTestApplication.TestFeatures.InterceptorInjection.SingletonFeature
{
    public class TestServiceThreeSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceThreeSingleton()
        {
            Name = "Intercepted " + nameof(TestServiceThreeSingleton);
        }
    }
}