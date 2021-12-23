namespace FeatureTestApplication.Services
{
    public class TestServiceTwo : TestServiceBase
    {
        public override string Name { get; }

        public TestServiceTwo()
        {
            Name = nameof(TestServiceTwo);
        }
    }
}