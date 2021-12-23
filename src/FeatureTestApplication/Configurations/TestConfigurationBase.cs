namespace FeatureTestApplication.Configurations
{
    public abstract class TestConfigurationBase : ITestConfiguration
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string GetTest()
        {
            return $"This is the {Name} instance.";
        }
    }
}
