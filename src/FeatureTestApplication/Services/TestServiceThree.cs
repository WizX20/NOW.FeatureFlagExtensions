namespace FeatureTestApplication.Services
{
    public class TestServiceThree : TestServiceBase
    {
        public override string Name { get; }

        public TestServiceThree()
        {
            Name = nameof(TestServiceThree);
        }
    }
}