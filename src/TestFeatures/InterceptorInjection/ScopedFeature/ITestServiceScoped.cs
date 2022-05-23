namespace TestFeatures.InterceptorInjection.ScopedFeature
{
    public interface ITestServiceScoped
    {
        string Name { get; }

        string GetTest();
    }
}