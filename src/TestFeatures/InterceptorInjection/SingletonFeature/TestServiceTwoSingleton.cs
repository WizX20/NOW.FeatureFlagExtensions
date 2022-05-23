namespace TestFeatures.InterceptorInjection.SingletonFeature
{
    public class TestServiceTwoSingleton : TestServiceSingletonBase
    {
        public override string Name { get; }

        public TestServiceTwoSingleton()
        {
            Name = "Intercepted " + nameof(TestServiceTwoSingleton);
        }
    }
}