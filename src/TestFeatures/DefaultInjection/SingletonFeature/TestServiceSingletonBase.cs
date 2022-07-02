namespace TestFeatures.DefaultInjection.SingletonFeature
{
    public abstract class TestServiceSingletonBase : ITestServiceSingleton
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string GetTest()
        {
            return $"This is the {Name} instance.";
        }
    }
}
