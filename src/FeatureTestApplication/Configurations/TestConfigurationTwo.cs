namespace FeatureTestApplication.Configurations
{
    public class TestConfigurationTwo : TestConfigurationBase
    {
        public override string Name { get; }

        public TestConfigurationTwo()
        {
            Name = nameof(TestConfigurationTwo);
        }
    }
}