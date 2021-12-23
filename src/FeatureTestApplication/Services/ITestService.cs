namespace FeatureTestApplication.Services
{
    public interface ITestService
    {
        string Name { get; }

        string GetTest();
    }
}