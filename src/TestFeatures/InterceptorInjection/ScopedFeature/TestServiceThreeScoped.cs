namespace TestFeatures.InterceptorInjection.ScopedFeature
{
    public class TestServiceThreeScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceThreeScoped()
        {
            Name = "Intercepted " + nameof(TestServiceThreeScoped);
        }
    }
}