namespace FeatureTestApplication.Configurations
{
    public interface ITestConfiguration
    {
        string Name { get; }

        string GetTest();
    }
}