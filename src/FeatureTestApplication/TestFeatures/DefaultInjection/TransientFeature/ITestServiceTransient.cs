namespace FeatureTestApplication.TestFeatures.DefaultInjection.TransientFeature
{
    public interface ITestServiceTransient
    {
        string Name { get; }

        string GetTest();
    }
}