﻿namespace FeatureTestApplication.Services
{
    public abstract class TestServiceBase : ITestService
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string GetTest()
        {
            return $"This is the {Name} instance.";
        }
    }
}