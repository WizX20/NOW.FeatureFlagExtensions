namespace FeatureTestApplication.Services
{
    public class TestServiceOne : TestServiceBase
    {
        public override string Name { get; }

        public TestServiceOne()
        {
            Name = nameof(TestServiceOne);
        }
    }
}