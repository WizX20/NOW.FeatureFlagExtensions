namespace FeatureTestApplication.TestFeatures.InterceptorInjection.TransientFeature
{
    public abstract class TestServiceTransientBase : ITestServiceTransient
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string GetTest()
        {
            return $"This is the {Name} instance.";
        }
    }
}
