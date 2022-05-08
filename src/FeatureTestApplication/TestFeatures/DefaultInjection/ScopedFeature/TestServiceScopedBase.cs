namespace FeatureTestApplication.TestFeatures.DefaultInjection.ScopedFeature
{
    public abstract class TestServiceScopedBase : ITestServiceScoped
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string GetTest()
        {
            return $"This is the {Name} instance.";
        }
    }
}
