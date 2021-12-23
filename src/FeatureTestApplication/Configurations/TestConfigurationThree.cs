namespace FeatureTestApplication.Configurations
{
    public class TestConfigurationThree : TestConfigurationBase
    {
        public override string Name { get; }

        public TestConfigurationThree()
        {
            Name = nameof(TestConfigurationThree);
        }
    }
}