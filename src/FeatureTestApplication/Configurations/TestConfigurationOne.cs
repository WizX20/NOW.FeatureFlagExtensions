namespace FeatureTestApplication.Configurations
{
    public class TestConfigurationOne : TestConfigurationBase
    {
        public override string Name { get; }

        public TestConfigurationOne()
        {
            Name = nameof(TestConfigurationOne);
        }
    }
}