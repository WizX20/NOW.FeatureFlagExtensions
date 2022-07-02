namespace TestFeatures.DefaultInjection.SingletonFeature
{
    public interface ITestServiceSingleton
    {
        string Name { get; }

        string GetTest();
    }
}