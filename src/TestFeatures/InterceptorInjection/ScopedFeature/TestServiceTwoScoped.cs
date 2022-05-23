namespace TestFeatures.InterceptorInjection.ScopedFeature
{
    public class TestServiceTwoScoped : TestServiceScopedBase
    {
        public override string Name { get; }

        public TestServiceTwoScoped()
        {
            Name = "Intercepted " + nameof(TestServiceTwoScoped);
        }
    }
}